using Core.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class EnemyInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI enemyDisplayedName;
        [SerializeField] private Image enemyLifebar;

        private Enemy GetCurrentEnemy()
        {
           return GameManager.instance.fightManager.CurrentEnemy;
        }
        
        public void ShowAllEnemyInfos()
        {
            enemyDisplayedName.gameObject.SetActive(true);
            enemyLifebar.gameObject.SetActive(true);
        }
        
        public void HideAllEnemyInfos()
        {
            enemyDisplayedName.gameObject.SetActive(false);
            enemyLifebar.gameObject.SetActive(false);
        }
        
        public void UpdateAllEnemyInfo()
        {
            UpdateEnemyDisplayedName(GetCurrentEnemy().Data.fullName);
            UpdateEnemyLifeBar();
        }

        private void UpdateEnemyDisplayedName(string newName)
        {
            enemyDisplayedName.text = newName;
        }

        public void UpdateEnemyLifeBar()
        {
            Vector2 sizeDelta = enemyLifebar.rectTransform.sizeDelta;
            sizeDelta.x = GetCurrentEnemy().CurrentStats.currentHp * 20;
            enemyLifebar.rectTransform.sizeDelta = sizeDelta;
        }
    }
}