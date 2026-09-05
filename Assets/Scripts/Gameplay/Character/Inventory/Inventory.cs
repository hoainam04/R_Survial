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

        [Header("Drop Settings")]
        [SerializeField] private WorldItem worldItemPrefab;
        [SerializeField] private float dropDistance = 1f;
        [SerializeField] private float dropUpwardForce = 1f;

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

        public bool DropItem(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0 || !HasItem(item, amount)) return false;
            if (worldItemPrefab == null)
            {
                Debug.LogError("[Inventory]: Chưa gán World Item Prefab, không thể drop item.");
                return false;
            }

            Vector3 dropPosition = transform.position + transform.forward * dropDistance + Vector3.up;
            WorldItem droppedItem = Instantiate(worldItemPrefab, dropPosition, Quaternion.identity);
            droppedItem.Initialize(item, amount);

            if (droppedItem.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                Vector3 dropDirection = (transform.forward + Vector3.up * dropUpwardForce).normalized;
                rigidbody.AddForce(dropDirection * dropUpwardForce, ForceMode.Impulse);
            }

            return RemoveItem(item, amount);
        }
        public bool DropAllItem(ItemSO item)
        {
            if (item == null) return false;

            int totalAmount = GetItemCount(item);
            if (totalAmount <= 0) return false;

            return DropItem(item, totalAmount);
        }
        public bool HasItem(ItemSO item, int amount = 1)
        {
            return GetItemCount(item) >= amount;
        }

        public bool CanAddItem(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            int remaining = amount;

            if (item.isStackable)
            {
                foreach (var slot in slots)
                {
                    if (slot.CanStackWith(item))
                    {
                        remaining -= Mathf.Max(0, item.maxStackSize - slot.Amount);
                        if (remaining <= 0) return true;
                    }
                }
            }

            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    remaining -= item.isStackable ? Mathf.Max(1, item.maxStackSize) : 1;
                    if (remaining <= 0) return true;
                }
            }

            return false;
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
        public int EmptySlotCount
        {
            get
            {
                int count = 0;

                foreach (var slot in slots)
                {
                    if (slot.IsEmpty)
                        count++;
                }

                return count;
            }
        }
    }
}