using System.Collections.Generic;

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
                    GameManager.instance.uiManager.TextArea.DisplayAbilities();
                    break;
                case EPlayerState.UsingItem:
                    GameManager.instance.uiManager.TextArea.DisplayItems();
                    break;
                case EPlayerState.StandingBy:
                    GameManager.instance.fightManager.SetPlayerAbilityOfThisTurn(EAbilities.StandBy);
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
            if (m_currentState == EPlayerState.UsingAbility)
            {
                UseAbility(actionIndex);
            }
            else if (m_currentState == EPlayerState.UsingItem)
            {
                UseItem(actionIndex);
            }
            else
            {
                switch (actionIndex)
                {
                    case 0:
                        SetPlayerState(EPlayerState.UsingAbility);
                        break;
                    case 1:
                        //TODO SetPlayerState(EPlayerState.UsingItem);
                        break;
                    case 2:
                    default:
                        SetPlayerState(EPlayerState.StandingBy);
                        break;
                }
            }
        }

        private void UseAbility(int abilityIndex)
        {
            AbilityData abilityData = DataManager.GetData<AbilityData>(AvailableAbilities[abilityIndex].ToString());
            if (abilityData.manaCost > CurrentStats.currentMana)
            {
                GameManager.instance.uiManager.TextArea.AddTextToDisplayQueue(new TextToDisplay("Not enough Mana!"));
                return;
            }
            GameManager.instance.fightManager.SetPlayerAbilityOfThisTurn(AvailableAbilities[abilityIndex]);
        }

        private void UseItem(int itemIndex)
        {
            //TODO
        }
        
        public override void ApplyDamage(int damage)
        {
            base.ApplyDamage(damage);
            GameManager.instance.uiManager.PlayerStats.UpdatePlayerHp(CurrentStats);
        }
    }

    public enum EPlayerState
    {
        Idle,
        UsingAbility,
        UsingItem,
        StandingBy,
    }
}