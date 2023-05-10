using System;
using UnityEngine;

namespace Chimera
{
    public class Camera : MonoBehaviour
    {
        public GameConfig gameConfig;
        public CameraConfig cameraConfig;
        public Transform viewTransform;
        public UnityEngine.Camera viewCamera;
        public CameraSpawnpoint[] spawnpoints;
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _targetZoomSize;

        private Vector2 _minBoundaryPos = new Vector2(float.MinValue, float.MinValue);
        private Vector2 _maxBoundaryPos = new Vector2(float.MaxValue, float.MaxValue);

        private Vector3 _rightClickPressPosition;
        
        private void Start()
        {
            foreach (var s in spawnpoints)
            {
                if (s.faction != gameConfig.playerFaction)
                {
                    continue;
                }
                
                transform.position = s.transform.position;
                transform.rotation = s.transform.rotation;
            }
            
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
            _targetZoomSize = Mathf.Lerp(cameraConfig.minZoomSize, cameraConfig.maxZoomSize, 0.5f);
            
            viewTransform.LookAt(transform.position);
            
            if (Level.Instance?.minLevelBoundary && Level.Instance?.maxLevelBoundary)
            {
                var minPos = Vector3.Min(Level.Instance.minLevelBoundary.position,
                    Level.Instance.maxLevelBoundary.position);
                _minBoundaryPos = new Vector2(minPos.x, minPos.z);
                
                var maxPos = Vector3.Max(Level.Instance.minLevelBoundary.position,
                    Level.Instance.maxLevelBoundary.position);
                _maxBoundaryPos = new Vector2(maxPos.x, maxPos.z);
            }
        }

        private void Update()
        {
            #region Rotation

            var inputRotationDelta = 0.0f;

            if (Input.GetKey(KeyCode.Q))
            {
                inputRotationDelta = 1;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                inputRotationDelta = -1;
            }

            if (Input.GetMouseButtonDown(1))
            {
                _rightClickPressPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                inputRotationDelta = _rightClickPressPosition.x - Input.mousePosition.x;
                inputRotationDelta *= cameraConfig.rotationMouseSensitivity * (cameraConfig.rotationMouseInvert ? -1 : 1);
                _rightClickPressPosition = Input.mousePosition;
            }
            
            _targetRotation *= Quaternion.Euler(Vector3.up * cameraConfig.rotationSpeed * inputRotationDelta * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation,
                cameraConfig.rotationSmoothing * Time.deltaTime);
            
            #endregion
            
            #region Movement

            var inputMoveDelta = new Vector2();
            
            // Left / Right
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDelta.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputMoveDelta.x = 1;
            }

            // Up / Down
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDelta.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {   
                inputMoveDelta.y = -1;
            }
            
            _targetPosition += transform.right * (inputMoveDelta.x * cameraConfig.moveSpeed * Time.deltaTime);
            _targetPosition += transform.forward * (inputMoveDelta.y * cameraConfig.moveSpeed * Time.deltaTime);

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _minBoundaryPos.x, _maxBoundaryPos.x);
            _targetPosition.z = Mathf.Clamp(_targetPosition.z, _minBoundaryPos.y, _maxBoundaryPos.y);

            transform.position =
                Vector3.Lerp(transform.position, _targetPosition, cameraConfig.moveSmoothing * Time.deltaTime);
            
            #endregion

            #region Zoom

            var zoomInputDelta = -1 * Input.mouseScrollDelta.y * cameraConfig.zoomScrollSensitivity;
            
            if (Input.GetKey(KeyCode.R))
            {
                zoomInputDelta = -1f;
            }
            else if (Input.GetKey(KeyCode.F))
            {
                zoomInputDelta = 1f;
            }
            
            _targetZoomSize += cameraConfig.zoomSpeed * zoomInputDelta * Time.deltaTime;
            _targetZoomSize = Math.Clamp(_targetZoomSize, cameraConfig.minZoomSize, cameraConfig.maxZoomSize);

            viewCamera.orthographicSize =
                Mathf.Lerp(viewCamera.orthographicSize, _targetZoomSize, cameraConfig.zoomSmoothing * Time.deltaTime);

            #endregion
        }
    }
}