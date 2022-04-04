using UnityEngine;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        public float moveAmount;
        public float horizontal;
        public float vertical;
        
        [SerializeField] private float mouseX;
        [SerializeField] private float mouseY;

        private PlayerControls _inputActions;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

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
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }
    }
}
