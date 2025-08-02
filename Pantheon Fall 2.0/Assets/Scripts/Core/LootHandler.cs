using System;
using System.Collections.Generic;
using Core.Upgrade;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class LootHandler : MonoBehaviour
    {
        [SerializeField] private List<UpgradeRarity> upgradeRarities = new List<UpgradeRarity>();
        private readonly List<UpgradeData> m_allUpgradesData = new List<UpgradeData>();

        private void Start()
        {
            foreach (object upgrade in Enum.GetValues(typeof(EUpgrades)))
            {
                m_allUpgradesData.Add(DataManager.GetData<UpgradeData>(upgrade.ToString()));
            }
        }

        public List<EUpgrades> GetRandomLoot()
        {
            List<EUpgrades> loot = new List<EUpgrades>();

            for (int i = 0; i < 3; i++)
            {
                float r = Random.Range(0f, 100f);

                if (r < upgradeRarities[3].lootProba) loot.Add(GetRandomUpgradeWithRarity(EUpgradeRarity.Legendary));
                else if (r < upgradeRarities[2].lootProba) loot.Add(GetRandomUpgradeWithRarity(EUpgradeRarity.Rare));
                else if (r < upgradeRarities[1].lootProba)
                    loot.Add(GetRandomUpgradeWithRarity(EUpgradeRarity.Uncommon));
                else loot.Add(GetRandomUpgradeWithRarity(EUpgradeRarity.Common));
            }

            return loot;
        }

        public EUpgrades GetRandomUpgradeWithRarity(EUpgradeRarity rarity)
        {
            List<EUpgrades> loot = new List<EUpgrades>();

            foreach (var upgradeData in m_allUpgradesData)
            {
                if (upgradeData.rarity == rarity)
                {
                    loot.Add(upgradeData.upgrade);
                }
            }

            return loot[Random.Range(0, loot.Count)];
        }

        public Color GetUpgradeRarityColor(EUpgradeRarity rarity)
        {
            foreach (UpgradeRarity upgradeRarity in upgradeRarities)
            {
                if (upgradeRarity.rarity == rarity)
                {
                    return upgradeRarity.color;
                }
            }
            
            return Color.white;
        }
    }
}