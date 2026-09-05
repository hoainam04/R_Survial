using UnityEngine;

namespace PROJ.Item
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Throwable,
        QuestItem,
        CraftingMaterial,
        Ammo,
        Miscellaneous
    }

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Unique,
        Artifact,
    }


    public abstract class ItemSO : ScriptableObject
    {
        [Header("Thông tin cơ bản")]
        public string itemName;
        public Sprite itemIcon;

        public string description;
        public abstract bool isStackable { get; }
        public int maxStackSize = 1; // Số lượng tối đa trong một stack (nếu là vật phẩm có thể xếp chồng)

        [Header("Phân loại")]
        public abstract ItemType itemType { get; }

        [Header("Giá trị bán lại")]
        public float sellPrice;

        [Header("Độ hiếm của vật phẩm")]
        public ItemRarity rarity;
        [Header("Weight (trọng lượng) của vật phẩm")]
        public float weight;
    }
    public interface IAttributeDisplayable
    {
        // Có thể trả về chuỗi đã format hoặc trả về danh sách Key-Value để tránh split string
        string GetAttributeSummary();
    }
}