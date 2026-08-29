using UnityEngine;

namespace PROJ.Equipment
{
    public class EquipmentHolder : MonoBehaviour
    {
        [Header("Các điểm gắn trang bị trên Model")]
        [SerializeField] private Transform weaponRightHandSlot; // Xương tay phải
        [SerializeField] private Transform helmetSlot;          // Xương đầu
        [SerializeField] private Transform armorSlot;           // Xương ngực (nếu cần gắn giáp tấm)
        [SerializeField] private Transform bootsSlot;
        [SerializeField] private Transform glovesSlot;
        [SerializeField] private Transform backpackSlot;        // Xương lưng (nếu cần gắn balo)

        // Hàm lấy slot tương ứng với loại trang bị
        public Transform GetSlotTransform(EquipmentSlot slot)
        {
            return slot switch
            {
                EquipmentSlot.Weapon => weaponRightHandSlot,
                EquipmentSlot.Helmet => helmetSlot,
                EquipmentSlot.Armor  => armorSlot,
                EquipmentSlot.Boots => bootsSlot,
                EquipmentSlot.Gloves => glovesSlot,
                EquipmentSlot.Backpack => backpackSlot,
                _ => null
            };
        }
    }
}