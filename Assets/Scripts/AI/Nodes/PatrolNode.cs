using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class PatrolNode : Node
    {
        private PatrolWaypoints _waypointsBehaviour;

        private int _currentWaypointIndex = 0;
        private bool _waiting = false;
        private float _waitCounter = 0.0f;
        
        public PatrolNode(BehaviourTree tree) : base(tree)
        {
            _waypointsBehaviour = _tree.GetComponent<PatrolWaypoints>();
            if (_waypointsBehaviour == null || _waypointsBehaviour.waypoints.Length == 0)
            {
                Debug.LogError("PatrolNode requires a PatrolWaypoints component with at least 1 waypoint!");
            }
        }
        
        public override State Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _tree.character.config.patrolWaitTime)
                {
                    _waiting = false;
                    _waitCounter = 0.0f;
                }
            }
            else
            {
                var targetWaypoint = _waypointsBehaviour.waypoints[_currentWaypointIndex];

                if (Vector3.SqrMagnitude(_tree.character.transform.position - targetWaypoint.position) < 0.01f)
                {
                    _tree.character.transform.position = targetWaypoint.position;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypointsBehaviour.waypoints.Length;
                }
                else
                {
                    _tree.character.transform.position = Vector3.MoveTowards(_tree.character.transform.position,
                        targetWaypoint.position, _tree.character.config.speed * Time.deltaTime);
                    _tree.character.transform.LookAt(targetWaypoint.position);
                }
            }
            
            _state = State.Running;
            return _state;
        }
    }
}