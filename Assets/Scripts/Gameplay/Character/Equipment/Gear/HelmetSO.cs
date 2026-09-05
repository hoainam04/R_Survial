using UnityEngine;
using PROJ.Item;
[CreateAssetMenu(fileName = "New Helmet Item", menuName = "PROJ/Item/HelmetItem")]
public class HelmetSO : EquipmentItemSO, IAttributeDisplayable
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì mũ bảo hiểm không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Helmet; // Ghi đè để luôn trả về Helmet

    // [Header("Thông tin mũ bảo hiểm")]
    // public float defenseBonus; // Chỉ số phòng thủ cộng thêm khi trang bị mũ bảo hiểm
    // // public float maxDurability; // Độ bền tối đa của mũ bảo hiểm
        public string GetAttributeSummary()
    {
        string summary = "";
        if (bonusDefense != 0) summary += $"Defense Bonus: {bonusDefense}\n";
        if (maxDurability != 0) summary += $"Max Durability: {maxDurability}\n";
        if (bonusMaxHealth != 0) summary += $"Max Health Bonus: {bonusMaxHealth}\n";
        if (bonusMaxStamina != 0) summary += $"Max Stamina Bonus: {bonusMaxStamina}\n";
        if (bonusCriticalRate != 0) summary += $"Critical Rate Bonus: {bonusCriticalRate}\n";
        if (bonusCriticalDamage != 0) summary += $"Critical Damage Bonus: {bonusCriticalDamage}\n";
        if (bonusMoveSpeed != 0) summary += $"Move Speed Bonus: {bonusMoveSpeed}\n";
        if (bonusDashSpeed != 0) summary += $"Dash Speed Bonus: {bonusDashSpeed}\n";
        if (bonusStaminaRegen != 0) summary += $"Stamina Regen Bonus: {bonusStaminaRegen}\n";
        if (bonusHPRegen != 0) summary += $"HP Regen Bonus: {bonusHPRegen}\n";

        return string.IsNullOrEmpty(summary) ? "" : summary.TrimEnd('\n');
    }
}