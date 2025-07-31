using System;
using System.Collections.Generic;
using Core.Entity;
using UnityEngine;

namespace Core
{
    public class Director : MonoBehaviour
    {
        [SerializeField] private List<Tier> tiersInLevel = new List<Tier>();
        
        private int m_floorNumber;
        private int m_turnNumber;
        private int m_tickNumber;

        public Action<int> TurnPass;
        public Action TickExectue;

        private void Awake()
        {
            GameManager.instance.director = this;
        }

        private void Start()
        {
            EnemyData enemyData = DataManager.GetData<EnemyData>(tiersInLevel[0].enemies[0].ToString());
            GameManager.instance.fightManager.SetEnemy(enemyData);
            
            StartNextTurn();
        }

        private void StartNextTurn()
        {
            m_turnNumber++;
            m_tickNumber++;
            
            TurnPass.Invoke(m_turnNumber);
            
            if (m_tickNumber == 4)
            {
                TickExectue.Invoke();
                m_tickNumber = 0;
            }
        }
    }

    [Serializable]
    public class Tier
    {
        public List<EEnemies> enemies = new List<EEnemies>();
    }
}