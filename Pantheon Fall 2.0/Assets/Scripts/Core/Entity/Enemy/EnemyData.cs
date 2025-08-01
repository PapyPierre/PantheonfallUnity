using System.Collections.Generic;
using UnityEngine;

namespace Core.Entity
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/EntityData/Enemy", order = 1)] 
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public EEnemies enemy;
        [field: SerializeField] public string fullName;
        [field: SerializeField] public Sprite sprite;
        
        [field: SerializeField] public TextToDisplay AppearingText {get; private set;}
        
        [field: SerializeField] public EntityStats BaseStats { get; private set; }

        [field: SerializeField] public List<EAbilities> KnownAbilities { get; private set; } = new List<EAbilities>();
    }
}