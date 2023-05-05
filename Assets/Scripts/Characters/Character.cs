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
        
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        public Faction faction;
        public CharacterConfig config;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = config.walkSpeed;
                navMeshAgent.angularSpeed = config.angularSpeed;
            }
        }
    }
}