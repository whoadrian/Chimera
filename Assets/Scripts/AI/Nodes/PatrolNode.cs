using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class PatrolNode : Node
    {
        private PatrolWaypoints _waypointsBehaviour;
        private int _currentWaypointIndex = 0;

        PatrolNode()
        {
            _waypointsBehaviour = _tree.GetComponent<PatrolWaypoints>();
            if (_waypointsBehaviour == null || _waypointsBehaviour.waypoints.Length == 0)
            {
                Debug.LogError("PatrolNode requires a PatrolWaypoints component with at least 1 waypoint!");
            }
        }
        
        public override State Evaluate()
        {
            

            _state = State.Running;
            return _state;
        }
    }
}