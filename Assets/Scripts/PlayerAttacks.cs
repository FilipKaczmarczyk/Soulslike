using Input;
using Items;
using Player;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private AnimatorHandler animatorHandler;
    [SerializeField] private InputHandler inputHandler;
    private string _lastAttack;
        
    private static readonly int Combo = Animator.StringToHash("Combo");

    public void WeaponCombo(Weapon weapon)
    {
        if (inputHandler.ComboFlag)
        {
            animatorHandler.anim.SetBool(Combo, false);
        
            if (_lastAttack == weapon.oneHandLightAttack00)
            {
                animatorHandler.PlayTargetAnimation(weapon.oneHandLightAttack01, true);
            }
        }
    }
    
    public void HandleLightAttack(Weapon weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.oneHandLightAttack00, true);
        _lastAttack = weapon.oneHandLightAttack00;
    }
    
    public void HandleHeavyAttack(Weapon weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.oneHandHeavyAttack, true);
        _lastAttack = weapon.oneHandHeavyAttack;
    }
}
