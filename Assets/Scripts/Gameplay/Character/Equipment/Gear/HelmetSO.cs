using UnityEngine;
using PROJ.Item;
[CreateAssetMenu(fileName = "New Helmet Item", menuName = "PROJ/Item/HelmetItem")]
public class HelmetSO : EquipmentItemSO
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì mũ bảo hiểm không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Helmet; // Ghi đè để luôn trả về Helmet

    // [Header("Thông tin mũ bảo hiểm")]
    // public float defenseBonus; // Chỉ số phòng thủ cộng thêm khi trang bị mũ bảo hiểm
    // // public float maxDurability; // Độ bền tối đa của mũ bảo hiểm
}