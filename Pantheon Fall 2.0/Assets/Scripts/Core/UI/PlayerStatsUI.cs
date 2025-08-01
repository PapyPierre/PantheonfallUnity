using System;
using Core.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Image[] playerHpIndicators;
        [SerializeField] private Image[] playerManaIndicators;
        [SerializeField] private TextMeshProUGUI acrTmp;
        [SerializeField] private TextMeshProUGUI agiTmp;
        [SerializeField] private TextMeshProUGUI intTmp;

        public void HideAllStats()
        {
            foreach (Image img in playerHpIndicators)
            {
                img.gameObject.SetActive(false);
            }

            foreach (Image img in playerManaIndicators)
            {
                img.gameObject.SetActive(false);
            }
            
            acrTmp.gameObject.SetActive(false);
            agiTmp.gameObject.SetActive(false);
            intTmp.gameObject.SetActive(false);
        }
            
        
        public void UpdatePlayerStats(EntityStats playerStats)
        {
            UpdatePlayerHp(playerStats);
            UpdatePlayerMana(playerStats);
            UpdatePlayerAcr(playerStats.accuracy);
            UpdatePlayerAgi(playerStats.agility);
            UpdatePlayerInt(playerStats.intelligence);
        }
        
        public void UpdatePlayerHp(EntityStats playerStats)
        {
            for (int index = 0; index < playerHpIndicators.Length; index++)
            {
                Image img = playerHpIndicators[index];

                if (index < playerStats.currentHp)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.green;
                }
                else if (index < playerStats.currentHp + playerStats.armor)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.cyan;
                }
                else if (index < playerStats.maxHp)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.grey;
                }
                else
                {
                    img.gameObject.SetActive(false);
                }
            }
        }
        
        public void UpdatePlayerMana(EntityStats playerStats)
        {
            for (int index = 0; index < playerManaIndicators.Length; index++)
            {
                Image img = playerManaIndicators[index];

                if (index < playerStats.currentMana)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.blue;
                }
                else if (index < playerStats.maxMana)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.grey;
                }
                else
                {
                    img.gameObject.SetActive(false);
                }
            }
        }
        
        public void UpdatePlayerAcr(int value)
        {
            acrTmp.gameObject.SetActive(true);
            acrTmp.text = $"ACR: {value}%";   
        }
        
        public void UpdatePlayerAgi(int value)
        {
            agiTmp.gameObject.SetActive(true);
            agiTmp.text = $"AGI: {value}%";   
        }
        
        public void UpdatePlayerInt(int value)
        {
            intTmp.gameObject.SetActive(true);
            intTmp.text = $"INT: {value}";   
        }
    }
}
