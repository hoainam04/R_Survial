using System;
using UnityEngine;
using PROJ.Attributes;

namespace PROJ.Item
{
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

            if (item is not ConsumableSO consumable) return;
            if (!inventory.HasItem(item, 1)) return;

            consumable.UseConsumable(attributeManager);
            inventory.RemoveItem(item, 1);
        }
    }
}
