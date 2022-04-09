using System;
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
         
        [Header("Flags")]
        public bool isInteracting;
        public bool isSprinting;
        
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        private void Update()
        {
            var delta = Time.deltaTime;
            
            isInteracting = animator.GetBool(IsInteracting);
            
            inputHandler.TickInput(delta);

            playerController.HandleMovement(delta);
            playerController.HandleRollingAndSprinting(delta);
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
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler._isActionInputPressed;
        }
    }
}
