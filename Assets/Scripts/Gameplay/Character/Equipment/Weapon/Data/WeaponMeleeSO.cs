using UnityEngine;

namespace PROJ.Equipment
{
    public enum MeleeType
    {
        Sword,
        Dagger,
        HeavyWeapon,
        Stick,
    }
    [CreateAssetMenu(fileName = "New Melee Weapon", menuName = "PROJ/Equipment/Weapon - Melee")]
    public class WeaponMeleeSO : WeaponEquipmentSO
    {
        public override bool isStackable => false; // Ghi đè để luôn trả về false, vì vũ khí cận chiến không thể xếp chồng
        public override WeaponType weaponType => WeaponType.Melee; // Ghi đè để luôn trả về Melee
        public override EquipmentSlot slotType => EquipmentSlot.Weapon; // Ghi đè để luôn trả về Weapon
        [Header("Đặc Tính Cận Chiến")]
        [Tooltip("Thời gian chờ vung kiếm đến điểm vàng rồi mới bật hitbox quét sát thương")]
        public MeleeType meleeType;
        public float attackCooldown = 0.5f; // Cận chiến: Thời gian khựng đòn
        public float hitboxDelay = 0.2f;

        // Ông có thể mở rộng thêm các chỉ số đặc trưng sau này như:
        // public float knockbackForce = 5f; // Lực đẩy lùi quái
    }
}