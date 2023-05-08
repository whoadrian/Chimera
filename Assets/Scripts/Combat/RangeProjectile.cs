using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class RangeProjectile : MonoBehaviour, IPoolable
    {
        public GameObject damageAreaPrefab;
        public GameObject hitParticlesPrefab;
        
        private float _damage;
        private Faction _faction;
        private float _speed;
        private float _maxDistanceSqr;

        private Rigidbody _rigidbody;
        private Vector3 _spawnPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (Vector3.SqrMagnitude(transform.position - _spawnPosition) >= _maxDistanceSqr)
            {
                OnHit();
                return;
            }
            
            _rigidbody.MovePosition(transform.position + transform.forward * (_speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.GetComponent<ICombatant>());
        }
        
        private void OnHit(ICombatant victim = null)
        {
            gameObject.SetActive(false);

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

            if (hitParticlesPrefab != null)
            {
                var particlesPoolable = ObjectPool.Instance.GetObject(hitParticlesPrefab);
                if (particlesPoolable != null)
                {
                    particlesPoolable.Activate(transform.position, transform.rotation);
                }
            }
        }

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
            return gameObject.activeSelf;
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            _spawnPosition = position;
            gameObject.SetActive(true);
        }
        
        #endregion
    }
}