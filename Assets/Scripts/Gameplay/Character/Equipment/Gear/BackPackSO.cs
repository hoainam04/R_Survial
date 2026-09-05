using UnityEngine;
using PROJ.Item;

[CreateAssetMenu(fileName = "New Backpack SO", menuName = "PROJ/Item/BackpackSO")]
public class BackPackSO : EquipmentItemSO, IAttributeDisplayable
{
    public override bool isStackable => false; // Ghi đè để luôn trả về false, vì balo không thể xếp chồng
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    public override EquipmentSlot slotType => EquipmentSlot.Backpack; // Ghi đè để luôn trả về Backpack

    [Header("Thông tin balo")]
    public int bonusInventorySlots; // Số ô chứa đồ bổ sung khi trang bị balo
    
    public string GetAttributeSummary()
    {
        string summary = "";
        if (bonusInventorySlots != 0) summary += $"Bonus Inventory Slots: {bonusInventorySlots}\n";
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