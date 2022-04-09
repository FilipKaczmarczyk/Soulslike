using System;
using Camera;
using Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody playerRigidbody;

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private AnimatorHandler animatorHandler;
        
        [Header("Movement statistics")] 
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float rotationSpeed;
        
        private Vector3 _moveDirection;
        
        private void Start()
        {
            animatorHandler.Init();
        }
        
        #region Movement

        private Vector3 _normalVector;
        private Vector3 _targetPosition;
        
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;
                
            _moveDirection = mainCamera.forward * inputHandler.vertical;
            _moveDirection += mainCamera.right * inputHandler.horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            var speed = movementSpeed;
            
            if (inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
            }
            
            _moveDirection *= speed;

            var projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            playerRigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0f, playerManager.isSprinting);
            
            if (animatorHandler.CanRotate)
            {
                HandleRotation(delta);
            }
        }
        
        private void HandleRotation(float delta)
        {
            var moveOverride = inputHandler.moveAmount;

            var targetDirection = mainCamera.forward * inputHandler.vertical;
            targetDirection += mainCamera.right * inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }
            
            var lookRotation = Quaternion.LookRotation(targetDirection);
            var targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * delta);

            transform.rotation = targetRotation;
        }
        
        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool(IsInteracting))
                return;

            if (inputHandler.rollFlag)
            {
                _moveDirection = mainCamera.forward * inputHandler.vertical;
                _moveDirection += mainCamera.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    _moveDirection.y = 0;
                    var rollRotation = Quaternion.LookRotation(_moveDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                   // animatorHandler.PlayTargetAnimation("BackStep", true);
                }
            }
        }

        #endregion
    }
}
