using Chimera.Pooling;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace Chimera
{
    public class Character : MonoBehaviour, IDamageable
    {
        public enum Faction
        {
            Red,
            Green,
            Blue
        }
        
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;
        
        public Faction faction;
        public CharacterConfig config;

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

        public void OnDamageReceived(float damageAmount)
        {
            _health -= damageAmount;
            
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
    }
}