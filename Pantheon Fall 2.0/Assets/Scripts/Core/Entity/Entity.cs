using System;
using UnityEngine;

namespace Core.Entity
{
    public abstract class Entity
    {
        private EntityStats CurrentStats { get; }
        private EEntityStatus CurrentStatus { get; set; }

        protected Entity(EntityStats stats)
        {
            CurrentStats = new EntityStats()
            {
                maxHp = stats.maxHp,
                currentHp = stats.maxHp, // Current is set to max
                hpRegen = stats.hpRegen,
                maxMana = stats.maxMana,
                currentMana = stats.maxMana, // Current is set to max
                manaRegen = stats.manaRegen,
                armor = stats.armor,
                agility = stats.agility,
                intelligence = stats.intelligence,
            };
        }

        public void ApplyDamage(int damage)
        {
             CurrentStats.currentHp -= damage;
             
             if (CurrentStats.currentHp <= 0)
             {
                Kill();
             }
        }

        public void Kill()
        {
            
        }

        public int GetCurrentHp()
        {
            return CurrentStats.currentHp;
        }

        public void Regenerate()
        {
            // HP
            CurrentStats.currentHp += CurrentStats.hpRegen;
            if (CurrentStats.currentHp > CurrentStats.maxHp) CurrentStats.currentHp = CurrentStats.maxHp;
            
            // Mana
            CurrentStats.currentMana += CurrentStats.manaRegen;
            if (CurrentStats.currentMana > CurrentStats.maxMana) CurrentStats.currentMana = CurrentStats.maxMana;
        }
    }

    [Serializable]
    public class EntityStats
    {
        [Header("Health")] public int maxHp;
        [HideInInspector] public int currentHp;
        public int hpRegen;
        
        [Header("Mana")] public int maxMana;
        [HideInInspector] public int currentMana;
        public int manaRegen;

        [Header("Other")] public int armor;

        [Space] public int agility;

        [Space] public int intelligence;
    }
    
    [Flags]
    public enum EEntityStatus
    {
        Poisoned = 1 << 0,
        Asleep = 1 << 1,
    }
    
    public enum EEntityStats
    {
        MaxHealth,
        Health,
        HealthRegen,
        MaxMana,
        Mana,
        ManaRegen,
        Armor,
        Agility,
        Intelligence,
    }
}