using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [SerializeField] private List<string> targetBools;
    [SerializeField] private bool status;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var targetBool in targetBools)
        {
            animator.SetBool(targetBool, status);
        }
    }
}
