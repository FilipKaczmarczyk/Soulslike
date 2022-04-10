using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/New Weapon")]
    public class Weapon : Item
    {
        public GameObject weaponPrefab;
        public bool isUnarmed;
    }
}
