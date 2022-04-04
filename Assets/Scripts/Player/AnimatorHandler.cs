using UnityEngine;

namespace Player
{
    public class AnimatorHandler : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        private int _vertical;
        private int _horizontal;

        public bool canRotate;

        public void Init()
        {
            _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical

            float v = 0;
            
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement > -0.55f && verticalMovement < 0)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else
            {
                v = 0;
            }
            #endregion
            
            #region Horizontal

            float h = 0;
            
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                v = 1;
            }
            else if (horizontalMovement > -0.55f && horizontalMovement < 0)
            {
                v = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                v = -1f;
            }
            else
            {
                v = 0;
            }
            
            #endregion
            
            anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
        }

        public void SetRotate(bool state)
        {
            canRotate = state;
        }
    }
}
