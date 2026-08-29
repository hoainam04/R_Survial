using UnityEngine;
using PROJ.Item;
[CreateAssetMenu(fileName = "New Armor Item", menuName = "PROJ/Item/ArmorItem")]
public class ArmorSO : EquipmentItemSO
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì giáp không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Armor; // Ghi đè để luôn trả về Armor

    // [Header("Thông tin áo giáp")]
    // public float defenseBonus; // Chỉ số phòng thủ cộng thêm khi trang bị áo giáp
    // public float maxDurability; // Độ bền tối đa của áo giáp
}   