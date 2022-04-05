using System;
using Camera;
using UnityEngine;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        public float moveAmount;
        public float horizontal;
        public float vertical;
        
        private PlayerControls _inputActions;
        private CameraController _cameraController;
        
        private Vector2 _movementInput;
        private Vector2 _cameraInput;
        
        private float _mouseX;
        private float _mouseY;

        private void Awake()
        {
            _cameraController = CameraController.cc;
        }

        private void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;

            if (_cameraController == null) return;
            
            _cameraController.FollowTarget(delta);
            _cameraController.HandleCameraRotation(delta, _mouseX, _mouseY);
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                
                _inputActions.PlayerMovement.Body.performed +=
                    inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                
                _inputActions.PlayerMovement.Camera.performed +=
                    inputActions => _cameraInput = inputActions.ReadValue<Vector2>();
            }
            
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            BodyMove(delta);
            CameraMove();
        }

        private void BodyMove(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }

        private void CameraMove()
        {
            _mouseX = _cameraInput.x;
            _mouseY = _cameraInput.y;
        }
    }
}
