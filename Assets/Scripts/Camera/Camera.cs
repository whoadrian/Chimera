using System;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Camera movement logic
    /// </summary>
    public class Camera : MonoBehaviour
    {
        public GameConfig gameConfig;
        public CameraConfig cameraConfig;
        public Transform viewTransform;
        public UnityEngine.Camera viewCamera;
        public CameraSpawnpoint[] spawnpoints;
        
        // Transform data to interpolate to
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _targetZoomSize;

        // Level boundaries
        private Vector2 _minBoundaryPos = new Vector2(float.MinValue, float.MinValue);
        private Vector2 _maxBoundaryPos = new Vector2(float.MaxValue, float.MaxValue);

        // First right click position press
        private Vector3 _rightClickPressPosition;

        private void Start()
        {
            // Set initial position, if any spawnpoints have been specified
            foreach (var s in spawnpoints)
            {
                if (s.faction != gameConfig.playerFaction)
                {
                    continue;
                }
                
                transform.position = s.transform.position;
                transform.rotation = s.transform.rotation;
            }
            
            // Init transform data
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
            _targetZoomSize = Mathf.Lerp(cameraConfig.minZoomSize, cameraConfig.maxZoomSize, 0.5f);
            
            // Look at target
            viewTransform.LookAt(transform.position);
            
            // Set level boundaries, if any level instance is present
            if (Level.Instance?.minLevelBoundary && Level.Instance?.maxLevelBoundary)
            {
                // Min
                var minPos = Vector3.Min(Level.Instance.minLevelBoundary.position,
                    Level.Instance.maxLevelBoundary.position);
                _minBoundaryPos = new Vector2(minPos.x, minPos.z);
                
                // Max
                var maxPos = Vector3.Max(Level.Instance.minLevelBoundary.position,
                    Level.Instance.maxLevelBoundary.position);
                _maxBoundaryPos = new Vector2(maxPos.x, maxPos.z);
            }
        }

        private void Update()
        {
            #region Rotation

            var inputRotationDelta = 0.0f;

            // Q/E rotation
            if (Input.GetKey(KeyCode.Q))
            {
                inputRotationDelta = 1;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                inputRotationDelta = -1;
            }

            // Right-click + move, rotation
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
            
            // Set target rotation
            _targetRotation *= Quaternion.Euler(Vector3.up * cameraConfig.rotationSpeed * inputRotationDelta * Time.deltaTime);

            // Interpolate towards target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation,
                cameraConfig.rotationSmoothing * Time.deltaTime);
            
            #endregion
            
            #region Movement

            var inputMoveDelta = new Vector2();
            
            // A/D, Left / Right
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDelta.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputMoveDelta.x = 1;
            }

            // W/S, Up / Down
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDelta.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {   
                inputMoveDelta.y = -1;
            }
            
            // Set target position
            _targetPosition += transform.right * (inputMoveDelta.x * cameraConfig.moveSpeed * Time.deltaTime);
            _targetPosition += transform.forward * (inputMoveDelta.y * cameraConfig.moveSpeed * Time.deltaTime);

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _minBoundaryPos.x, _maxBoundaryPos.x);
            _targetPosition.z = Mathf.Clamp(_targetPosition.z, _minBoundaryPos.y, _maxBoundaryPos.y);

            // Interpolate towards target position
            transform.position =
                Vector3.Lerp(transform.position, _targetPosition, cameraConfig.moveSmoothing * Time.deltaTime);
            
            #endregion

            #region Zoom

            // Mouse scroll zoom
            var zoomInputDelta = -1 * Input.mouseScrollDelta.y * cameraConfig.zoomScrollSensitivity;
            
            // R/F zoom
            if (Input.GetKey(KeyCode.R))
            {
                zoomInputDelta = -1f;
            }
            else if (Input.GetKey(KeyCode.F))
            {
                zoomInputDelta = 1f;
            }
            
            // Set target zoom ortho size
            _targetZoomSize += cameraConfig.zoomSpeed * zoomInputDelta * Time.deltaTime;
            _targetZoomSize = Math.Clamp(_targetZoomSize, cameraConfig.minZoomSize, cameraConfig.maxZoomSize);

            // Interpolate ortho camera size towards target zoom
            viewCamera.orthographicSize =
                Mathf.Lerp(viewCamera.orthographicSize, _targetZoomSize, cameraConfig.zoomSmoothing * Time.deltaTime);

            #endregion
        }
    }
}