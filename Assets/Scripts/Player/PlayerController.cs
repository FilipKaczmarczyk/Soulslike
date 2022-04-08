using System;
using Camera;
using Input;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody rigidbody;
        
        [SerializeField] private Transform camera;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private AnimatorHandler animatorHandler;
        
        private Vector3 moveDirection;

        [Header("Statistics")] 
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        private void Start()
        {
            animatorHandler.Init();
        }

        private void Update()
        {
            var delta = Time.deltaTime;
            
            inputHandler.TickInput(delta);

            HandleMovement(delta);

            HandleRollingAndSprinting(delta);
            
            Debug.Log(inputHandler.rollFlag);
        }

        #region Movement

        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        private void HandleMovement(float delta)
        {
            moveDirection = camera.forward * inputHandler.vertical;
            moveDirection += camera.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            moveDirection *= movementSpeed;

            var projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0f);
            
            if (animatorHandler.CanRotate)
            {
                HandleRotation(delta);
            }
        }
        
        private void HandleRotation(float delta)
        {
            var moveOverride = inputHandler.moveAmount;

            var targetDirection = camera.forward * inputHandler.vertical;
            targetDirection += camera.right * inputHandler.horizontal;

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
        
        private void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool("IsInteracting"))
                return;

            if (inputHandler.rollFlag)
            {
                moveDirection = camera.forward * inputHandler.vertical;
                moveDirection += camera.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    var rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("BackStep", true);
                }
            }
        }

        #endregion
    }
}
