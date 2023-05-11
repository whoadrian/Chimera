using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Settings for actors in the game, that belong to a faction and participate in combat and gameplay mechanics.
    /// </summary>
    [CreateAssetMenu(menuName = "Chimera/Actors/Actor Config", fileName = "ActorConfig")]
    public class ActorConfig : ScriptableObject
    {
        [Header("Combat")] 
        public float maxHealth = 100;
        public float damage = 10;
        public float fovRange = 10;
        public float attackRange = 3;
        public float projectileSpeed = 100;
        public GameObject deathParticles;
        [Tooltip("Should the level ignore this actor when tracking the win/lose state?")]
        public bool excludeFromWinLoseState = false;
        
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