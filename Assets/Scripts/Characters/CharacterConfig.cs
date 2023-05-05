using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Characters/CharacterConfig", fileName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("General")] 
        public Character.Faction faction;
        public float speed = 10;
        public float fovRange = 10;
        public float attackRange = 3;

        [Header("Patrol")] 
        public float patrolWaitTime = 1.0f;
    }
}