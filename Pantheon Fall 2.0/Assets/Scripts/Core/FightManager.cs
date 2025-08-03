using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entity;
using Core.Entity.Ability;
using Core.UI;
using UnityEngine;

namespace Core
{
    public class FightManager : MonoBehaviour
    {
        private GameManager m_gm;
        public Player Player { get; private set; }
        public Enemy CurrentEnemy { get; private set; }

        [Header("Enemy"), SerializeField] private SpriteRenderer enemyRenderer;
        private EAbilities m_enemyAbilityThisTurn;
        [HideInInspector] public bool enemyHasBeenKilled;

        [Header("Player"), SerializeField] private EntityStats playerBaseStats;
        [SerializeField] private List<EAbilities> playerAbilitiesOnStart = new List<EAbilities>();
        private EAbilities m_playerAbilityThisTurn;

        public int TurnNumber { get; private set; }

        public Action<int> TurnPass;

        public LootHandler LootHandler { get; private set; }

        private Action m_enemyAppears;
        private Action m_startNextTurn;

        private void Awake()
        {
            m_gm = GameManager.instance;
            m_gm.fightManager = this;
            LootHandler = GetComponent<LootHandler>();
            m_enemyAppears += OnEnemyAppears;
            m_startNextTurn += StartNextTurn;
        }

        private void Start()
        {
            InitializePlayer();
            HideEnemy();
        }

        public void StartFirstFight(EnemyData firstEnemyData)
        {
            SetEnemy(firstEnemyData);
            m_gm.uiManager.TurnInfo.EnableTurnNumberText();
        }
        
        private void InitializePlayer()
        {
            Player = new Player(playerBaseStats, "Player");
            m_gm.uiManager.PlayerStats.SetPlayerRef(Player);
            m_gm.uiManager.PlayerStats.UpdateAllPlayerStats();

            foreach (EAbilities ability in playerAbilitiesOnStart)
            {
                Player.UnlockAbility(ability);
            }
        }

        public void SetEnemy(EnemyData newEnemyData)
        {
            if (newEnemyData == null)
            {
                Debug.LogWarning($"newEnemyData is null");
                return;
            }
            
            enemyHasBeenKilled = false;
            CurrentEnemy = new Enemy(newEnemyData);
            enemyRenderer.sprite = newEnemyData.sprite;
            m_gm.uiManager.EnemyInfo.ShowAllEnemyInfos();
            m_gm.uiManager.EnemyInfo.UpdateAllEnemyInfo();
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay(newEnemyData.AppearingText,
                m_enemyAppears));
        }

        private void OnEnemyAppears()
        {
            DelayStartNextTurn();
        }

        private void DelayStartNextTurn()
        {
            //TODO Use a better way to do this
            //this adds an empty text that will automatically calls the action which will then call DisplayActions()
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay(string.Empty, m_startNextTurn));
        }

        private void StartNextTurn()
        {
            ResetEntitiesAbilitiesThisTurn();

            if (CurrentEnemy == null) return;

            TurnNumber++;

            Player.Regenerate();
            CurrentEnemy.Regenerate();

            TurnPass.Invoke(TurnNumber);

            SetEnemyAbilityOfThisTurn(CurrentEnemy.GetAbilityToUse());

            m_gm.uiManager.TextArea.DisplayActions();
        }

        public void TrySetNextEnemy()
        {
            if (CurrentEnemy == null && TurnNumber > 1) SetEnemy(m_gm.director.GetNextEnemy());
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

        private void CheckForTurnResolution()
        {
            if (m_playerAbilityThisTurn == EAbilities.None || m_enemyAbilityThisTurn == EAbilities.None) return;
            ResolvesTurn();
        }

        private void ResolvesTurn()
        {
            m_gm.uiManager.TextArea.HideActions();

            AbilityData playerAbility = DataManager.GetData<AbilityData>(m_playerAbilityThisTurn.ToString());
            AbilityData enemyAbility = DataManager.GetData<AbilityData>(m_enemyAbilityThisTurn.ToString());

            bool enemyPlaysFirst = enemyAbility.speed > playerAbility.speed;

            if (enemyPlaysFirst)
            {
                ResolvesAbility(enemyAbility, CurrentEnemy);
                ResolvesAbility(playerAbility, Player);
            }
            else
            {
                ResolvesAbility(playerAbility, Player);
                if (enemyHasBeenKilled) return;
                ResolvesAbility(enemyAbility, CurrentEnemy);
            }
            
            m_gm.uiManager.TextArea.DisplayText();

            DelayStartNextTurn();
        }

        private void ResolvesAbility(AbilityData ability, Entity.Entity caster)
        {
            switch (ability.targets)
            {
                case EAbilityTarget.Self:
                    if (caster == Player) caster.CastAbility(ability, Player);
                    else caster.CastAbility(ability, CurrentEnemy);
                    break;
                default:
                case EAbilityTarget.Opponent:
                    if (caster == Player) caster.CastAbility(ability, CurrentEnemy);
                    else caster.CastAbility(ability, Player);
                    break;
                case EAbilityTarget.Everyone:

                    if (caster == Player)
                    {
                        caster.CastAbility(ability, CurrentEnemy);
                        caster.CastAbility(ability, Player);
                    }
                    else
                    {
                        caster.CastAbility(ability, Player);
                        caster.CastAbility(ability, CurrentEnemy);
                    }
                    break;
            }
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

            if (CurrentEnemy != null)
            {
                m_gm.uiManager.EnemyInfo.UpdateEnemyLifeBar();
            }
        }

        public void EnemyDeathFeedback()
        {
            HideEnemy();
            m_gm.uiManager.LootScreen.ShowLootScreen(LootHandler.GetRandomLoot());
            m_gm.director.GoUp();
            CurrentEnemy = null;
        }

        public void HideEnemy()
        {
            m_gm.uiManager.EnemyInfo.HideAllEnemyInfos();
            enemyRenderer.sprite = null;
        }
    }
}