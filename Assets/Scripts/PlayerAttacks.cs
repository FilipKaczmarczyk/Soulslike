using System.Collections;
using System.Collections.Generic;
using Items;
using Player;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private AnimatorHandler animatorHandler;
    
    public void HandleLightAttack(Weapon weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.oneHandLightAttack, true);
    }
    
    public void HandleHeavyAttack(Weapon weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.oneHandHeavyAttack, true);
    }
}
