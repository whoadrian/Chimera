using System;
using Chimera.Combat;
using Chimera.Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera
{
    public class Actor : MonoBehaviour, ICombatant
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;

        public Faction faction;
        public ActorConfig config;

        private Vector3 _lastPos;
        private float _health;

        private void Awake()
        {
            _lastPos = transform.position;

            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = config.walkSpeed;
                navMeshAgent.angularSpeed = config.angularSpeed;
            }

            animator = GetComponent<Animator>();

            _health = config.maxHealth;

            var meleeWeapon = GetComponent<MeleeWeapon>();
            meleeWeapon?.SetDamage(config.damage);

            var rangedWeapon = GetComponent<RangeWeapon>();
            rangedWeapon?.SetDamage(config.damage);
        }

        private void Update()
        {
            animator.SetBool(config.walkAnimBool, Vector3.SqrMagnitude(transform.position - _lastPos) > 0f);
            _lastPos = transform.position;
        }

        #region ICombatant

        public Faction Faction => faction;
        public float CurrentHealth => _health;
        public float MaxHealth => config.maxHealth;

        public void DealDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0)
            {
                if (config.deathParticles)
                {
                    var p = ObjectPool.Instance.GetObject(config.deathParticles);
                    p.Activate(transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }

        public void Heal(float amount)
        {
            _health = Math.Min(_health + amount, MaxHealth);
        }

        #endregion
    }
}