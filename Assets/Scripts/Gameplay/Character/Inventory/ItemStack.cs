using System;
using UnityEngine;

namespace PROJ.Item
{
    [Serializable]
    public class ItemStack
    {
        public ItemSO item;
        public int amount;

        public bool IsEmpty => item == null || amount <= 0;
        public int MaxStackSize => item == null ? 0 : Mathf.Max(1, item.maxStackSize);
        public bool IsFull => !IsEmpty && amount >= MaxStackSize;

        public ItemStack(ItemSO item, int amount)
        {
            this.item = item;
            this.amount = Mathf.Max(0, amount);
        }

        public int GetRemainingSpace()
        {
            if (IsEmpty) return 0;
            return MaxStackSize - amount;
        }

        public bool CanStackWith(ItemSO targetItem)
        {
            if (IsEmpty || targetItem == null) return false;
            if (item != targetItem) return false;
            if (!item.isStackable) return false;
            return amount < MaxStackSize;
        }

        public void Clear()
        {
            item = null;
            amount = 0;
        }
    }
}