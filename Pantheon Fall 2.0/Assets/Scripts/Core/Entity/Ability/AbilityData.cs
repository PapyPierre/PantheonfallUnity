using System.Collections.Generic;
using UnityEngine;

namespace Core.Entity.Ability
{
    [CreateAssetMenu(fileName = "Ability Data", menuName = "ScriptableObjects/Ability Data", order = 1)] 
    public class AbilityData : ScriptableObject
    {
        public EAbilities ability;
        public string abilityName;
        public int speed;
        public EAbilityTarget targets;
        public int manaCost;
        public List<AbilityEffect> effects;
    }
}