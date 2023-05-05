using System;
using UnityEngine;

namespace Chimera
{
    public class MeleeWeapon : MonoBehaviour
    {
        public Collider damageCollider;

        private float _damage;

        private void Awake()
        {
            damageCollider.isTrigger = true;
            DeactivateWeapon();
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        public void ActivateWeapon()
        {
            damageCollider.enabled = true;
        }

        public void DeactivateWeapon()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            damageable?.OnDamageReceived(_damage);
        }
    }
}