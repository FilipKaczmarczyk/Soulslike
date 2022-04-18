using UnityEngine;

namespace Player
{
    public class AnimatorHandler : MonoBehaviour
    {
        public bool CanRotate { get; set; } = true;
        
        public Animator anim;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerManager playerManager;
        
        private int _vertical;
        private int _horizontal;

        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public void Init()
        {
            _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical

            float v;
            
            if (verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else if (verticalMovement > -0.55f && verticalMovement < 0)
            {
                v = -0.5f;
            }
            else if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1f;
            }
            else
            {
                v = 0;
            }
            
            #endregion
            
            #region Horizontal

            float h;
            
            if (horizontalMovement < -0.55f)
            {
                h = -1f;
            }
            else if (horizontalMovement > -0.55f && horizontalMovement < 0)
            {
                h = -0.5f;
            }
            else if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else
            {
                h = 0;
            }
            
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            
            anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            // anim.applyRootMotion = isInteracting;
            anim.SetBool(IsInteracting, isInteracting);
            anim.CrossFade(targetAnimation, 0.2f);
        }

        public void EnableCombo()
        {
            anim.SetBool("Combo", true);
        }
        
        public void DisableCombo()
        {
            anim.SetBool("Combo", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.IsInteracting == false)
                return;

            var delta = Time.deltaTime;
            playerController.playerRigidbody.drag = 0;
            var deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            var velocity = deltaPosition / delta;
            playerController.playerRigidbody.velocity = velocity;
        }
    }
}
