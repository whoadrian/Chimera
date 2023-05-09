using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera.AI.Utilities
{
    public class PatrolWaypoints : MonoBehaviour
    {
        public Transform waypointsParent;

        [HideInInspector] public List<Transform> waypoints;

        private void Awake()
        {
            for (int i = 0; i < waypointsParent.childCount; ++i)
            {
                waypoints.Add(waypointsParent.GetChild(i));
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (waypointsParent == null || waypointsParent.childCount == 0)
            {
                return;
            }

            Handles.color = Color.blue;
            for (int i = 1; i < waypointsParent.childCount; ++i)
            {
                Handles.DrawLine(waypointsParent.GetChild(i - 1).position, waypointsParent.GetChild(i).position);
            }

            if (waypointsParent.childCount > 1)
            {
                Handles.DrawLine(waypointsParent.GetChild(waypointsParent.childCount - 1).position, waypointsParent.GetChild(0).position);
            }
        }

#endif
    }
}