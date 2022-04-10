using Input;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody playerRigidbody;
        public Vector3 moveDirection;
        
        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private AnimatorHandler animatorHandler;
        
        [Header("Movement statistics")] 
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float fallSped;
        
        [Header("Air and ground detection statistics")]
        [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] private float minimumDistanceToBeginFall = 1f;
        [SerializeField] private float groundDetectionRayDistance = 0.2f;
        [SerializeField] private LayerMask ignoreGroundCheck;

        public float inAirTimer;
        
        private void Start()
        {
            animatorHandler.Init();
            
            playerManager.IsGrounded = true;
            ignoreGroundCheck = ~(1 << 8 | 1 << 11);
        }
        
        #region Movement
        
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public void HandleMovement(float delta)
        {
            if (inputHandler.RollFlag)
                return;

            if (playerManager.IsInteracting)
                return;
            
            moveDirection = mainCamera.forward * inputHandler.Vertical;
            moveDirection += mainCamera.right * inputHandler.Horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            var speed = movementSpeed;
            
            if (inputHandler.SprintFlag && inputHandler.MoveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.IsSprinting = true;
            }

            if (inputHandler.MoveAmount < 0.5f)
            {
                playerManager.IsSprinting = false;
            }
            
            moveDirection *= speed;

            var projectedVelocity = Vector3.ProjectOnPlane(moveDirection, Vector3.zero);
            playerRigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount, 0f, playerManager.IsSprinting);
            
            if (animatorHandler.CanRotate)
            {
                HandleRotation(delta);
            }
        }
        
        private void HandleRotation(float delta)
        {
            var targetDirection = mainCamera.forward * inputHandler.Vertical;
            targetDirection += mainCamera.right * inputHandler.Horizontal;

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

            if (inputHandler.RollFlag)
            {
                moveDirection = mainCamera.forward * inputHandler.Vertical;
                moveDirection += mainCamera.right * inputHandler.Horizontal;

                if (inputHandler.MoveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Roll", true);
                    moveDirection.y = 0;
                    var rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                   // animatorHandler.PlayTargetAnimation("BackStep", true);
                }
            }
        }

        public void HandleFall(float delta)
        {
            playerManager.IsGrounded = false;
            var origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, transform.forward, out var hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.IsInAir)
            {
                playerRigidbody.AddForce(-Vector3.up * fallSped);
                playerRigidbody.AddForce(moveDirection * fallSped / 10f);
            }

            var dir = moveDirection;
            dir.Normalize();
            origin += dir * groundDetectionRayDistance;

            var targetPosition = transform.position;
            
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceToBeginFall, Color.red, 0.1f, false);
            
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToBeginFall, ignoreGroundCheck))
            {
                var tp = hit.point;
                playerManager.IsGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.IsInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Movement", true);
                    }
                    
                    inAirTimer = 0;
                    playerManager.IsInAir = false;
                }
            }
            else
            {
                if (playerManager.IsGrounded)
                {
                    playerManager.IsGrounded = false;
                }

                if (playerManager.IsInAir == false)
                {
                    if (playerManager.IsInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Fall", true);
                    }

                    var vel = playerRigidbody.velocity;
                    vel.Normalize();

                    playerRigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.IsInAir = true;
                }
            }

            if (playerManager.IsGrounded)
            {
                if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        #endregion
    }
}
