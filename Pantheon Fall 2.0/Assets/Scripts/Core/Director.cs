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

        public Action IntroTextFinished;

        private void Awake()
        {
            GameManager.instance.director = this;
            IntroTextFinished += OnIntroFinished;
        }

        private void OnIntroFinished()
        {
            EnemyData enemyData = DataManager.GetData<EnemyData>(tiersInLevel[0].enemies[0].ToString());
            GameManager.instance.fightManager.StartFirstFight(enemyData);
        }
    }

    [Serializable]
    public class Tier
    {
        public List<EEnemies> enemies = new List<EEnemies>();
    }
}