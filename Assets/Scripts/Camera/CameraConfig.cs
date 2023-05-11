using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Camera movement settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Chimera/Camera/Camera Config", fileName = "CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 10;
        public float moveSmoothing = 10;

        [Header("Rotation")] 
        public float rotationSpeed = 10;
        public float rotationSmoothing = 10;
        public float rotationMouseSensitivity = 1;
        public bool rotationMouseInvert = false;
        
        [Header("Zoom")]
        public float zoomSpeed = 10;
        public float zoomSmoothing = 10;
        public float zoomScrollSensitivity = 1;
        public float minZoomSize = 10;
        public float maxZoomSize = 100;
    }
}