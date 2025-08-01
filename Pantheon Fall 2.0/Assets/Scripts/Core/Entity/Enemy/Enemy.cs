using UnityEngine;

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
            return Data.KnownAbilities[index];
        }

        public override void ApplyDamage(int damage)
        {
            base.ApplyDamage(damage);
            GameManager.instance.fightManager.FeedbackDamageOnEnemy();
            GameManager.instance.uiManager.EnemyInfo.UpdateEnemyInfo(Data, CurrentStats);
        }
    }
}