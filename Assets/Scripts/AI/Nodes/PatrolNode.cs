using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Moves the actor between a list of points. Actor needs to have the PatrolWaypoints component!
    /// </summary>
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
            // No waypoints
            if (_waypointsBehaviour == null || _waypointsBehaviour.waypoints == null || _waypointsBehaviour.waypoints.Count == 0)
            {
                _state = State.Success;
                return _state;
            }
            
            // Are we at a patrol point, pausing before goind to the next one?
            if (_waiting)
            {
                // Track pause time
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _tree.actor.config.patrolWaitTime)
                {
                    _waiting = false;
                    _waitCounter = 0.0f;

                    // Move to next patrol point
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypointsBehaviour.waypoints.Count;
                    _tree.actor.navMeshAgent.SetDestination(_waypointsBehaviour.waypoints[_currentWaypointIndex].position);
                }
            }
            else // We are walking towards patrol point
            {
                // Make sure we are walking toward our patrol point
                if (_tree.actor.navMeshAgent.destination !=
                    _waypointsBehaviour.waypoints[_currentWaypointIndex].position)
                {
                    _tree.actor.navMeshAgent.SetDestination(_waypointsBehaviour.waypoints[_currentWaypointIndex].position);
                }
                
                // Set walk animation
                _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
                
                // Set destination context
                _tree.SetNodesContext(Context.NodeKey.Destination, _tree.actor.navMeshAgent.destination);
                
                // Check if destination reached
                if (_tree.actor.navMeshAgent.remainingDistance < 0.1f)
                {
                    _waiting = true;
                }
            }
            
            // We are patrolling
            _state = State.Running;
            return _state;
        }
    }
}