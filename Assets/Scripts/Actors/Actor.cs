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
    public class Actor : MonoBehaviour, ICombatant, ISelectable
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;

        public Faction faction;
        public ActorConfig config;

        private int _id;
        private float _health;

        private void Start()
        {
            _health = config.maxHealth;

            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = config.walkSpeed;
                navMeshAgent.angularSpeed = config.angularSpeed;
            }

            animator = GetComponent<Animator>();

            GetComponent<MeleeWeapon>()?.Setup(this);
            GetComponent<RangeWeapon>()?.Setup(this);

            _id = Level.Instance.RegisterActor(this);
        }

        private void OnDestroy()
        {
            Level.Instance.UnregisterActor(_id);
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

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = faction switch
            {
                Faction.Blue => Color.blue,
                Faction.Green => Color.green,
                Faction.Red => Color.red,
                _ => throw new NotImplementedException()
            };

            Gizmos.DrawSphere(transform.position + Vector3.up * 3, 0.4f);
            
            if (config != null)
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.up, config.attackRange);
                Handles.DrawLine(transform.position, transform.position - transform.forward * config.attackRange);

                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, Vector3.up, config.fovRange);
                Handles.DrawLine(transform.position, transform.position + transform.forward * config.fovRange);
            }
        }

#endif
    }
}