using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class FightManager : MonoBehaviour
    {
        private GameManager m_gm;
        public Player Player { get; private set; }
        public Enemy CurrentEnemy { get; private set; }

        [Header("Enemy"), SerializeField] private SpriteRenderer enemyRenderer;
        private EAbilities m_enemyAbilityThisTurn;

        [Header("Player"), SerializeField] private EntityStats playerBaseStats;
        [SerializeField] private List<EAbilities> playerAbilitiesOnStart = new List<EAbilities>();
        private EAbilities m_playerAbilityThisTurn;

        public int TurnNumber { get; private set; }
        public int TickNumber { get; private set; }

        public Action<int> TurnPass;
        public Action TickExecute;
        
        public const int k_FightTextShowDuration = 2000;

        private void Awake()
        {
            m_gm = GameManager.instance;
            m_gm.fightManager = this;
            TickExecute += OnTickExecute;
            enemyRenderer.sprite = null;
        }

        public void StartFirstFight(EnemyData firstEnemyData)
        {
            InitializePlayer();
            SetEnemy(firstEnemyData);
            m_gm.uiManager.TurnInfo.EnableTurnNumberText();
            StartNextTurn();
        }

        private void InitializePlayer()
        {
            Player = new Player(playerBaseStats, "Player");
            m_gm.uiManager.PlayerStats.UpdatePlayerStats(Player.CurrentStats);

            foreach (EAbilities ability in playerAbilitiesOnStart)
            {
                Player.UnlockAbility(ability);
            }
        }

        public void SetEnemy(EnemyData newEnemyData)
        {
            CurrentEnemy = new Enemy(newEnemyData);
            enemyRenderer.sprite = newEnemyData.sprite;
            m_gm.uiManager.EnemyInfo.UpdateEnemyInfo(newEnemyData, CurrentEnemy.CurrentStats);
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(newEnemyData.AppearingText);
        }

        private void OnTickExecute()
        {
            Player.Regenerate();
            CurrentEnemy.Regenerate();
        }

        private void StartNextTurn()
        {
            ResetEntitiesAbilitiesThisTurn();
            
            TurnNumber++;
          
            IncrementTick();
            
            TurnPass.Invoke(TurnNumber);

            SetEnemyAbilityOfThisTurn(CurrentEnemy.GetAbilityToUse());

            m_gm.uiManager.TextArea.DisplayActions();
        }

        private void IncrementTick()
        {
            TickNumber++;

            if (TickNumber == 4)
            {
                TickExecute.Invoke();
                TickNumber = 0;
            }
        }

        public void SetPlayerAbilityOfThisTurn(EAbilities ability)
        {
            m_playerAbilityThisTurn = ability;
            CheckForTurnResolution();
        }

        public void SetEnemyAbilityOfThisTurn(EAbilities ability)
        {
            m_enemyAbilityThisTurn = ability;
            CheckForTurnResolution();
        }

        private void ResetEntitiesAbilitiesThisTurn()
        {
            m_playerAbilityThisTurn = EAbilities.None;
            m_enemyAbilityThisTurn = EAbilities.None;
        }

        private async void CheckForTurnResolution()
        {
            if (m_playerAbilityThisTurn == EAbilities.None || m_enemyAbilityThisTurn == EAbilities.None) return;
            await ResolvesTurn();
        }

        private async Task ResolvesTurn()
        {
            AbilityData playerAbility = DataManager.GetData<AbilityData>(m_playerAbilityThisTurn.ToString());
            AbilityData enemyAbility = DataManager.GetData<AbilityData>(m_enemyAbilityThisTurn.ToString());

            bool enemyPlaysFirst = enemyAbility.speed > playerAbility.speed;

            if (enemyPlaysFirst)
            {
                await ResolvesAbility(enemyAbility, CurrentEnemy);
                await ResolvesAbility(playerAbility, Player);
            }
            else
            {
                await ResolvesAbility(playerAbility, Player);
                await ResolvesAbility(enemyAbility, CurrentEnemy);
            }

            StartNextTurn();
        }

        private async Task ResolvesAbility(AbilityData ability, Entity.Entity source)
        {
            switch (ability.targets)
            {
                case EAbilityTarget.Self:
                    if (source == Player) await CastAbility(ability, source, Player);
                    else await CastAbility(ability, source, CurrentEnemy);
                    break;
                default:
                case EAbilityTarget.Opponent:
                    if (source == Player) await CastAbility(ability, source, CurrentEnemy);
                    else await CastAbility(ability, source, Player);
                    break;
                case EAbilityTarget.Everyone:
                    await CastAbility(ability, source, Player);
                    await CastAbility(ability, source, CurrentEnemy);
                    break;
            }
        }

        private async Task CastAbility(AbilityData ability, Entity.Entity source, Entity.Entity target)
        {
            if (ability.ability == EAbilities.StandBy)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                    new TextToDisplay($"{source.EntityName} stand by!"));
                m_gm.uiManager.TextArea.ShowNextTextInQueue();
                await Task.Delay(k_FightTextShowDuration);
                return;
            }
            
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{source.EntityName} cast {ability.ability.ToString()} on {target.EntityName}!"));

            await Task.Delay(k_FightTextShowDuration);
            
            // Misses (Accuracy)
            if (Random.Range(1, 101) > source.CurrentStats.accuracy)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay($"{source.EntityName} misses!"));
                m_gm.uiManager.TextArea.ShowNextTextInQueue();
                await Task.Delay(k_FightTextShowDuration);
                return;
            }

            // Misses (Agility)
            if (Random.Range(1, 101) < target.CurrentStats.agility)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay($"{source.EntityName} misses!"));
                m_gm.uiManager.TextArea.ShowNextTextInQueue();
                await Task.Delay(k_FightTextShowDuration);
                return;
            }

            foreach (AbilityEffect abilityEffect in ability.effects)
            {
                if (abilityEffect.ModifyStat())
                {
                    int value = abilityEffect.value;

                    switch (abilityEffect.targetedStat)
                    {
                        case EEntityStats.MaxHealth:
                            target.SetMaxHp(target.CurrentStats.maxHp += value);
                            break;
                        case EEntityStats.Health:
                            if (value > 0) target.Heal(value);
                            else target.ApplyDamage(-value); // -value cuz ApplyDamage() takes positive inputs
                            break;
                        case EEntityStats.HealthRegen:
                            target.SetHpRegen(target.CurrentStats.hpRegen += value);
                            break;
                        case EEntityStats.MaxMana:
                            target.SetMaxMana(target.CurrentStats.maxMana += value);
                            break;
                        case EEntityStats.Mana:
                            if (value > 0) target.RecoverMana(value);
                            else target.UseMana(-value); // -value cuz UseMana() takes positive inputs
                            break;
                        case EEntityStats.ManaRegen:
                            target.SetManaRegen(target.CurrentStats.manaRegen += value);
                            break;
                        case EEntityStats.Armor:
                            target.SetArmor(target.CurrentStats.armor += value);
                            break;
                        case EEntityStats.Agility:
                            target.SetAgility(target.CurrentStats.agility += value);
                            break;
                        case EEntityStats.Intelligence:
                            target.SetIntelligence(target.CurrentStats.intelligence += value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    m_gm.uiManager.TextArea.ShowNextTextInQueue();
                }
                else
                    switch (abilityEffect.effect)
                    {
                        case EAbilityEffect.AddStatus:
                            target.SetStatus(target.CurrentStatus | abilityEffect.targetedStatus);
                            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                                new TextToDisplay(
                                    $"{source.EntityName} makes {target.EntityName} {abilityEffect.targetedStatus.ToString()}"));
                            m_gm.uiManager.TextArea.ShowNextTextInQueue();
                            break;
                        case EAbilityEffect.RemoveStatus:
                            target.SetStatus(target.CurrentStatus & ~abilityEffect.targetedStatus);
                            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                                new TextToDisplay($"{target.EntityName} is no longer {abilityEffect.targetedStatus.ToString()}"));
                            m_gm.uiManager.TextArea.ShowNextTextInQueue();
                            break;
                    }
            }

            await Task.Delay(k_FightTextShowDuration);
        }

        public async void FeedbackDamageOnEnemy()
        {
            for (int i = 0; i < 3; i++)
            {
                enemyRenderer.color = Color.red;
                await Task.Delay(250);
                enemyRenderer.color = Color.white;
                await Task.Delay(150);
            }
        }
    }
}