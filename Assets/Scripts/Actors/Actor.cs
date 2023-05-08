using System;
using Chimera.Combat;
using Chimera.Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera
{
    public class Actor : MonoBehaviour, ICombatant, ISelectable
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;

        public Faction faction;
        public ActorConfig config;

        private float _health;

        private void Awake()
        {
            _health = config.maxHealth;
            
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = config.walkSpeed;
                navMeshAgent.angularSpeed = config.angularSpeed;
            }

            animator = GetComponent<Animator>();
            
            GetComponent<MeleeWeapon>()?.Setup(config.damage, faction);
            GetComponent<RangeWeapon>()?.Setup(config.damage, faction, config.projectileSpeed, config.attackRange);
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

        #region ISelectable

        public bool Selected { get; set; }

        #endregion
    }
}