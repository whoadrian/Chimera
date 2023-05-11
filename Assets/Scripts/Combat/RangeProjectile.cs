using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    /// <summary>
    /// Pooled component to be attached to a projectile prefab, to be spawned by a RangedWeapon.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RangeProjectile : MonoBehaviour, IPoolable
    {
        // Pooled prefab to spawn when this projectile hits something
        public GameObject damageAreaPrefab;
        
        // Pooled fx prefab to be spawned when this projectile gets spawned
        public GameObject spawnParticlesPrefab;
        
        // Pooled fx prefab to be spawned when this projectile hits something
        public GameObject hitParticlesPrefab;
        
        // Offset position for the hitParticlesPrefab above
        public Vector3 hitParticlesOffset;
        
        // Useful members
        private float _damage;
        private Faction _faction;
        private float _speed;
        private float _maxDistanceSqr;
        private Vector3 _spawnPosition;
        private Rigidbody _rigidbody;

        #region MonoBehaviour
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            // Check max distance, trigger hit when reached
            if (Vector3.SqrMagnitude(transform.position - _spawnPosition) >= _maxDistanceSqr)
            {
                OnHit();
                return;
            }
            
            // Move rigidbody
            _rigidbody.MovePosition(transform.position + transform.forward * (_speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            // If we've hit something, trigger hit
            OnHit(other.GetComponent<ICombatant>());
        }
        
        #endregion
        
        private void OnHit(ICombatant victim = null)
        {
            // Deactivate pooled object, to be used by others
            gameObject.SetActive(false);

            // If we have a pooled damage area prefab, spawn it
            if (damageAreaPrefab != null)
            {
                var dmgAreaPoolable = ObjectPool.Instance.GetObject(damageAreaPrefab);
                if (dmgAreaPoolable != null)
                {
                    var dmgArea = (DamageArea)dmgAreaPoolable;
                    if (dmgArea)
                    {
                        dmgArea.Setup(_damage, _faction);
                    }
                    dmgAreaPoolable.Activate(transform.position, transform.rotation);
                }
            }

            // If we have a pooled particle fx prefab on hit, spawn it
            if (hitParticlesPrefab != null)
            {
                var particlesPoolable = ObjectPool.Instance.GetObject(hitParticlesPrefab);
                if (particlesPoolable != null)
                {
                    particlesPoolable.Activate(transform.position + hitParticlesOffset, transform.rotation);
                }
            }
        }

        /// <summary>
        /// Initializes the projectile.
        /// </summary>
        public void Setup(float damage, Faction faction, float speed, float maxDistance)
        {
            _damage = damage;
            _faction = faction;
            _speed = speed;
            _maxDistanceSqr = maxDistance * maxDistance;
        }

        #region IPoolable

        public bool IsActive()
        {
            // This projectile is active when its gameobject is active
            return gameObject.activeSelf;
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            // Setup position
            transform.position = position;
            transform.rotation = rotation;

            _spawnPosition = position;
            gameObject.SetActive(true);
            
            // If we have a pooled fx prefab, spawn it
            if (spawnParticlesPrefab != null)
            {
                var particlesPoolable = ObjectPool.Instance.GetObject(spawnParticlesPrefab);
                if (particlesPoolable != null)
                {
                    particlesPoolable.Activate(position, rotation);
                }
            }
        }
        
        #endregion
    }
}