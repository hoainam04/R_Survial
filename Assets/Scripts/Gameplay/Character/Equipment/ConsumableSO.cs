using UnityEngine;
using PROJ.Item;
using PROJ.Attributes;
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "PROJ/Item/ConsumableItem")]
public class ConsumableSO : ItemSO
{
    public override ItemType itemType => ItemType.Consumable; // Ghi đè để luôn trả về Consumable
    [Header("Thông tin vật phẩm tiêu hao")]
    public float healthRestoreAmount;
    public float staminaRestoreAmount;
    public float thirstRestoreAmount;
    public float hungerRestoreAmount;
    public float duration; // Thời gian hiệu lực của vật phẩm (nếu có)
// public bool isStackable; // Có thể xếp chồng hay không
    public override bool isStackable => true; // Ghi đè để luôn trả về true, vì vật phẩm tiêu hao có thể xếp chồng
    public void UseConsumable(CharacterAttributeManager attributeManager)
    {
        if (attributeManager == null) return;

        attributeManager.GetAttribute(AttributeType.Health)?.Modify(healthRestoreAmount);
        attributeManager.GetAttribute(AttributeType.Stamina)?.Modify(staminaRestoreAmount);
        attributeManager.GetAttribute(AttributeType.Thirst)?.Modify(thirstRestoreAmount);
        attributeManager.GetAttribute(AttributeType.Hunger)?.Modify(hungerRestoreAmount);
    }

}