using System;
using Chimera.Combat;
using Chimera.Pooling;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    /// <summary>
    /// An actor is an entity that belongs to a faction, and participates in combat and gameplay mechanics.
    /// </summary>
    public class Actor : MonoBehaviour, ICombatant, ISelectable
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;

        public Faction faction;
        public ActorConfig config;

        private int _id = Level.InvalidActorId;
        private float _health;

        #region MonoBehaviour

        private void Start()
        {
            // Setup current health
            _health = config.maxHealth;

            // Setup nav mesh agent settings
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = config.walkSpeed;
                navMeshAgent.angularSpeed = config.angularSpeed;
            }

            animator = GetComponent<Animator>();

            // Setup weapons
            GetComponent<MeleeWeapon>()?.Setup(this);
            GetComponent<RangeWeapon>()?.Setup(this);

            // Register this actor to the level, for win/lose states
            if (Level.Instance)
            {
                _id = Level.Instance.RegisterActor(this);
            }
        }

        private void OnDestroy()
        {
            // Unregister this actor from the level, for win/lose states
            if (_id != Level.InvalidActorId && Level.Instance != null)
            {
                Level.Instance.UnregisterActor(_id);
            }
        }
        
        #endregion

        #region ICombatant

        public Faction Faction => faction;
        public float CurrentHealth => _health;
        public float MaxHealth => config.maxHealth;

        public void DealDamage(float amount)
        {
            // Subtract from current health
            _health -= amount;

            // Check if dead
            if (_health <= 0)
            {
                // Death particles spawn
                if (config.deathParticles)
                {
                    // Object pooling usage
                    var p = ObjectPool.Instance.GetObject(config.deathParticles);
                    p.Activate(transform.position, transform.rotation);
                }

                // Destroy actor, trigger OnDestroy
                Destroy(gameObject);
            }
        }

        public void Heal(float amount)
        {
            // Add to health
            _health = Math.Min(_health + amount, MaxHealth);
        }

        #endregion

        #region ISelectable

        public bool Selected { get; set; }

        #endregion

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = faction switch
            {
                Faction.Blue => Color.blue,
                Faction.Green => Color.green,
                Faction.Red => Color.red,
                _ => throw new NotImplementedException()
            };

            // Draw faction sphere color
            Gizmos.DrawSphere(transform.position + Vector3.up * 3, 0.4f);
            
            // Draw config ranges
            if (config != null)
            {
                // Attack
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.up, config.attackRange);
                Handles.DrawLine(transform.position, transform.position - transform.forward * config.attackRange);

                // Field of view
                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, Vector3.up, config.fovRange);
                Handles.DrawLine(transform.position, transform.position + transform.forward * config.fovRange);
            }
        }

#endif
    }
}