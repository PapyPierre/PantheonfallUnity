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
        FireballParchment,
        LifeGem,
        ManaGem,
        Boots,
        Gloves,
        Shield,
        LightingStrikeParchment,
        Fountain,
        LifePotion,
        ManaPotion,
        MysthicalLeaf,
        SacredPendant,
        Glasses,
        MythrilArmor,
        SongOfTheDeepParchment,
        TidalWaveParchment,
        TideBindParchment,
        WhirlwindParchment,
        SleepyMelodyParchment,
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