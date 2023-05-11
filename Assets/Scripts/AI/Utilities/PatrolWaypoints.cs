using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera.AI.Utilities
{
    /// <summary>
    /// Component used by the PatrolNode inside a behaviour tree, for moving an actor between a list of positions.
    /// </summary>
    public class PatrolWaypoints : MonoBehaviour
    {
        public List<Transform> waypoints;

#if UNITY_EDITOR

        // Draw the waypoints paths in the editor
        private void OnDrawGizmos()
        {
            if (waypoints == null)
            {
                return;
            }
            
            Handles.color = Color.blue;
            for (int i = 1; i < waypoints.Count; ++i)
            {
                Handles.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }

            // Loop
            if (waypoints.Count > 1)
            {
                Handles.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }

#endif
    }
}