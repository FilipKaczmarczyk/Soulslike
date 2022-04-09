using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        
        public float MoveAmount { get; private set; }
        public float Horizontal{ get; private set; }
        public float Vertical { get; private set; }
        
        public bool IsActionInputPressed { get; private set; }
        public bool RollFlag { get; set; }
        public bool SprintFlag { get; set; }

        private PlayerControls _inputActions;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private float _rollInputTimer;
        
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
            BodyMove();
            CameraMove();
            Roll(delta);
        }

        private void BodyMove()
        {
            Horizontal = _movementInput.x;
            Vertical = _movementInput.y;

            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
        }

        private void CameraMove()
        {
            MouseX = _cameraInput.x;
            MouseY = _cameraInput.y;
        }

        private void Roll(float delta)
        {
            IsActionInputPressed = _inputActions.PlayerActions.Roll.phase == InputActionPhase.Started;

            if (IsActionInputPressed)
            {
                _rollInputTimer += delta;
                SprintFlag = true;
            }
            else
            {
                if (_rollInputTimer > 0 && _rollInputTimer < 0.5f)
                {
                    SprintFlag = false;
                    RollFlag = true;
                }

                _rollInputTimer = 0;
            }
        }
        
    }
}
