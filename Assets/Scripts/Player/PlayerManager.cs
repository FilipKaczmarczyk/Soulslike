using Camera;
using Input;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Animator animator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController playerController;
        public bool IsInteracting { get; private set; }
        public bool IsSprinting { get; set; }
        public bool IsInAir { get; set; }
        public bool IsGrounded { get; set; }
        
        private static readonly int AnimatorIsInteracting = Animator.StringToHash("IsInteracting");

        private void Update()
        {
            var delta = Time.deltaTime;
            
            IsInteracting = animator.GetBool(AnimatorIsInteracting);
            
            inputHandler.TickInput(delta);

            playerController.HandleMovement(delta);
            playerController.HandleRollingAndSprinting(delta);
            playerController.HandleFall(delta);
        }
        
        private void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;

            if (cameraController == null) return;
            
            cameraController.FollowTarget(delta);
            cameraController.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
        }

        private void LateUpdate()
        {
            inputHandler.RollFlag = false;
            inputHandler.SprintFlag = false;
            IsSprinting = inputHandler.IsActionInputPressed;

            if (IsInAir)
            {
                playerController.inAirTimer += Time.deltaTime;
            }
        }
    }
}
