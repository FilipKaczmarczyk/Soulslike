using System;
using Input;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Rigidbody rigidbody;
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

            moveDirection = camera.forward * inputHandler.vertical;
            moveDirection += camera.right * inputHandler.horizontal;
            moveDirection.Normalize();

            moveDirection *= movementSpeed;

            var projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement

        private Vector3 _normalVector;
        private Vector3 _targetPosition;

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

        #endregion
    }
}
