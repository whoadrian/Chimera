using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    public class RangeWeapon : MonoBehaviour
    {
        public Transform[] projectileSpawnpoints;
        public GameObject projectilePrefab;

        private float _damage;
        private Faction _faction;
        private float _projectileSpeed;
        private float _projectileMaxDistance;
        
        public void Setup(float damage, Faction faction, float projectileSpeed, float projectileMaxDistance)
        {
            _damage = damage;
            _faction = faction;
            _projectileSpeed = projectileSpeed;
            _projectileMaxDistance = projectileMaxDistance;
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
            rangedProjectile.Setup(_damage, _faction, _projectileSpeed, _projectileMaxDistance);
            
            projectile.Activate(projectileSpawnpoints[spawnpointIndex].position,
                projectileSpawnpoints[spawnpointIndex].rotation);
        }
    }
}