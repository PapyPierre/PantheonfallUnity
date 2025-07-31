using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Image[] playerHpIndicators;
        [SerializeField] private Image[] playerManaIndicators;
        [SerializeField] private TextMeshProUGUI agiTmp;
        [SerializeField] private TextMeshProUGUI intTmp;

        public void UpdatePlayerStats(int hp, int armor, int mana, int agi, int inte)
        {
            UpdatePlayerHp(hp, armor);
            UpdatePlayerMana(mana);
            UpdatePlayerAgi(agi);
            UpdatePlayerInt(inte);
        }
        
        public void UpdatePlayerHp(int hp, int armor)
        {
            for (int index = 0; index < playerHpIndicators.Length; index++)
            {
                Image img = playerHpIndicators[index];

                if (index < hp)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.green;
                }
                else if (index < hp + armor)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.cyan;
                }
                else
                {
                    img.gameObject.SetActive(false);
                }
            }
        }
        
        public void UpdatePlayerMana(int value)
        {
            for (int index = 0; index < playerManaIndicators.Length; index++)
            {
                Image img = playerManaIndicators[index];

                if (index < value)
                {
                    img.gameObject.SetActive(true);
                }
                else
                {
                    img.gameObject.SetActive(false);
                }
            }
        }
        
        public void UpdatePlayerAgi(int value)
        {
            agiTmp.text = $"AGI: {value}%";   
        }
        
        public void UpdatePlayerInt(int value)
        {
            intTmp.text = $"INT: {value}";   
        }
    }
}
