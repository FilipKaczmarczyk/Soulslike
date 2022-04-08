using Input;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private Animator _animator;
        
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        private void Update()
        {
            _inputHandler.isInteracting = _animator.GetBool(IsInteracting);
            _inputHandler.rollFlag = false;
        }
    }
}
