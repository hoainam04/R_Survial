using UnityEngine;

namespace PROJ.Equipment
{
    public enum WeaponType
    {
        Melee,
        Ranged,
        Throwable,
    }

    // LƯU Ý: Xóa bỏ dòng [CreateAssetMenu] ở file gốc này đi, 
    // vì chúng ta không muốn tạo ra một Asset chung chung không rõ là súng hay kiếm.
    public abstract class WeaponEquipmentSO : EquipmentItemSO
    {
        public abstract WeaponType weaponType { get; }
        [Header("Cấu Hình Chung Vũ Khí")]

        public float damage = 20f;
        public float attackRange = 2f;      // Cận chiến: Bán kính chém | Súng: Khoảng cách bắn xa
        public float critChance = 0f; // Xác suất chí mạng (0-1)
        public float critDamageMultiplier = 1.5f; // Hệ số sát thương chí mạng (mặc định 1.5x)
        public float moveSpeedModifier = 0f; // Bonus tốc độ cộng thẳng vào MoveSpeed (có thể âm)
    }
}