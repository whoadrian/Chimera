using System;
using Chimera.Pooling;
using UnityEngine;

namespace Chimera.Combat
{
    public class DamageArea : MonoBehaviour, IPoolable
    {
        public float lifetime = 0.2f;
        
        private float _damage = 10;
        private Faction _faction;
        private float _spawnTime = 0;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            var victim = other.GetComponent<ICombatant>();
            if (victim == null || victim.Faction == _faction)
            {
                return;
            }

            victim.DealDamage(_damage);
        }

        private void Update()
        {
            if (Time.time - _spawnTime > lifetime)
            {
                gameObject.SetActive(false);
            }
        }

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