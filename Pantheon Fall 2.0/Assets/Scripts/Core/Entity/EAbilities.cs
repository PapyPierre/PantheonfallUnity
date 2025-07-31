using System;
using NaughtyAttributes;

namespace Core.Entity
{
    [Serializable]
    public enum EAbilities
    {
        Strike,
    }
    
    [Serializable]
    public class AbilityEffect
    {
        public EAbilityEffect effect;
        
        [ShowIf("ModifyStat"), AllowNesting] public EEntityStats targetedStat;
        [ShowIf("ModifyStat"), AllowNesting] public uint value;
        
        [ShowIf("ModifyStatus"), AllowNesting] public EEntityStatus targetedStatus;

        private bool ModifyStat() => effect ==  EAbilityEffect.AddStat || effect == EAbilityEffect.RemoveStat;
        private bool ModifyStatus() => effect == EAbilityEffect.AddStatus || effect == EAbilityEffect.RemoveStatus;
    }

    [Serializable]
    public enum EAbilityEffect
    {
        None,
        AddStat,
        RemoveStat,
        AddStatus,
        RemoveStatus,
    }
    
    [Serializable]
    public enum EAbilityTarget
    {
        None,
        Self,
        Allies,
        Enemies,
        Everyone,
    }
}