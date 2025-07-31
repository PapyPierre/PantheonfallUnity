using System.Collections.Generic;

namespace Core.Entity
{
    public class Player : Entity
    {
        private readonly List<EAbilities> m_availableAbilities = new List<EAbilities>();
        
        public Player(EntityStats stats) : base(stats) {}
        
        public void UnlockAbility(EAbilities ability)
        {
            m_availableAbilities.Add(ability);
        }

        public void UnregisterAction(EAbilities ability)
        {
            m_availableAbilities.Remove(ability);
        }

        public void DoAction(int actionIndex)
        {
           
        }

        public void ApplyEffectToTarget(AbilityEffect effect, List<Entity> targets)
        {
            
        }
    }
}