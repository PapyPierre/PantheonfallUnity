using System;
using UnityEngine;

namespace Core.Entity
{
    public abstract class Entity
    {
        public string EntityName;
        
        public EntityStats CurrentStats { get; }
        public EEntityStatus CurrentStatus { get; private set; }

        protected Entity(EntityStats stats, string entityName)
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
                accuracy = stats.accuracy,
                agility = stats.agility,
                intelligence = stats.intelligence,
            };

            EntityName = entityName;
        }

        public void SetMaxHp(int newValue)
        {
            CurrentStats.maxHp = newValue;

            if (CurrentStats.maxHp < CurrentStats.currentHp)
            {
                ApplyDamage(CurrentStats.currentHp - CurrentStats.maxHp);
            }

            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max HP changed to {CurrentStats.maxHp}!"));
        }

        public virtual void ApplyDamage(int damage)
        {
            CurrentStats.currentHp -= damage;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} has lost {damage} HP!"));

            if (CurrentStats.currentHp <= 0)
            {
                Kill();
            }
        }

        public void Heal(int value)
        {
            CurrentStats.currentHp += value;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} heals {value} HP!"));

            if (CurrentStats.currentHp > CurrentStats.maxHp) CurrentStats.currentHp = CurrentStats.maxHp;
            
            GameManager.instance.uiManager.PlayerStats.UpdatePlayerHp(CurrentStats);
        }

        public void SetHpRegen(int newValue)
        {
            CurrentStats.hpRegen = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s HP regeneration set to {newValue}!"));
        }

        public void SetMaxMana(int newValue)
        {
            CurrentStats.maxMana = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max MP set to {newValue}!"));

            if (CurrentStats.maxMana < CurrentStats.currentMana)
            {
                UseMana(CurrentStats.currentMana - CurrentStats.maxMana);
            }
        }

        public void UseMana(int value)
        {
            CurrentStats.currentMana -= value;
            
            if (CurrentStats.currentMana < 0)
            {
                CurrentStats.currentMana = 0;
                Debug.LogWarning("Used more mana than entity has");
            }
        }

        public void RecoverMana(int value)
        {
            CurrentStats.currentMana += value;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} recovered {value} MP!"));

            if (CurrentStats.currentMana > CurrentStats.maxMana)
            {
                CurrentStats.currentMana = CurrentStats.maxMana;
            }
        }

        public void SetManaRegen(int newValue)
        {
            CurrentStats.manaRegen = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s MP regeneration set to {newValue}!"));
        }

        public void SetArmor(int newValue)
        {
            CurrentStats.armor = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s armor set to {newValue}!"));
        }

        public void SetAgility(int newValue)
        {
            CurrentStats.agility = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s agility set to {newValue}!"));
        }

        public void SetIntelligence(int newValue)
        {
            CurrentStats.intelligence = newValue;
            
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s intelligence set to {newValue}!"));
        }

        public void Kill()
        {
            GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} has been kill!"));
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

        public void SetStatus(EEntityStatus newStatus)
        {
            CurrentStatus = newStatus;
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

        [Space] public int accuracy;
        
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
        Accuracy,
        Agility,
        Intelligence,
    }
}