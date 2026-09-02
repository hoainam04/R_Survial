using UnityEngine;
using PROJ.Item;

namespace PROJ.UI
{
    public class HotbarUI : MonoBehaviour
    {
        [SerializeField] private Hotbar hotbar;
        [SerializeField] private Inventory inventory;
        [SerializeField] private HotbarSlotUI[] slots = new HotbarSlotUI[5];

        private void Start()
        {
            if (hotbar == null)
            {
                hotbar = FindObjectOfType<Hotbar>();
            }

            if (inventory == null)
            {
                inventory = FindObjectOfType<Inventory>();
            }

            if (hotbar == null || inventory == null)
            {
                Debug.LogWarning("[HotbarUI]: Không tìm thấy Hotbar hoặc Inventory trong scene.");
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Initialize(hotbar, i);
            }

            hotbar.OnHotbarChanged += RefreshAll;
            inventory.OnInventoryChanged += RefreshAll;
            RefreshAll();
        }

        private void OnDestroy()
        {
            if (hotbar != null)
            {
                hotbar.OnHotbarChanged -= RefreshAll;
            }

            if (inventory != null)
            {
                inventory.OnInventoryChanged -= RefreshAll;
            }
        }

        private void RefreshAll()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSO item = hotbar.GetAssignedItem(i);
                int count = item != null ? inventory.GetItemCount(item) : 0;
                slots[i].SetData(item, count);
            }
        }
    }
}
