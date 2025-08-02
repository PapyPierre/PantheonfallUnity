using System;
using Core.Entity;
using Core.Entity.Ability;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Upgrade
{
    [Serializable]
    public enum EUpgrades
    {
        FireballParchment = 0,
        LifeGem = 1,
        ManaGem = 2,
        Boots = 3,
        Gloves = 4,
        Shield = 5,
        Grimoire = 6,
        Book = 7,
        LightingStrikeParchment = 8,
        Fountain = 9,
        LifePotion = 10,
        ManaPotion = 11,
        MysthicalLeaf = 12,
        SacredPendant = 13,
        WiseFistParchment = 14,
        Glasses = 15,
        MythrilArmor = 16,
    }
    
    [Serializable]
    public class UpgradeBonus
    {
        public EUpgradesBonus bonus;
        
        [ShowIf("ModifyStat"), AllowNesting] public EEntityStats targetedStat;
        [ShowIf("ModifyStat"), AllowNesting] public int value;
        
        [ShowIf("UnlockAbility"), AllowNesting] public EAbilities ability;
        
        public bool ModifyStat() => bonus ==  EUpgradesBonus.ModifyStat;
        public bool UnlockAbility() => bonus ==  EUpgradesBonus.UnlockAbility;
    }
    
    [Serializable]
    public enum EUpgradesBonus
    {
        None,
        ModifyStat,
        UnlockAbility,
        GiveItem, //TODO
    }
    
    [Serializable]
    public enum EUpgradeRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
    }
    
    [Serializable]
    public class UpgradeRarity
    {
        public EUpgradeRarity rarity;
        [Range(0, 100)] public float lootProba;
        public Color color;
    }
}