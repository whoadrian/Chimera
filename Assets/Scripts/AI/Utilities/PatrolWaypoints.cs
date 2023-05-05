using System.Collections.Generic;
using UnityEngine;

namespace Chimera.AI.Utilities
{
    public class PatrolWaypoints : MonoBehaviour
    {
        public Transform waypointsParent;
        
        [HideInInspector]
        public List<Transform> waypoints;

        private void Awake()
        {
            for (int i = 0; i < waypointsParent.childCount; ++i)
            {
                waypoints.Add(waypointsParent.GetChild(i));
            }
        }
    }
}