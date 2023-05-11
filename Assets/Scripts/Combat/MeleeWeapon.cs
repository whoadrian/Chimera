using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    /// <summary>
    /// Component for any actor that has a melee combat behaviour.
    /// </summary>
    [RequireComponent(typeof(Actor))]
    public class MeleeWeapon : MonoBehaviour
    {
        // Collider that deals damage. Gets activated on attack animation event callback
        public Collider damageCollider;
        
        // Fx to be spawned when attacking on animation event callback
        public GameObject hitFxPrefab;

        // Owner actor
        private Actor _owner;

        #region MonoBehaviour
        
        private void Awake()
        {
            damageCollider.isTrigger = true;
            DeactivateWeapon();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var victim = other.GetComponent<ICombatant>();
            if (victim == null || victim.Faction == _owner.faction)
            {
                return;
            }
            
            victim.DealDamage(_owner.config.damage);
        }

        #endregion

        /// <summary>
        /// Initializes the weapon
        /// </summary>
        public void Setup(Actor owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Animation event callback
        /// </summary>
        public void ActivateWeapon()
        {
            damageCollider.enabled = true;

            // Spawn pooled hit fx, if any
            if (hitFxPrefab != null)
            {
                var particlesPoolable = ObjectPool.Instance.GetObject(hitFxPrefab);
                if (particlesPoolable != null)
                {
                    particlesPoolable.Activate(damageCollider.transform.position, damageCollider.transform.rotation);
                }
            }
        }

        /// <summary>
        /// Animation event callback
        /// </summary>
        public void DeactivateWeapon()
        {
            damageCollider.enabled = false;
        }
    }
}