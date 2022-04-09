using Input;
using UnityEngine;

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
        
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public void HandleMovement(float delta)
        {
            if (inputHandler.RollFlag)
                return;
                
            _moveDirection = mainCamera.forward * inputHandler.Vertical;
            _moveDirection += mainCamera.right * inputHandler.Horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            var speed = movementSpeed;
            
            if (inputHandler.SprintFlag)
            {
                speed = sprintSpeed;
                playerManager.IsSprinting = true;
            }
            
            _moveDirection *= speed;

            var projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, Vector3.zero);
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
                _moveDirection = mainCamera.forward * inputHandler.Vertical;
                _moveDirection += mainCamera.right * inputHandler.Horizontal;

                if (inputHandler.MoveAmount > 0)
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
