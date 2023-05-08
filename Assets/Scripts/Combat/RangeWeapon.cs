using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    public class RangeWeapon : MonoBehaviour
    {
        public Transform[] projectileSpawnpoints;
        public GameObject projectilePrefab;

        private Actor _owner;
        
        public void Setup(Actor owner)
        {
            _owner = owner;
        }

        public void ActivateWeapon(int spawnpointIndex = 0)
        {
            if (spawnpointIndex < 0 || spawnpointIndex >= projectileSpawnpoints.Length)
            {
                Debug.LogError("Ranged Weapon projectile spawnpoint index out of range");
                return;
            }

            var projectile = ObjectPool.Instance.GetObject(projectilePrefab);
            var rangedProjectile = (RangeProjectile)projectile;
            rangedProjectile.Setup(_owner.config.damage, _owner.faction, _owner.config.projectileSpeed, _owner.config.attackRange);
            
            projectile.Activate(projectileSpawnpoints[spawnpointIndex].position,
                projectileSpawnpoints[spawnpointIndex].rotation);
        }
    }
}