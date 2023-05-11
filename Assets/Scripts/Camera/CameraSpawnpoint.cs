using System;
using Chimera.Combat;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Spawnpoint for the camera, per faction.
    /// </summary>
    public class CameraSpawnpoint : MonoBehaviour
    {
        public Faction faction;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = faction switch
            {
                Faction.Blue => Color.blue,
                Faction.Green => Color.green,
                Faction.Red => Color.red,
                _ => throw new NotImplementedException()
            };
            
            Gizmos.DrawSphere(transform.position, 5);
        }
#endif
    }
}