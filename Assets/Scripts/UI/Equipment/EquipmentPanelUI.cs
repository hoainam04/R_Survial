using UnityEngine;
using PROJ.Equipment;

namespace PROJ.UI
{
    public class EquipmentPanelUI : MonoBehaviour
    {
        [SerializeField] private CharacterEquipmentManager equipmentManager;
        [SerializeField] private EquipmentSlotUI[] slots; // Wep chính, Wep phụ, Helmet, Armor, Boots, Gloves, Backpack (kéo trong Inspector)

        private void Start()
        {
            if (equipmentManager == null)
            {
                equipmentManager = FindObjectOfType<CharacterEquipmentManager>();
            }

            if (equipmentManager == null)
            {
                Debug.LogWarning("[EquipmentPanelUI]: Không tìm thấy CharacterEquipmentManager trong scene.");
                return;
            }

            foreach (var slot in slots)
            {
                slot.Initialize(equipmentManager);
            }

            equipmentManager.OnEquipmentChanged += RefreshAll;
            equipmentManager.OnWeaponLoadoutChanged += RefreshAll;
            RefreshAll();
        }

        private void OnDestroy()
        {
            if (equipmentManager != null)
            {
                equipmentManager.OnEquipmentChanged -= RefreshAll;
                equipmentManager.OnWeaponLoadoutChanged -= RefreshAll;
            }
        }

        private void RefreshAll()
        {
            foreach (var slot in slots)
            {
                slot.Refresh();
            }
        }
    }
}
