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

        public void UpdateEnemyInfo(EnemyData data)
        {
            UpdateEnemyDisplayedName(data.fullName);
            UpdateEnemyLifeBar(data.BaseStats.maxHp);
        }
        
        private void UpdateEnemyDisplayedName(string newName)
        {
            enemyDisplayedName.text = newName;
        }

        private void UpdateEnemyLifeBar(int newValue)
        {
            Vector2 sizeDelta = enemyLifebar.rectTransform.sizeDelta;
            sizeDelta.x = newValue * 20;
            enemyLifebar.rectTransform.sizeDelta = sizeDelta;
        }
    }
}
