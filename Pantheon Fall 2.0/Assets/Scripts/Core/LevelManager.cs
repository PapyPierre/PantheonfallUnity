using Core.Entity;
using UnityEngine;

namespace Core
{
    public class LevelManager : MonoBehaviour
    {
        private Enemy m_currentEnemy;
        
        [SerializeField] private SpriteRenderer enemyRenderer;

        private void Start()
        {
            EnemyData enemyData = DataManager.GetEntityData("EnemyTest");
            m_currentEnemy = new Enemy(enemyData.shortName, enemyData.fullName, enemyData.BaseStats);

            enemyRenderer.sprite = enemyData.sprite;
        }
    }
}
