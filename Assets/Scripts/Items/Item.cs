using UnityEngine;

namespace Items
{
    public class Item : ScriptableObject
    {
        [Header("Item information")]
        public Sprite itemIcon;
        public string itemName;
    }
}
