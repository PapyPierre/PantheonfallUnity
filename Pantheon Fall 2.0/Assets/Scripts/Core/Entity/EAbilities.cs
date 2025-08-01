using System;
using NaughtyAttributes;

namespace Core.Entity
{
    [Serializable]
    public enum EAbilities
    {
        None,
        StandBy,
        Strike,
    }
    
    [Serializable]
    public class AbilityEffect
    {
        public EAbilityEffect effect;
        
        [ShowIf("ModifyStat"), AllowNesting] public EEntityStats targetedStat;
        [ShowIf("ModifyStat"), AllowNesting] public int value;
        
        [ShowIf("ModifyStatus"), AllowNesting] public EEntityStatus targetedStatus;

        public bool ModifyStat() => effect ==  EAbilityEffect.ModifyStat;
        public bool ModifyStatus() => effect == EAbilityEffect.AddStatus || effect == EAbilityEffect.RemoveStatus;
    }

    [Serializable]
    public enum EAbilityEffect
    {
        None,
        ModifyStat,
        AddStatus,
        RemoveStatus,
    }
    
    [Serializable]
    public enum EAbilityTarget
    {
        Self,
        Opponent,
        Everyone,
    }
}