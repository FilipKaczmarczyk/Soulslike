using System;
using Items;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponSlotManager weaponSlotManager;
    
    public Weapon rightHandWeapon;
    public Weapon leftHandWeapon;

    private void Start()
    {
        weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, HandSlot.Right);
        weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, HandSlot.Left);
    }
}
