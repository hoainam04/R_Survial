using UnityEngine;
using PROJ.Item;
[CreateAssetMenu(fileName = "New Boots Item", menuName = "PROJ/Item/BootsItem")]
public class BootsSO : EquipmentItemSO
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì giày không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Boots; // Ghi đè để luôn trả về Boots

    // [Header("Thông tin giày")]
    // public float defenseBonus; // Chỉ số phòng thủ cộng thêm khi trang bị giày
    // public float maxDurability; // Độ bền tối đa của giày
}