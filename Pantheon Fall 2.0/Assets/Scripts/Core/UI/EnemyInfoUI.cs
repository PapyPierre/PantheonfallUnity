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

        public void HideAllEnemyInfos()
        {
            enemyDisplayedName.gameObject.SetActive(false);
            enemyLifebar.gameObject.SetActive(false);
        }
        
        public void UpdateEnemyInfo(EnemyData data, EntityStats stats)
        {
            UpdateEnemyDisplayedName(data.fullName);
            UpdateEnemyLifeBar(stats.maxHp);
        }

        private void UpdateEnemyDisplayedName(string newName)
        {
            enemyDisplayedName.gameObject.SetActive(true);
            enemyDisplayedName.text = newName;
        }

        private void UpdateEnemyLifeBar(int newValue)
        {
            enemyLifebar.gameObject.SetActive(true);
            Vector2 sizeDelta = enemyLifebar.rectTransform.sizeDelta;
            sizeDelta.x = newValue * 20;
            enemyLifebar.rectTransform.sizeDelta = sizeDelta;
        }
    }
}