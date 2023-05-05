using System;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera
{
    public class Character : MonoBehaviour
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
        }

        private void Update()
        {
            animator.SetBool(config.walkAnimBool, Vector3.SqrMagnitude(transform.position - _lastPos) > 0f);
            _lastPos = transform.position;
        }
    }
}