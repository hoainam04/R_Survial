using UnityEngine;
using PROJ.Equipment;

public class EquipmentTestController : MonoBehaviour
{
    [Header("Hệ thống quản lý gốc")]
    private CharacterEquipmentManager equipmentManager;

    [Header("Danh sách dữ liệu vũ khí Test (Kéo thả ScriptableObject vào đây)")]
    [SerializeField] private WeaponEquipmentSO weaponSlot1; // Phím 1 (Ví dụ: Kiếm Melee)
    [SerializeField] private WeaponEquipmentSO weaponSlot2; // Phím 2 (Ví dụ: Cung Bow)
    [SerializeField] private WeaponEquipmentSO weaponSlot3; // Phím 3 (Ví dụ: Súng Pistol)
    [SerializeField] private WeaponEquipmentSO weaponSlot4; // Phím 4 (Ví dụ: Súng trường AR)
    [SerializeField] private EquipmentItemSO equipmentSlot5; // Phím 5 (Ví dụ: item 1)

    private void Awake()
    {
        // Tự động tìm Equipment Manager trên cùng GameObject
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        
        if (equipmentManager == null)
        {
            Debug.LogError("[EquipmentTest]: Không tìm thấy CharacterEquipmentManager trên GameObject này!");
        }
    }

    private void Update()
    {
        if (equipmentManager == null) return;

        var settings = GameSettingsManager.Instance;

        // Nhấn phím 1 -> Trang bị vũ khí 1
        bool slot1Pressed = settings != null ? settings.GetKeyDown("Slot1") : (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1));
        if (slot1Pressed)
        {
            TryEquip(weaponSlot1, "Slot 1");
        }

        // Nhấn phím 2 -> Trang bị vũ khí 2
        bool slot2Pressed = settings != null ? settings.GetKeyDown("Slot2") : (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2));
        if (slot2Pressed)
        {
            TryEquip(weaponSlot2, "Slot 2");
        }

        // Nhấn phím 3 -> Trang bị vũ khí 3
        bool slot3Pressed = settings != null ? settings.GetKeyDown("Slot3") : (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3));
        if (slot3Pressed)
        {
            TryEquip(weaponSlot3, "Slot 3");
        }

        // Nhấn phím 4 -> Trang bị vũ khí 4
        bool slot4Pressed = settings != null ? settings.GetKeyDown("Slot4") : (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4));
        if (slot4Pressed)
        {
            TryEquip(weaponSlot4, "Slot 4");
        }
        // Nhấn phím 5 -> Trang bị balo
        bool slot5Pressed = settings != null ? settings.GetKeyDown("Slot5") : (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5));
        if (slot5Pressed)
        {
            TryEquip(equipmentSlot5, "Slot 5");
        }
        // Nhấn phím T -> Tháo hoàn toàn vũ khí ra (Trở về tay không / Cận chiến thô sơ)
        if (Input.GetKeyDown(KeyCode.T))
        {
            equipmentManager.UnequipItem(EquipmentSlot.Weapon);
            equipmentManager.UnequipItem(EquipmentSlot.Backpack);
            equipmentManager.UnequipItem(EquipmentSlot.Helmet);
            equipmentManager.UnequipItem(EquipmentSlot.Armor);
            equipmentManager.UnequipItem(EquipmentSlot.Boots);
            equipmentManager.UnequipItem(EquipmentSlot.Gloves);
            Debug.Log("[EquipmentTest]: Đã chủ động THÁO VŨ KHÍ (Tay không)!");
        }
    }
    private void TryEquip(EquipmentItemSO item, string slotName)
    {
        if (item != null)
        {
            Debug.Log($"[EquipmentTest]: Đang kích hoạt nhanh {slotName}: {item.itemName}");
            equipmentManager.EquipItem(item);
        }
        else
        {
            Debug.LogWarning($"[EquipmentTest]: {slotName} đang trống! Hãy kéo ScriptableObject vào Inspector.");
        }
    }

    private void TryEquip(WeaponEquipmentSO weapon, string slotName)
    {
        if (weapon != null)
        {
            Debug.Log($"[EquipmentTest]: Đang kích hoạt nhanh {slotName}: {weapon.itemName}");
            equipmentManager.EquipItem(weapon);
        }
        else
        {
            Debug.LogWarning($"[EquipmentTest]: {slotName} đang trống! Hãy kéo ScriptableObject vũ khí vào Inspector.");
        }
    }
}