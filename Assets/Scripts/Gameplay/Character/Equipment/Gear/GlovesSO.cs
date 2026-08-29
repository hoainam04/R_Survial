using UnityEngine;
using PROJ.Item;
[CreateAssetMenu(fileName = "New Gloves Item", menuName = "PROJ/Item/GlovesItem")]
public class GlovesSO : EquipmentItemSO
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì găng tay không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Gloves; // Ghi đè để luôn trả về Gloves

    // [Header("Thông tin găng tay")]
    // public float defenseBonus; // Chỉ số phòng thủ cộng thêm khi trang bị găng tay
    // public float maxDurability; // Độ bền tối đa của găng tay
}