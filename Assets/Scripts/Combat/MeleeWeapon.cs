using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    public class MeleeWeapon : MonoBehaviour
    {
        public Collider damageCollider;
        public GameObject hitFxPrefab;

        private Actor _owner;

        private void Awake()
        {
            damageCollider.isTrigger = true;
            DeactivateWeapon();
        }

        public void Setup(Actor owner)
        {
            _owner = owner;
        }

        public void ActivateWeapon()
        {
            damageCollider.enabled = true;

            if (hitFxPrefab != null)
            {
                var particlesPoolable = ObjectPool.Instance.GetObject(hitFxPrefab);
                if (particlesPoolable != null)
                {
                    particlesPoolable.Activate(damageCollider.transform.position, damageCollider.transform.rotation);
                }
            }
        }

        public void DeactivateWeapon()
        {
            damageCollider.enabled = false;
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
    }
}