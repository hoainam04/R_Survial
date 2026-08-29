using UnityEngine;
using PROJ.Item;

[CreateAssetMenu(fileName = "New Backpack SO", menuName = "PROJ/Item/BackpackSO")]
public class BackPackSO : EquipmentItemSO
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì balo không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Backpack; // Ghi đè để luôn trả về Backpack

    [Header("Thông tin balo")]
    public int bonusInventorySlots; // Số ô chứa đồ bổ sung khi trang bị balo
}