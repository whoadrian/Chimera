using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    /// <summary>
    /// Pooled component that deals damage to any enemy ICombatant components.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DamageArea : MonoBehaviour, IPoolable
    {
        // How long does this object lives?
        public float lifetime = 0.2f;

        private float _spawnTime = 0;

        // Combat data
        private float _damage = 10;
        private Faction _faction;

        #region MonoBehaviour

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if other combatant is detected
            var victim = other.GetComponent<ICombatant>();
            if (victim == null || victim.Faction == _faction)
            {
                return;
            }

            // Deal damage
            victim.DealDamage(_damage);
        }

        private void Update()
        {
            // Lifetime counter
            if (Time.time - _spawnTime > lifetime)
            {
                gameObject.SetActive(false);
            }
        }
        
        #endregion

        /// <summary>
        /// Initializes damage area.
        /// </summary>
        public void Setup(float damage, Faction faction)
        {
            _damage = damage;
            _faction = faction;
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

            _spawnTime = Time.time;
            gameObject.SetActive(true);
        }

        #endregion
    }
}