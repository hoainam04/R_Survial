using System;
using UnityEngine;

namespace PROJ.Item
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemStack stack;

        public ItemStack Stack => stack;
        public ItemSO Item => stack?.item;
        public int Amount => stack == null ? 0 : stack.amount;
        public bool IsEmpty => stack == null || stack.IsEmpty;

        public InventorySlot()
        {
            stack = new ItemStack(null, 0);
        }

        public void Set(ItemSO item, int amount)
        {
            if (item == null || amount <= 0)
            {
                Clear();
                return;
            }

            stack.item = item;
            stack.amount = Mathf.Clamp(amount, 1, Mathf.Max(1, item.maxStackSize));
        }

        public int AddAmount(int amount)
        {
            if (IsEmpty || amount <= 0) return amount;

            int space = stack.GetRemainingSpace();
            int added = Mathf.Min(space, amount);

            stack.amount += added;

            return amount - added;
        }

        public int RemoveAmount(int amount)
        {
            if (IsEmpty || amount <= 0) return 0;

            int removed = Mathf.Min(stack.amount, amount);
            stack.amount -= removed;

            if (stack.amount <= 0)
                Clear();

            return removed;
        }

        public bool CanStackWith(ItemSO item)
        {
            return !IsEmpty && stack.CanStackWith(item);
        }

        public void Clear()
        {
            stack.Clear();
        }
    }
}