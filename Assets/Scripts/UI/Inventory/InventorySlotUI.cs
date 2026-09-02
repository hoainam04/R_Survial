using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PROJ.Item;

namespace PROJ.UI
{
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;

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
    }
}
