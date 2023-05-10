using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class PatrolNode : Node
    {
        private PatrolWaypoints _waypointsBehaviour;

        private int _currentWaypointIndex = 0;
        private bool _waiting = true;
        private float _waitCounter = 0.0f;
        
        public PatrolNode(BehaviourTree tree) : base(tree)
        {
            _waypointsBehaviour = _tree.GetComponent<PatrolWaypoints>();
        }
        
        public override State Evaluate()
        {
            if (_waypointsBehaviour == null || _waypointsBehaviour.waypoints == null || _waypointsBehaviour.waypoints.Count == 0)
            {
                _state = State.Success;
                return _state;
            }
            
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _tree.actor.config.patrolWaitTime)
                {
                    _waiting = false;
                    _waitCounter = 0.0f;
                    
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypointsBehaviour.waypoints.Count;
                    _tree.actor.navMeshAgent.SetDestination(_waypointsBehaviour.waypoints[_currentWaypointIndex].position);
                }
            }
            else
            {
                _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
                _tree.SetContext(Context.DestinationKey, _tree.actor.navMeshAgent.destination);
                
                if (_tree.actor.navMeshAgent.remainingDistance < 0.01f)
                {
                    _waiting = true;
                }
            }
            
            _state = State.Running;
            return _state;
        }
    }
}