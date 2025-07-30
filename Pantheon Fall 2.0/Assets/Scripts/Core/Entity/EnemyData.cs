using UnityEngine;

namespace Core.Entity
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/EntityData/Enemy", order = 1)] 
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public string shortName;
        [field: SerializeField] public string fullName;
        [field: SerializeField] public Sprite sprite;
        [field: SerializeField] public int tier;
        
        [field: SerializeField] public EntityStats BaseStats { get; private set; }
    }
}