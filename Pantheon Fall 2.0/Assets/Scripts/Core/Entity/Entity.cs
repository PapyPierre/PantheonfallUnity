using System;
using UnityEngine;

namespace Core.Entity
{
    public abstract class Entity
    {
        private EntityStats CurrentStats { get; }

        protected Entity(EntityStats stats)
        {
            CurrentStats = new EntityStats()
            {
                maxHealth = stats.maxHealth,
                currentHealth = stats.maxHealth, // Current is set to max
                healthRegen = stats.healthRegen,
                maxStamina = stats.maxStamina,
                currentStamina = stats.maxStamina, // Current is set to max
                staminaRegen = stats.staminaRegen,
                armor = stats.armor,
                agility = stats.agility,
                intelligence = stats.intelligence,
            };
        }
    }

    [Serializable]
    public struct EntityStats
    {
        [Header("Health")] public uint maxHealth;
        [HideInInspector] public uint currentHealth;
        public uint healthRegen;
        
        [Header("Stamina")] public uint maxStamina;
        [HideInInspector] public uint currentStamina;
        public uint staminaRegen;

        [Header("Other")] public uint armor;

        [Space] public uint agility;

        [Space] public uint intelligence;
    }
}