using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Characters/CharacterConfig", fileName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Speed")] 
        public float walkSpeed = 5;
        public float angularSpeed = 180;
        
        [Header("Ranges")]
        public float fovRange = 10;
        public float attackRange = 3;

        [Header("Patrol")] 
        public float patrolWaitTime = 1.0f;

        [Header("Animation")] 
        public string walkAnimBool = "Walk";
        public string attackAnimBool = "Attack";
    }
}