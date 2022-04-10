using System;
using Items;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [SerializeField] private WeaponHolderSlot leftHandSlot;
    [SerializeField] private WeaponHolderSlot rightHandSlot;

    private void Awake()
    {
        var weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        
        foreach (var weaponHolderSlot in weaponHolderSlots)
        {
            switch (weaponHolderSlot.handSlot)
            {
                case HandSlot.Left:
                    leftHandSlot = weaponHolderSlot;
                    break;
                
                case HandSlot.Right:
                    rightHandSlot = weaponHolderSlot;
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
}
