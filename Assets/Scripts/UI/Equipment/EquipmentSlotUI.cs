using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PROJ.Equipment;

namespace PROJ.UI
{
    public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Chọn 1 trong 2 chế độ bên dưới")]
        [SerializeField] private bool isWeaponLoadoutSlot;
        [SerializeField] private EquipmentSlot slotType; // Dùng khi isWeaponLoadoutSlot = false
        [SerializeField] private int weaponLoadoutIndex; // Dùng khi isWeaponLoadoutSlot = true (0 = Wep chính, 1 = Wep phụ)

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject backgroundName;
        [SerializeField] private GameObject activeHighlight; // Chỉ dùng cho ô vũ khí, đánh dấu vũ khí đang active

        private CharacterEquipmentManager equipmentManager;
        private EquipmentItemSO currentItem;
        private GameObject dragGhost;
        private Canvas rootCanvas;

        public static EquipmentSlotUI CurrentDragSource { get; private set; }
        public static bool IsDraggingEquippedItem => CurrentDragSource != null;

        public bool IsWeaponLoadoutSlot => isWeaponLoadoutSlot;
        public int WeaponLoadoutIndex => weaponLoadoutIndex;

        private void Awake()
        {
            rootCanvas = GetComponentInParent<Canvas>();
        }

        public void Initialize(CharacterEquipmentManager manager)
        {
            equipmentManager = manager;
        }

        public void Refresh()
        {
            if (equipmentManager == null) return;

            EquipmentItemSO displayItem = isWeaponLoadoutSlot
                ? equipmentManager.GetWeaponInLoadoutSlot(weaponLoadoutIndex)
                : equipmentManager.GetEquippedItem(slotType);

            bool isActive = isWeaponLoadoutSlot && equipmentManager.ActiveWeaponSlotIndex == weaponLoadoutIndex;
            SetData(displayItem, isActive);
        }

        private void SetData(EquipmentItemSO item, bool isActive)
        {
            currentItem = item;

            if (icon != null)
            {
                icon.enabled = item != null;
                icon.sprite = item != null ? item.itemIcon : null;
            }

            if (nameText != null)
            {
                nameText.enabled = item != null;
                nameText.text = item != null ? item.itemName : string.Empty;
                if (backgroundName != null)
                {
                    backgroundName.SetActive(item != null);
                }
            }

            if (activeHighlight != null)
            {
                activeHighlight.SetActive(isActive);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (equipmentManager == null) return;

            if (isWeaponLoadoutSlot && CurrentDragSource != null &&
                CurrentDragSource != this && CurrentDragSource.IsWeaponLoadoutSlot)
            {
                equipmentManager.SwapWeaponLoadoutSlots(
                    CurrentDragSource.WeaponLoadoutIndex,
                    weaponLoadoutIndex);
                return;
            }

            var draggedItem = InventorySlotUI.CurrentlyDraggedItem;

            if (isWeaponLoadoutSlot)
            {
                if (draggedItem is not WeaponEquipmentSO weapon) return;
                equipmentManager.AssignWeaponToLoadout(weaponLoadoutIndex, weapon);
                return;
            }

            if (draggedItem is not EquipmentItemSO equipmentItem) return;
            if (equipmentItem.slotType != slotType) return;
            if (equipmentItem is WeaponEquipmentSO) return; // Vũ khí chỉ gán qua ô Wep chính/phụ

            equipmentManager.EquipItem(equipmentItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemSelectionEvents.Select(currentItem);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentItem == null || equipmentManager == null) return;

            CurrentDragSource = this;
            dragGhost = new GameObject("EquipmentDragGhost", typeof(RectTransform), typeof(Image));
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

            if (CurrentDragSource == this)
            {
                CurrentDragSource = null;
            }
        }

        public void UnequipDraggedItem()
        {
            if (equipmentManager == null || currentItem == null) return;

            if (isWeaponLoadoutSlot)
            {
                equipmentManager.UnequipWeaponFromLoadout(weaponLoadoutIndex);
            }
            else
            {
                equipmentManager.UnequipItem(slotType);
            }
        }
    }
}
