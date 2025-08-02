using System;
using Core.Entity.Ability;
using Core.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Entity
{
    public abstract class Entity
    {
        protected GameManager m_gm;

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

            m_gm = GameManager.instance;
        }

        public void SetMaxHp(int newValue)
        {
            int oldValue = CurrentStats.maxHp;
            CurrentStats.maxHp = newValue;

            if (newValue > CurrentStats.maxHp)
            {
                int dif = newValue - oldValue;
                Heal(dif);
            }
            else if (newValue < oldValue)
            {
                if (CurrentStats.maxHp < CurrentStats.currentHp)
                {
                    ApplyDamage(CurrentStats.currentHp - CurrentStats.maxHp);
                }
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max HP changed from {oldValue} to {CurrentStats.maxHp}!"));
        }

        public virtual void ApplyDamage(int damage, Action feedback = null)
        {
            if (CurrentStats.armor >= damage)
            {
                CurrentStats.armor -= damage;
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                    new TextToDisplay($"{EntityName} has lost {damage} armor!", feedback));
                return;
            }
            
            if (CurrentStats.armor > 0)
            {
                damage -= CurrentStats.armor;
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                    new TextToDisplay($"{EntityName} has lost {CurrentStats.armor} armor!", feedback));
                CurrentStats.armor = 0;
            }
            
            CurrentStats.currentHp -= damage;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} has lost {damage} HP!", feedback));

            if (CurrentStats.currentHp <= 0) Kill();
        }

        public void Heal(int value)
        {
            CurrentStats.currentHp += value;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} heals {value} HP!"));

            if (CurrentStats.currentHp > CurrentStats.maxHp) CurrentStats.currentHp = CurrentStats.maxHp;

            m_gm.uiManager.PlayerStats.UpdatePlayerHp();
        }

        public void SetHpRegen(int newValue)
        {
            CurrentStats.hpRegen = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s HP regeneration set to {newValue}!"));
        }

        public void SetMaxMana(int newValue)
        {
            int oldValue = CurrentStats.maxMana;
            
            CurrentStats.maxMana = newValue;

            if (newValue > oldValue)
            {
                RecoverMana(newValue - oldValue);
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max MP changed from {oldValue} to {newValue}!"));

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

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} recovered {value} MP!"));

            if (CurrentStats.currentMana > CurrentStats.maxMana)
            {
                CurrentStats.currentMana = CurrentStats.maxMana;
            }
        }

        public void SetManaRegen(int newValue)
        {
            CurrentStats.manaRegen = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s MP regeneration set to {newValue}!"));
        }

        public void SetArmor(int newValue)
        {
            CurrentStats.armor = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s armor set to {newValue}!"));
        }

        public void SetAccuracy(int newValue)
        {
            CurrentStats.accuracy = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s accuracy set to {newValue}!"));
        }

        public void SetAgility(int newValue)
        {
            CurrentStats.agility = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s agility set to {newValue}!"));
        }

        public void SetIntelligence(int newValue)
        {
            CurrentStats.intelligence = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s intelligence set to {newValue}!"));
        }

        protected virtual void Kill()
        {
            Action feedback = m_gm.fightManager.EnemyDeathFeedback;
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} has been defeated!", feedback));
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

        public void CastAbility(AbilityData ability, Entity target)
        {
            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} cast {ability.ability.ToString()} on {target.EntityName}!"));

            // Miss (Accuracy)
            if (Random.Range(1, 101) > CurrentStats.accuracy)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay($"{EntityName} misses!"));
                return;
            }

            // Dodge (Agility)
            if (Random.Range(1, 101) < target.CurrentStats.agility)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay($"{target.EntityName} dodges!"));
                return;
            }

            foreach (AbilityEffect abilityEffect in ability.effects)
            {
                if (abilityEffect.ModifyStat())
                {
                    target.UpdateStat(abilityEffect.targetedStat, abilityEffect.value);
                }
                else
                    switch (abilityEffect.effect)
                    {
                        case EAbilityEffect.AddStatus:
                            target.SetStatus(target.CurrentStatus | abilityEffect.targetedStatus);
                            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                                new TextToDisplay(
                                    $"{EntityName} makes {target.EntityName} {abilityEffect.targetedStatus.ToString()}"));
                            break;
                        case EAbilityEffect.RemoveStatus:
                            target.SetStatus(target.CurrentStatus & ~abilityEffect.targetedStatus);
                            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                                new TextToDisplay(
                                    $"{target.EntityName} is no longer {abilityEffect.targetedStatus.ToString()}"));
                            break;
                    }
            }
        }

        public void UpdateStat(EEntityStats stat, int modifierValue, Action feedbackOnDamageReceive = null)
        {
            switch (stat)
            {
                case EEntityStats.MaxHealth:
                    SetMaxHp(CurrentStats.maxHp + modifierValue);
                    break;
                case EEntityStats.Health:
                    if (modifierValue >= 0) Heal(modifierValue);
                    else ApplyDamage(-modifierValue, 
                        feedbackOnDamageReceive); // -value cuz ApplyDamage() takes positive inputs
                    break;
                case EEntityStats.HealthRegen:
                    SetHpRegen(CurrentStats.hpRegen + modifierValue);
                    break;
                case EEntityStats.MaxMana:
                    SetMaxMana(CurrentStats.maxMana + modifierValue);
                    break;
                case EEntityStats.Mana:
                    if (modifierValue >= 0) RecoverMana(modifierValue);
                    else UseMana(-modifierValue); // -value cuz UseMana() takes positive inputs
                    break;
                case EEntityStats.ManaRegen:
                    SetManaRegen(CurrentStats.manaRegen + modifierValue);
                    break;
                case EEntityStats.Armor:
                    SetArmor(CurrentStats.armor + modifierValue);
                    break;
                case EEntityStats.Accuracy:
                    SetAccuracy(CurrentStats.accuracy + modifierValue);
                    break;
                case EEntityStats.Agility:
                    SetAgility(CurrentStats.agility + modifierValue);
                    break;
                case EEntityStats.Intelligence:
                    SetIntelligence(CurrentStats.intelligence + modifierValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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