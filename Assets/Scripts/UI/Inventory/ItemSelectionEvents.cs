using System;
using PROJ.Item;

namespace PROJ.UI
{
    // Kênh sự kiện UI-only để các widget (Inventory/Hotbar/Equipment) báo item đang được chọn cho ItemDescriptionUI
    public static class ItemSelectionEvents
    {
        public static event Action<ItemSO> OnItemSelected;

        public static void Select(ItemSO item)
        {
            OnItemSelected?.Invoke(item);
        }

        public static void ClearSelection()
        {
            OnItemSelected?.Invoke(null);
        }
    }
}
