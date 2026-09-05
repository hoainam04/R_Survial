using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PROJ.Item;

namespace PROJ.UI
{
    public class HotbarSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject backgroundAmount;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject backgroundKeyLabel;
        [SerializeField] private TextMeshProUGUI keyLabel;
        [SerializeField] private GameObject backgroundName;
        [SerializeField] private TextMeshProUGUI nameText;

        private Hotbar hotbar;
        private int slotIndex;
        private ItemSO currentItem;

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
            currentItem = item;

            if (icon != null)
            {
                icon.enabled = item != null;
                icon.sprite = item != null ? item.itemIcon : null;
            }

            if (amountText != null)
            {
                amountText.enabled = item != null && count > 1;
                amountText.text = item != null && count > 1 ? count.ToString() : string.Empty;
                if (backgroundAmount != null)
                {
                    backgroundAmount.SetActive(item != null && count > 1);
                }
            }

            if (nameText != null)
            {
                nameText.enabled = item != null;
                nameText.text = item != null ? item.itemName : string.Empty;
                if (backgroundName != null)
                {
                    backgroundName.SetActive(item != null);
                }

                if (keyLabel != null)
                {
                    keyLabel.enabled = true;
                    if (backgroundKeyLabel != null)
                    {
                        backgroundKeyLabel.SetActive(true);
                    }
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (hotbar == null) return;

            ItemSO draggedItem = InventorySlotUI.CurrentlyDraggedItem;
            if (draggedItem == null) return;
            if (draggedItem is not ConsumableSO && draggedItem is not ThrowableSO) return;

            hotbar.AssignSlot(slotIndex, draggedItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemSelectionEvents.Select(currentItem);
        }
    }
}
