using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PROJ.Item;

namespace PROJ.UI
{
    public class HotbarSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI keyLabel;

        private Hotbar hotbar;
        private int slotIndex;

        public void Initialize(Hotbar targetHotbar, int index)
        {
            hotbar = targetHotbar;
            slotIndex = index;

            if (keyLabel != null)
            {
                keyLabel.text = (index + 1).ToString();
            }
        }

        public void SetData(ItemSO item, int count)
        {
            if (icon != null)
            {
                icon.enabled = item != null;
                icon.sprite = item != null ? item.itemIcon : null;
            }

            if (amountText != null)
            {
                amountText.enabled = item != null && count > 1;
                amountText.text = item != null && count > 1 ? count.ToString() : string.Empty;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (hotbar == null) return;

            ItemSO draggedItem = InventorySlotUI.CurrentlyDraggedItem;
            if (draggedItem == null) return;

            hotbar.AssignSlot(slotIndex, draggedItem);
        }
    }
}
