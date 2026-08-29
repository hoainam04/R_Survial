using UnityEngine;
using PROJ.Item;
    [CreateAssetMenu(fileName = "New Crafting Material", menuName = "PROJ/Item/CraftingMaterial")]
public class CraftingMaterial : ItemSO
{
    public override bool isStackable => true; // Ghi đè để luôn trả về true, vì vật phẩm chế tạo có thể xếp chồng
    public override ItemType itemType => ItemType.CraftingMaterial; // Ghi đè để luôn trả về CraftingMaterial
    [Header("Thông tin vật phẩm chế tạo")]
    public float craftingValue; // Giá trị chế tạo của vật phẩm
}