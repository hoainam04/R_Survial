using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PROJ.Item;

namespace PROJ.UI
{
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject backgroundAmount;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject backgroundName;


        public static ItemSO CurrentlyDraggedItem { get; private set; }

        private ItemSO currentItem;
        private GameObject dragGhost;
        private Canvas rootCanvas;

        private void Awake()
        {
            rootCanvas = GetComponentInParent<Canvas>();
        }

        public void SetData(InventorySlot slot)
        {
            currentItem = slot == null || slot.IsEmpty ? null : slot.Item;
            int amount = slot == null ? 0 : slot.Amount;

            if (icon != null)
            {
                icon.enabled = currentItem != null;
                icon.sprite = currentItem != null ? currentItem.itemIcon : null;
            }

            if (amountText != null)
            {
                amountText.enabled = amount > 1;
                amountText.text = amount > 1 ? amount.ToString() : string.Empty;
                if (backgroundAmount != null)
                {
                    backgroundAmount.SetActive(amount > 1);
                }
            }
            
            if (nameText != null)
            {
                nameText.enabled = currentItem != null;
                nameText.text = currentItem != null ? currentItem.itemName : string.Empty;
                if (backgroundName != null)
                {
                    backgroundName.SetActive(currentItem != null);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentItem == null) return;

            CurrentlyDraggedItem = currentItem;

            dragGhost = new GameObject("DragGhost", typeof(RectTransform), typeof(Image));
            dragGhost.transform.SetParent(rootCanvas != null ? rootCanvas.transform : transform, false);

            var ghostImage = dragGhost.GetComponent<Image>();
            ghostImage.sprite = currentItem.itemIcon;
            ghostImage.raycastTarget = false;
            ghostImage.SetNativeSize();

            dragGhost.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragGhost != null)
            {
                dragGhost.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragGhost != null)
            {
                Destroy(dragGhost);
                dragGhost = null;
            }

            CurrentlyDraggedItem = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemSelectionEvents.Select(currentItem);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (EquipmentSlotUI.IsDraggingEquippedItem)
            {
                EquipmentSlotUI.CurrentDragSource.UnequipDraggedItem();
            }
        }
    }
}
