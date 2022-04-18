using System;
using Items;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [SerializeField] private WeaponHolderSlot leftHandSlot;
    [SerializeField] private WeaponHolderSlot rightHandSlot;

    private DamageCollider _leftHandDamageCollider;
    private DamageCollider _rightHandDamageCollider;

    private void Start()
    {
        var weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        
        foreach (var weaponHolderSlot in weaponHolderSlots)
        {
            switch (weaponHolderSlot.handSlot)
            {
                case HandSlot.Left:
                    leftHandSlot = weaponHolderSlot;
                    LoadLeftWeaponDamageCollider();
                    break;
                
                case HandSlot.Right:
                    rightHandSlot = weaponHolderSlot;
                    LoadRightWeaponDamageCollider();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void LoadWeaponOnSlot(Weapon weapon, HandSlot handSlot)
    {
        switch (handSlot)
        {
            case HandSlot.Left:
                leftHandSlot.LoadWeaponModel(weapon);
                break;
            
            case HandSlot.Right:
                rightHandSlot.LoadWeaponModel(weapon);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(handSlot), handSlot, null);
        }
    }
    
    #region Handle Weapon's Damage Colliders
    
    private void LoadLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    
    private void LoadRightWeaponDamageCollider()
    {
        _rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void EnableLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider.ToggleDamageCollider(true);
    }
    
    public void EnableRightWeaponDamageCollider()
    {
        _rightHandDamageCollider.ToggleDamageCollider(true);
    }
    
    public void DisableLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider.ToggleDamageCollider(false);
    }
    
    public void DisableRightWeaponDamageCollider()
    {
        _rightHandDamageCollider.ToggleDamageCollider(false);
    }
    
    #endregion
}
