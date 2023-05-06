using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Camera/Camera Config", fileName = "CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 10;
        public float moveSmoothing = 10;

        [Header("Rotation")] 
        public float cameraAngle = 45;
        public float rotationSpeed = 10;
        public float rotationSmoothing = 10;
        
        [Header("Zoom")]
        public float zoomSpeed = 10;
        public float minZoomSize = 10;
        public float maxZoomSize = 100;
        public float zoomSmoothing = 10;
    }
}