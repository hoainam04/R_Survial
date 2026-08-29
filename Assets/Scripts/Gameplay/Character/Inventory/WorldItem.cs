using UnityEngine;

namespace PROJ.Item
{
    public class WorldItem : MonoBehaviour
    {
        [Header("Item Data")]
        [SerializeField] private ItemSO item;
        [SerializeField] private int amount = 1;

        public ItemSO Item => item;
        public int Amount => amount;

        public bool CanPickup => item != null && amount > 0;

        public void Initialize(ItemSO itemSO, int itemAmount)
        {
            item = itemSO;
            amount = Mathf.Max(1, itemAmount);
        }

        public void DestroyWorldItem()
        {
            Destroy(gameObject);
        }
    }
}