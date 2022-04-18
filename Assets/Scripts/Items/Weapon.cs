using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/New Weapon")]
    public class Weapon : Item
    {
        public GameObject weaponPrefab;
        public bool isUnarmed;

        [Header("One handed attack animation")] 
        public string oneHandLightAttack00;
        public string oneHandLightAttack01;
        public string oneHandHeavyAttack;
    }
}
