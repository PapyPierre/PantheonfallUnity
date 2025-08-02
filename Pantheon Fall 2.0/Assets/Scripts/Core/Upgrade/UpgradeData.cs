using System.Collections.Generic;
using UnityEngine;

namespace Core.Upgrade
{
    [CreateAssetMenu(fileName = "Upgrade Data", menuName = "ScriptableObjects/Upgrade Data", order = 1)] 
    public class UpgradeData : ScriptableObject
    {
        public EUpgrades upgrade;
        public string upgradeName;
        public Sprite sprite;
        public EUpgradeRarity rarity;
        public List<UpgradeBonus> bonusList;
    }
}