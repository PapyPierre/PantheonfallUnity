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

        public int turnBeingAsleep;

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
            };

            EntityName = entityName;

            m_gm = GameManager.instance;
        }

        public bool HasStatus(EEntityStatus status)
        {
            return (CurrentStatus & status) > 0;
        }

        public void SetMaxHp(int newValue, Action feedback)
        {
            int oldValue = CurrentStats.maxHp;
            CurrentStats.maxHp = newValue;

            if (newValue > oldValue)
            {
                int dif = newValue - oldValue;
                Heal(dif, feedback);
            }
            else if (newValue < oldValue)
            {
                if (CurrentStats.maxHp < CurrentStats.currentHp)
                {
                    ApplyDamage(CurrentStats.currentHp - CurrentStats.maxHp, feedback);
                }
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max HP changed from {oldValue} to {CurrentStats.maxHp}!", feedback));
        }

        public virtual void ApplyDamage(int damage, Action feedback)
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

        public void Heal(int value, Action feedback)
        {
            CurrentStats.currentHp += value;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} heals {value} HP!", feedback));

            if (CurrentStats.currentHp > CurrentStats.maxHp) CurrentStats.currentHp = CurrentStats.maxHp;

            m_gm.uiManager.PlayerStats.UpdatePlayerHp();
        }

        public void SetHpRegen(int newValue, Action feedback)
        {
            CurrentStats.hpRegen = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s HP regeneration set to {newValue}!", feedback));
        }

        public void SetMaxMana(int newValue, Action feedback)
        {
            int oldValue = CurrentStats.maxMana;

            CurrentStats.maxMana = newValue;

            if (newValue > oldValue)
            {
                RecoverMana(newValue - oldValue, feedback);
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s max MP changed from {oldValue} to {newValue}!", feedback));

            if (CurrentStats.maxMana < CurrentStats.currentMana)
            {
                UseMana(CurrentStats.currentMana - CurrentStats.maxMana);
            }
        }

        public virtual void UseMana(int value)
        {
            CurrentStats.currentMana -= value;

            if (CurrentStats.currentMana < 0)
            {
                CurrentStats.currentMana = 0;
                Debug.LogWarning("Used more mana than entity has");
            }
            
            GameManager.instance.fightManager.Player.UpdateUIStats();
        }

        public void RecoverMana(int value, Action feedback)
        {
            CurrentStats.currentMana += value;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} recovers {value} MP!", feedback));

            if (CurrentStats.currentMana > CurrentStats.maxMana)
            {
                CurrentStats.currentMana = CurrentStats.maxMana;
            }
        }

        public void SetManaRegen(int newValue, Action feedback)
        {
            CurrentStats.manaRegen = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s MP regeneration set to {newValue}!", feedback));
        }

        public void SetArmor(int newValue, Action feedback)
        {
            CurrentStats.armor = newValue;

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName}'s armor set to {newValue}!", feedback));
        }

        public void SetAccuracy(int newValue, Action feedback)
        {
            int oldValue = CurrentStats.accuracy;
            CurrentStats.accuracy = newValue;

            int dif = newValue - oldValue;

            if (CurrentStats.accuracy < 0)
            {
                CurrentStats.accuracy = 0;
            }

            if (CurrentStats.accuracy > 100)
            {
                CurrentStats.accuracy = 100;
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                dif > 0
                    ? new TextToDisplay($"{EntityName} gained {dif} accuracy point!", feedback)
                    : new TextToDisplay($"{EntityName} lost {dif} accuracy point!", feedback));
        }

        public void SetAgility(int newValue, Action feedback)
        {
            int oldValue = CurrentStats.agility;
            CurrentStats.agility = newValue;

            int dif = newValue - oldValue;

            if (CurrentStats.agility < 0)
            {
                CurrentStats.agility = 0;
            }

            if (CurrentStats.agility > 100)
            {
                CurrentStats.agility = 100;
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                dif > 0
                    ? new TextToDisplay($"{EntityName} gained {dif} agility point!", feedback)
                    : new TextToDisplay($"{EntityName} lost {dif} agility point!", feedback));
        }

        protected virtual void Kill()
        {
        }

        public void Regenerate()
        {
            // HP
            if (CurrentStats.hpRegen > 0)
            {
                if (CurrentStats.currentHp + CurrentStats.hpRegen <=  CurrentStats.maxHp)
                {
                    m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                        new TextToDisplay($"{EntityName} regenerated {CurrentStats.hpRegen} HP!"));
                }
                else if (CurrentStats.currentHp < CurrentStats.maxHp)
                {
                    int dif = CurrentStats.maxHp - CurrentStats.currentHp;
                    m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                        new TextToDisplay($"{EntityName} regenerated {dif} HP!"));
                }
                
                CurrentStats.currentHp += CurrentStats.hpRegen;
                if (CurrentStats.currentHp > CurrentStats.maxHp) CurrentStats.currentHp = CurrentStats.maxHp;
              
            }
            
            // Mana
            if (CurrentStats.manaRegen > 0)
            {
                if (CurrentStats.currentMana + CurrentStats.manaRegen <=  CurrentStats.maxMana)
                {
                    m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                        new TextToDisplay($"{EntityName} regenerated {CurrentStats.manaRegen} MP!"));
                }
                else if (CurrentStats.currentMana < CurrentStats.maxMana)
                {
                    int dif = CurrentStats.maxMana - CurrentStats.currentMana;
                    m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                        new TextToDisplay($"{EntityName} regenerated {dif} MP!"));
                }
                
                CurrentStats.currentMana += CurrentStats.manaRegen;
                if (CurrentStats.currentMana > CurrentStats.maxMana) CurrentStats.currentMana = CurrentStats.maxMana;
            }
        }

        public void SetStatus(EEntityStatus newStatus)
        {
            CurrentStatus = newStatus;

            switch (newStatus)
            {
                case EEntityStatus.Asleep:
                    turnBeingAsleep = 0;
                    break;
            }
        }

        public void CastAbility(AbilityData ability, Entity target)
        {
            if (HasStatus(EEntityStatus.Asleep))
            {
                switch (turnBeingAsleep)
                {
                    case < 3:
                        m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                            new TextToDisplay($"{EntityName} is asleep!"));
                        turnBeingAsleep++;
                        return;
                    case 3:
                        m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay($"{EntityName} wakes up!"));
                        target.SetStatus(target.CurrentStatus & ~EEntityStatus.Asleep);
                        break;
                }
            }

            if (CurrentStats.currentMana < ability.manaCost)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                    new TextToDisplay($"{EntityName} has not enough mana to cast {ability.abilityName}!"));
                return;
            }

            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                new TextToDisplay($"{EntityName} cast {ability.abilityName} on {target.EntityName}!"));

            if (ability.manaCost > 0)
            {
                UseMana(ability.manaCost);
            }
            
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
                                    $"{EntityName} makes {target.EntityName} {abilityEffect.targetedStatus.ToString()}!"));
                            break;
                        case EAbilityEffect.RemoveStatus:
                            target.SetStatus(target.CurrentStatus & ~abilityEffect.targetedStatus);
                            m_gm.uiManager.TextArea.AddTextToDisplayQueue(
                                new TextToDisplay(
                                    $"{target.EntityName} is no longer {abilityEffect.targetedStatus.ToString()}!"));
                            break;
                    }
            }
        }

        public virtual void UpdateStat(EEntityStats stat, int modifierValue, Action feedback = null)
        {
            switch (stat)
            {
                case EEntityStats.MaxHealth:
                    SetMaxHp(CurrentStats.maxHp + modifierValue, feedback);
                    break;
                case EEntityStats.Health:
                    if (modifierValue >= 0) Heal(modifierValue, feedback);
                    else ApplyDamage(-modifierValue, feedback); // -value cuz ApplyDamage() takes positive inputs
                    break;
                case EEntityStats.HealthRegen:
                    SetHpRegen(CurrentStats.hpRegen + modifierValue, feedback);
                    break;
                case EEntityStats.MaxMana:
                    SetMaxMana(CurrentStats.maxMana + modifierValue, feedback);
                    break;
                case EEntityStats.Mana:
                    if (modifierValue >= 0) RecoverMana(modifierValue, feedback);
                    else UseMana(-modifierValue); // -value cuz UseMana() takes positive inputs
                    break;
                case EEntityStats.ManaRegen:
                    SetManaRegen(CurrentStats.manaRegen + modifierValue, feedback);
                    break;
                case EEntityStats.Armor:
                    SetArmor(CurrentStats.armor + modifierValue, feedback);
                    break;
                case EEntityStats.Accuracy:
                    SetAccuracy(CurrentStats.accuracy + modifierValue, feedback);
                    break;
                case EEntityStats.Agility:
                    SetAgility(CurrentStats.agility + modifierValue, feedback);
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
    }
}