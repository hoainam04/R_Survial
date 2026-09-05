using UnityEngine;
using PROJ.Item;
public enum ThrowableType
{
    Grenade,
    Molotov,
    Flashbang,
    SlowBomb,
}
public class ThrowableSO : ItemSO, IAttributeDisplayable
{
    public override bool isStackable => true; // Ghi đè để luôn trả về true, vì vật phẩm ném có thể xếp chồng
    public override ItemType itemType => ItemType.Throwable; // Ghi đè để luôn trả về Throwable
    [Header("Thông tin vật phẩm ném")]
    public GameObject throwablePrefab; // Prefab của vật phẩm ném
    public float throwForce; // Lực ném của vật phẩm
    public ThrowableType throwableType; // Loại vật phẩm ném
    public float damage; // Sát} thương gây ra khi ném vật phẩm
    public float areaOfEffect; // Bán kính ảnh hưởng của vật phẩm ném (nếu có)
    public float duration; // Thời gian hiệu lực của vật phẩm ném (nếu có)

    public string GetAttributeSummary()
    {
        string summary = "";
        if (throwForce != 0) summary += $"Throw Force: {throwForce}\n";
        if (damage != 0) summary += $"Damage: {damage}\n";
        if (areaOfEffect != 0) summary += $"Area of Effect: {areaOfEffect}\n";
        if (duration != 0) summary += $"Duration: {duration}\n";

        return string.IsNullOrEmpty(summary) ? "" : summary.TrimEnd('\n');
    }
}