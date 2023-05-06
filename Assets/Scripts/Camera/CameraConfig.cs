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
        
        [Header("FoV")]
        public float fovSpeed = 10;
        public float minFov = 10;
        public float maxFov = 100;
        public float fovSmoothing = 10;
    }
}