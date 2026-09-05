using System;
using UnityEngine;
using PROJ.Attributes;

namespace PROJ.Item
{
    // Chỉ dùng nhanh Consumable/Throwable từ Inventory. Vũ khí chính/phụ KHÔNG thuộc Hotbar, xem CharacterEquipmentManager.
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(CharacterAttributeManager))]
    public class Hotbar : MonoBehaviour
    {
        private const int SlotCount = 5;

        private ItemSO[] assignedItems = new ItemSO[SlotCount];

        private Inventory inventory;
        private CharacterAttributeManager attributeManager;

        public event Action OnHotbarChanged;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            attributeManager = GetComponent<CharacterAttributeManager>();
        }

        private void Update()
        {
            if (GameSettingsManager.Instance == null) return;

            for (int i = 0; i < SlotCount; i++)
            {
                if (GameSettingsManager.Instance.GetKeyDown($"Slot{i + 1}"))
                {
                    UseSlot(i);
                }
            }
        }

        public ItemSO GetAssignedItem(int index)
        {
            if (index < 0 || index >= SlotCount) return null;
            return assignedItems[index];
        }

        public void AssignSlot(int index, ItemSO item)
        {
            if (index < 0 || index >= SlotCount) return;
            if (item != null && item is not ConsumableSO && item is not ThrowableSO) return;

            assignedItems[index] = item;
            OnHotbarChanged?.Invoke();
        }

        public void ClearSlot(int index)
        {
            if (index < 0 || index >= SlotCount) return;

            assignedItems[index] = null;
            OnHotbarChanged?.Invoke();
        }

        public void UseSlot(int index)
        {
            if (index < 0 || index >= SlotCount) return;

            ItemSO item = assignedItems[index];
            if (item == null) return;
            if (!inventory.HasItem(item, 1)) return;

            switch (item)
            {
                case ConsumableSO consumable:
                    consumable.UseConsumable(attributeManager);
                    break;
                case ThrowableSO throwable:
                    ThrowItem(throwable);
                    break;
                default:
                    return;
            }

            inventory.RemoveItem(item, 1);
        }

        private void ThrowItem(ThrowableSO throwable)
        {
            if (throwable.throwablePrefab == null) return;

            Vector3 spawnPosition = transform.position + transform.forward + Vector3.up;
            GameObject spawned = Instantiate(throwable.throwablePrefab, spawnPosition, Quaternion.LookRotation(transform.forward));

            if (spawned.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * throwable.throwForce, ForceMode.Impulse);
            }
        }
    }
}

