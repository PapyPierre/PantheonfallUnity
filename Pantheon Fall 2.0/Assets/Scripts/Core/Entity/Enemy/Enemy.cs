using System;
using Core.Entity.Ability;
using Core.UI;
using Random = UnityEngine.Random;

namespace Core.Entity
{
    public class Enemy : Entity
    {
        public EnemyData Data { get; private set; }

        public Enemy(EnemyData data) : base(data.BaseStats, data.enemy.ToString())
        {
            Data = data;
        }

        public EAbilities GetAbilityToUse() //TODO Improve AI
        {
            int index = Random.Range(0, Data.KnownAbilities.Count);

            // /!\ infinite loop possible if all abilities use mana /!\
            if (DataManager.GetData<AbilityData>(Data.KnownAbilities[index].ToString()).manaCost > CurrentStats.currentMana)
            {
                return GetAbilityToUse();
            }
            
            return Data.KnownAbilities[index];
        }

        public override void ApplyDamage(int damage, Action feedback = null)
        {
            feedback += m_gm.fightManager.FeedbackDamageOnEnemy;
            base.ApplyDamage(damage, feedback); 
        }

        protected override void Kill()
        {
            m_gm.fightManager.enemyHasBeenKilled = true;
            Action feedback = m_gm.fightManager.EnemyDeathFeedback;
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} has been defeated!", feedback));
        }
    }
}