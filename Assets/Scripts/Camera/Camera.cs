using System;
using UnityEngine;

namespace Chimera
{
    public class Camera : MonoBehaviour
    {
        public CameraConfig config;
        public Transform viewTransform;
        public UnityEngine.Camera viewCamera;
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _targetZoomSize;

        private void Start()
        {
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
            _targetZoomSize = Mathf.Lerp(config.minZoomSize, config.maxZoomSize, 0.5f);
            
            viewTransform.LookAt(transform.position);
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
            
            _targetRotation *= Quaternion.Euler(Vector3.up * config.rotationSpeed * inputRotationDelta * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation,
                config.rotationSmoothing * Time.deltaTime);
            
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
            
            _targetPosition += transform.right * (inputMoveDelta.x * config.moveSpeed * Time.deltaTime);
            _targetPosition += transform.forward * (inputMoveDelta.y * config.moveSpeed * Time.deltaTime);

            transform.position =
                Vector3.Lerp(transform.position, _targetPosition, config.moveSmoothing * Time.deltaTime);
            
            #endregion

            #region Zoom

            var zoomInputDelta = -1 * Input.mouseScrollDelta.y;
            _targetZoomSize += config.zoomSpeed * zoomInputDelta * Time.deltaTime;
            _targetZoomSize = Math.Clamp(_targetZoomSize, config.minZoomSize, config.maxZoomSize);

            viewCamera.orthographicSize =
                Mathf.Lerp(viewCamera.orthographicSize, _targetZoomSize, config.zoomSmoothing * Time.deltaTime);

            #endregion
        }
    }
}