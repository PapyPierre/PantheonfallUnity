using System;
using System.Collections.Generic;
using Core.Entity.Ability;
using Core.UI;

namespace Core.Entity
{
    public class Player : Entity
    {
        public readonly List<EAbilities> AvailableAbilities = new List<EAbilities>();
        private EPlayerState m_currentState = EPlayerState.Idle;
        
        public Player(EntityStats stats, string entityName) : base(stats, entityName) {}

        public void SetPlayerState(EPlayerState state)
        {
            m_currentState = state;

            switch (m_currentState)
            {
                case EPlayerState.Idle:
                    break;
                case EPlayerState.UsingAbility:
                    m_gm.uiManager.TextArea.DisplayAbilities();
                    break;
                case EPlayerState.UsingItem:
                    m_gm.uiManager.TextArea.DisplayItems();
                    break;
            }
        }
        
        public void UnlockAbility(EAbilities ability)
        {
            AvailableAbilities.Add(ability);
        }

        public void UnregisterAction(EAbilities ability)
        {
            AvailableAbilities.Remove(ability);
        }

        public void DoAction(int actionIndex)
        {
            switch (m_currentState)
            {
                case EPlayerState.UsingAbility: UseAbility(actionIndex);
                    break;
                case EPlayerState.UsingItem: UseItem(actionIndex);
                    break;
                default:
                    switch (actionIndex)
                    {
                        case 0:
                            SetPlayerState(EPlayerState.UsingAbility);
                            break;
                        case 1:
                            SetPlayerState(EPlayerState.UsingItem);
                            break;
                    }

                    break;
            }
        }

        private void UseAbility(int abilityIndex)
        {
            AbilityData abilityData = DataManager.GetData<AbilityData>(AvailableAbilities[abilityIndex].ToString());
            if (abilityData.manaCost > CurrentStats.currentMana)
            {
                m_gm.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay("Not enough Mana!"));
                return;
            }
            m_gm.fightManager.SetPlayerAbilityOfThisTurn(AvailableAbilities[abilityIndex]);

            SetPlayerState(EPlayerState.Idle);
        }

        private void UseItem(int itemIndex)
        {
            //TODO
        }
        
        public override void ApplyDamage(int damage, Action feedback)
        {
            feedback += m_gm.uiManager.PlayerStats.UpdatePlayerHp;
            base.ApplyDamage(damage, feedback);
        }
    }

    public enum EPlayerState
    {
        Idle,
        UsingAbility,
        UsingItem,
    }
}