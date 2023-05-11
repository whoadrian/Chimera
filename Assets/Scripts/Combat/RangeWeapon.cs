using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    /// <summary>
    /// Component for any actor that has a melee combat behaviour.
    /// </summary>
    [RequireComponent(typeof(Actor))]
    public class RangeWeapon : MonoBehaviour
    {
        // Transform data for spawning projectiles
        public Transform[] projectileSpawnpoints;
        
        // Pooled projectile prefab, needs IPoolable interface
        public GameObject projectilePrefab;

        // Owner actor
        private Actor _owner;
        
        public void Setup(Actor owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Animation event callback. Spawns a projectile at projectile spawnpointIndex. See RangeProjectile class.
        /// </summary>
        public void ActivateWeapon(int spawnpointIndex = 0)
        {
            // Check spawnpoint index range
            if (spawnpointIndex < 0 || spawnpointIndex >= projectileSpawnpoints.Length)
            {
                Debug.LogError("Ranged Weapon projectile spawnpoint index out of range");
                return;
            }

            // Spawn pooled projectile
            var projectile = ObjectPool.Instance.GetObject(projectilePrefab);
            var rangedProjectile = (RangeProjectile)projectile;
            rangedProjectile.Setup(_owner.config.damage, _owner.faction, _owner.config.projectileSpeed, _owner.config.attackRange);
            
            // Initialize projectile
            projectile.Activate(projectileSpawnpoints[spawnpointIndex].position,
                projectileSpawnpoints[spawnpointIndex].rotation);
        }
    }
}