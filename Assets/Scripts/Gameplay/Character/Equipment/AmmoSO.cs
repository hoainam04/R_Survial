using UnityEngine;
using PROJ.Item;

[CreateAssetMenu(fileName = "New Ammo Item", menuName = "PROJ/Item/Ammo")]
public class AmmoSO : ItemSO
{
    public override bool isStackable => true; // Ghi đè để luôn trả về true, vì đạn dược có thể xếp chồng
    public override ItemType itemType => ItemType.Ammo; // Ghi đè để luôn trả về Ammo
    [Header("Thông tin đạn dược")]
    // public int ammoCount; // Số lượng đạn trong vật phẩm
    public float damageModifier; // Hệ số sát thương của loại đạn này
    public float armorPenetration; // Khả năng xuyên giáp của loại đạn này
}