using System.Collections.Generic;
using Core.Entity;
using UnityEngine;

namespace Core
{
    public class FightManager : MonoBehaviour
    {
        private GameManager m_gm;
        public Player Player { get; private set; }
        public Enemy CurrentEnemy { get; private set; }
        
        [Header("Enemy"), SerializeField] private SpriteRenderer enemyRenderer;
        
        [Header("Player"), SerializeField] private EntityStats playerBaseStats;
        [SerializeField] private List<EAbilities> playerAbilitiesOnStart = new List<EAbilities>();
        
        private void Awake()
        {
            m_gm = GameManager.instance;
            m_gm.fightManager = this;
            m_gm.director.TickExectue += OnTickExecute;
        }

        private void Start()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            Player = new Player(playerBaseStats);
            m_gm.uiManager.PlayerStats.UpdatePlayerStats(playerBaseStats.maxHp, playerBaseStats.armor,
                playerBaseStats.maxMana, playerBaseStats.agility, playerBaseStats.intelligence);

            foreach (EAbilities ability in playerAbilitiesOnStart)
            {
                Player.UnlockAbility(ability);
            }
        }

        public void SetEnemy(EnemyData newEnemyData)
        {
            CurrentEnemy = new Enemy(newEnemyData.BaseStats);
            enemyRenderer.sprite = newEnemyData.sprite;
            m_gm.uiManager.EnemyInfo.UpdateEnemyInfo(newEnemyData);
        }

        private void OnTickExecute()
        {
            Player.Regenerate();
            CurrentEnemy.Regenerate();
        }
    }
}