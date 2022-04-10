using Items;
using UnityEngine;

public enum HandSlot
{
    Right,
    Left
}

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public HandSlot handSlot;
    public GameObject currentWeaponModel;

    private void UnloadModel()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    private void UnloadModelAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }
    
    public void LoadWeaponModel(Weapon weapon)
    {
        UnloadModelAndDestroy();
        
        if (weapon == null)
        {
            UnloadModel();
            return;
        }

        var model = Instantiate(weapon.weaponPrefab);
        
        if (model != null)
        {
            model.transform.parent = parentOverride != null ? parentOverride : transform;
            
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}
