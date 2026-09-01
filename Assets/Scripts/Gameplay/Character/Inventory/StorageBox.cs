using System;
using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Item
{
    public class StorageBox : MonoBehaviour
    {
        [Header("Storage Settings")]
        [SerializeField] private int slotCount = 20;
        [SerializeField] private List<InventorySlot> slots = new();

        public IReadOnlyList<InventorySlot> Slots => slots;
        public int SlotCount => slotCount;

        public event Action OnStorageChanged;

        private void Awake()
        {
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            slots.Clear();
            int count = Mathf.Max(1, slotCount);
            for (int i = 0; i < count; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        public bool AddItem(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            int remaining = amount;

            if (item.isStackable)
            {
                foreach (var slot in slots)
                {
                    if (!slot.CanStackWith(item)) continue;

                    remaining = slot.AddAmount(remaining);

                    if (remaining <= 0)
                    {
                        OnStorageChanged?.Invoke();
                        return true;
                    }
                }
            }

            foreach (var slot in slots)
            {
                if (!slot.IsEmpty) continue;

                int stackAmount = item.isStackable
                    ? Mathf.Min(remaining, Mathf.Max(1, item.maxStackSize))
                    : 1;

                slot.Set(item, stackAmount);
                remaining -= stackAmount;

                if (remaining <= 0)
                {
                    OnStorageChanged?.Invoke();
                    return true;
                }
            }

            OnStorageChanged?.Invoke();
            return false;
        }

        public bool RemoveItem(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            int remaining = amount;

            foreach (var slot in slots)
            {
                if (slot.IsEmpty || slot.Item != item) continue;

                int removed = slot.RemoveAmount(remaining);
                remaining -= removed;

                if (remaining <= 0)
                {
                    OnStorageChanged?.Invoke();
                    return true;
                }
            }

            OnStorageChanged?.Invoke();
            return false;
        }

        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= slots.Count)
                return null;

            return slots[index];
        }

        public void SwapSlots(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= slots.Count) return;
            if (indexB < 0 || indexB >= slots.Count) return;
            if (indexA == indexB) return;

            ItemSO itemA = slots[indexA].Item;
            int amountA = slots[indexA].Amount;

            slots[indexA].Set(slots[indexB].Item, slots[indexB].Amount);
            slots[indexB].Set(itemA, amountA);

            OnStorageChanged?.Invoke();
        }
    }
}
