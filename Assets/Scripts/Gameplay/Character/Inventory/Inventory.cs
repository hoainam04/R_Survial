using System;
using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Item
{
    public class Inventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int baseSlotCount = 12;
        [SerializeField] private List<InventorySlot> slots = new();

        private int bonusSlotCount;

        public IReadOnlyList<InventorySlot> Slots => slots;
        public int BaseSlotCount => baseSlotCount;
        public int BonusSlotCount => bonusSlotCount;
        public int TotalSlotCount => baseSlotCount + bonusSlotCount;

        public event Action OnInventoryChanged;

        private void Awake()
        {
            ResizeSlots(TotalSlotCount);
        }

        public void SetBonusSlots(int value)
        {
            bonusSlotCount = Mathf.Max(0, value);
            ResizeSlots(TotalSlotCount);
            OnInventoryChanged?.Invoke();
        }

        public bool CanRemoveBonusSlots(int newBonusSlotCount)
        {
            int targetCount = baseSlotCount + Mathf.Max(0, newBonusSlotCount);

            if (targetCount >= slots.Count)
                return true;

            for (int i = targetCount; i < slots.Count; i++)
            {
                if (!slots[i].IsEmpty)
                    return false;
            }

            return true;
        }

        private void ResizeSlots(int targetCount)
        {
            targetCount = Mathf.Max(0, targetCount);

            while (slots.Count < targetCount)
            {
                slots.Add(new InventorySlot());
            }

            while (slots.Count > targetCount)
            {
                int lastIndex = slots.Count - 1;

                if (!slots[lastIndex].IsEmpty)
                {
                    Debug.LogWarning("[Inventory]: Không thể giảm slot vì slot cuối đang có item.");
                    break;
                }

                slots.RemoveAt(lastIndex);
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
                        OnInventoryChanged?.Invoke();
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
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            OnInventoryChanged?.Invoke();
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
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            OnInventoryChanged?.Invoke();
            return false;
        }

        public bool HasItem(ItemSO item, int amount = 1)
        {
            return GetItemCount(item) >= amount;
        }

        public int GetItemCount(ItemSO item)
        {
            if (item == null) return 0;

            int count = 0;

            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item == item)
                    count += slot.Amount;
            }

            return count;
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

            OnInventoryChanged?.Invoke();
        }
    }
}