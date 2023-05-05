using Chimera.Pooling;
using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Characters/CharacterConfig", fileName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Combat")] 
        public float maxHealth = 100;
        public float damage = 10;
        public float fovRange = 10;
        public float attackRange = 3;
        public GameObject deathParticles;
        
        [Header("Speed")] 
        public float walkSpeed = 5;
        public float angularSpeed = 180;

        [Header("Patrol")] 
        public float patrolWaitTime = 1.0f;

        [Header("Animation")] 
        public string walkAnimBool = "Walk";
        public string attackAnimBool = "Attack";
    }
}