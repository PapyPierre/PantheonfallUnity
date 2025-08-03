using System;
using Core.Entity;
using Core.Entity.Ability;
using Core.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class LootElementUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image borderBackground;

        private EUpgrades m_currentUpgrade;

        public void UpdateInfos(EUpgrades upgrade)
        {
            UpgradeData data = DataManager.GetData<UpgradeData>(upgrade.ToString());
            
            Color color = GameManager.instance.fightManager.LootHandler.GetUpgradeRarityColor(data.rarity);
            
            m_currentUpgrade = upgrade;
         
            titleText.text = data.upgradeName;
            titleText.color = color;
            icon.sprite = data.sprite;
            
            UpdateDescription(data);
            
            borderBackground.color = color;
        }

        private void UpdateDescription(UpgradeData data)
        {
            descriptionText.text = string.Empty;

            foreach (UpgradeBonus bonus in data.bonusList)
            {
                if (bonus.ModifyStat())
                {
                    switch (bonus.targetedStat)
                    {
                        case EEntityStats.Health when bonus.value > 0:
                            descriptionText.text += $"Heals {bonus.value} HP\n";
                            break;
                        case EEntityStats.Health:
                            descriptionText.text += $"Inflicts {bonus.value} HP\n";
                            break;
                        case EEntityStats.Mana when bonus.value > 0:
                            descriptionText.text += $"Makes you recover {bonus.value} MP\n";
                            break;
                        case EEntityStats.Mana:
                            descriptionText.text += $"Cost {bonus.value} MP to loot\n";
                            break;
                        default:
                        {
                            if (bonus.value > 0)
                            {
                                descriptionText.text += $"{bonus.targetedStat.ToString()} +{bonus.value}\n";
                            }
                            else
                            {
                                descriptionText.text += $"{bonus.targetedStat.ToString()} -{bonus.value}\n";
                            }

                            break;
                        }
                    }
                }
                
                if (bonus.UnlockAbility())
                {
                    AbilityData abilityData = DataManager.GetData<AbilityData>(bonus.ability.ToString());
                    
                    descriptionText.text += $"Unlock {abilityData.abilityName}\n";
                }
            }
        }

        public void OnLootSelected()
        {
            GameManager.instance.uiManager.LootScreen.OnUpgradeSelected(m_currentUpgrade);
        }
    }
}
