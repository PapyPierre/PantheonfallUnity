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

        private void Start()
        {
            GameManager.instance.gameIsStarted = true;
        }

        private void OnIntroFinished()
        {
            Debug.Log("Intro Finished");
            GameManager.instance.fightManager.StartFirstFight(GetNextEnemy());
        }

        public void GoUp()
        {
            m_floorNumber++;
            //TODO Reset turn and tick number
        }

        public EnemyData GetNextEnemy()
        {
            return DataManager.GetData<EnemyData>(tiersInLevel[0].enemies[m_floorNumber].ToString());
        }
    }

    [Serializable]
    public class Tier
    {
        public List<EEnemies> enemies = new List<EEnemies>();
    }
}