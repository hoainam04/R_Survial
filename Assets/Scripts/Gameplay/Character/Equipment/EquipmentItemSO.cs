using UnityEngine;
using PROJ.Item;

// namespace PROJ.Equipment
// {
public enum EquipmentSlot
{
    Weapon,
    Helmet,
    Armor,
    Boots,
    Gloves,
    Backpack,
}

// [CreateAssetMenu(fileName = "New Equipment", menuName = "PROJ/Item/EquipmentItem")]
public abstract class EquipmentItemSO : ItemSO
{
    public override ItemType itemType => ItemType.Equipment; // Ghi đè để luôn trả về Equipment
    [Header("Trang bị")]
    public abstract EquipmentSlot slotType { get; }

    [Header("Prefab Hiển thị (Visual)")]
    public GameObject equipmentPrefab;

    [Header("Chỉ số cộng thêm (Attributes Modification)")]
    public float bonusDamage;
    public float bonusDefense;
    public float bonusMaxHealth;
    public float bonusMaxStamina;
    public float bonusCriticalRate;
    public float bonusCriticalDamage;
    public float bonusMoveSpeed;
    public float bonusDashSpeed;
    public float bonusStaminaRegen;
    public float bonusHPRegen;
    public float bonusAttackRange;

    [Header("Độ bền")]
    public float maxDurability;
}
// }