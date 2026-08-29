using System.Collections.Generic;
using UnityEngine;
using PROJ.Attributes;
using PROJ.Item;

namespace PROJ.Equipment
{
    [RequireComponent(typeof(CharacterAttributeManager))]
    [RequireComponent(typeof(EquipmentHolder))]
    [RequireComponent(typeof(Inventory))]
    public class CharacterEquipmentManager : MonoBehaviour
    {
        private CharacterAttributeManager attributeManager;
        private EquipmentHolder equipmentHolder;
        private Inventory inventory;

        private Dictionary<EquipmentSlot, EquipmentItemSO> currentEquipmentDict;
        private Dictionary<EquipmentSlot, GameObject> spawnedEquipmentVisuals;

        public WeaponType CurrentWeaponType { get; private set; } = WeaponType.Melee;
        public WeaponEquipmentSO CurrentWeaponData { get; private set; }

        [Header("Cấu Hình Mặc Định Khi Không Cầm Vũ Khí")]
        [SerializeField] private WeaponEquipmentSO defaultBareFistsAsset;

        private void Awake()
        {
            attributeManager = GetComponent<CharacterAttributeManager>();
            equipmentHolder = GetComponent<EquipmentHolder>();
            inventory = GetComponent<Inventory>();

            currentEquipmentDict = new Dictionary<EquipmentSlot, EquipmentItemSO>();
            spawnedEquipmentVisuals = new Dictionary<EquipmentSlot, GameObject>();

            CurrentWeaponData = defaultBareFistsAsset;
        }

        public void EquipItem(EquipmentItemSO newItem)
        {
            if (newItem == null) return;

            if (currentEquipmentDict.ContainsKey(newItem.slotType))
            {
                UnequipItem(newItem.slotType);
            }

            currentEquipmentDict.Add(newItem.slotType, newItem);

            ApplyEquipmentStats(newItem, true);
            ApplyInventoryBonus(newItem, true);
            SpawnEquipmentVisual(newItem);

            if (newItem.slotType == EquipmentSlot.Weapon)
            {
                if (newItem is WeaponEquipmentSO weaponItem)
                {
                    CurrentWeaponData = weaponItem;
                    CurrentWeaponType = weaponItem.weaponType;

                    Debug.Log($"[EquipmentManager]: Đã chuyển thế chiến đấu sang loại vũ khí: {weaponItem.itemName} - {CurrentWeaponType}");
                }
            }

            Debug.Log($"[EquipmentManager]: Đã trang bị thành công: {newItem.itemName}");
        }

        public void UnequipItem(EquipmentSlot slot)
        {
            if (!currentEquipmentDict.TryGetValue(slot, out var oldItem)) return;

            if (oldItem is BackPackSO backpack && backpack.bonusInventorySlots > 0)
            {
                if (!inventory.CanRemoveBonusSlots(0))
                {
                    Debug.LogWarning("[EquipmentManager]: Không thể tháo balo vì các slot mở rộng vẫn còn item.");
                    return;
                }
            }

            ApplyEquipmentStats(oldItem, false);
            ApplyInventoryBonus(oldItem, false);
            DestroyEquipmentVisual(slot);
            currentEquipmentDict.Remove(slot);

            if (slot == EquipmentSlot.Weapon)
            {
                CurrentWeaponData = defaultBareFistsAsset;
                CurrentWeaponType = defaultBareFistsAsset != null ? defaultBareFistsAsset.weaponType : WeaponType.Melee;

                Debug.Log("[EquipmentManager]: Tháo vũ khí, trả tư thế về mặc định.");
            }

            Debug.Log($"[EquipmentManager]: Đã tháo trang bị ở slot: {slot}");
        }

        private void ApplyInventoryBonus(EquipmentItemSO item, bool isEquipping)
        {
            if (item is not BackPackSO backpack) return;

            int targetBonus = isEquipping ? backpack.bonusInventorySlots : 0;
            inventory.SetBonusSlots(targetBonus);
        }

        private void ApplyEquipmentStats(EquipmentItemSO item, bool isEquipping)
        {
            float modifierMultiplier = isEquipping ? 1f : -1f;

            if (item is WeaponEquipmentSO weaponItem)
            {
                ApplyWeaponStats(modifierMultiplier, weaponItem);
            }
            else
            {
                ApplyNonWeaponStats(item, modifierMultiplier);
            }
        }

        private void ApplyWeaponStats(float modifierMultiplier, WeaponEquipmentSO weaponItem)
        {
            if (weaponItem.damage != 0)
            {
                var attackPowerAttr = attributeManager.GetAttribute(AttributeType.AttackPower);
                attackPowerAttr?.ModifyMaxValueByAmount(weaponItem.damage * modifierMultiplier);
            }

            if (weaponItem.attackRange != 0)
            {
                var attackRangeAttr = attributeManager.GetAttribute(AttributeType.AttackRange);
                attackRangeAttr?.ModifyMaxValueByAmount(weaponItem.attackRange * modifierMultiplier);
            }

            if (weaponItem.moveSpeedModifier != 0)
            {
                var moveSpeedAttr = attributeManager.GetAttribute(AttributeType.MoveSpeed);
                moveSpeedAttr?.ModifyMaxValueByAmount(weaponItem.moveSpeedModifier * modifierMultiplier);
            }
        }

        private void ApplyNonWeaponStats(EquipmentItemSO item, float modifierMultiplier)
        {
            if (item.bonusMaxHealth != 0)
            {
                var healthAttr = attributeManager.GetAttribute(AttributeType.Health);
                healthAttr?.ModifyMaxValueByAmount(item.bonusMaxHealth * modifierMultiplier);
            }

            if (item.bonusHPRegen != 0)
            {
                var hpRegenAttr = attributeManager.GetAttribute(AttributeType.HPRegen);
                hpRegenAttr?.ModifyMaxValueByAmount(item.bonusHPRegen * modifierMultiplier);
            }

            if (item.bonusDefense != 0)
            {
                var defenseAttr = attributeManager.GetAttribute(AttributeType.Defense);
                defenseAttr?.ModifyMaxValueByAmount(item.bonusDefense * modifierMultiplier);
            }

            if (item.bonusMaxStamina != 0)
            {
                var staminaAttr = attributeManager.GetAttribute(AttributeType.Stamina);
                staminaAttr?.ModifyMaxValueByAmount(item.bonusMaxStamina * modifierMultiplier);
            }

            if (item.bonusDamage != 0)
            {
                var attackPowerAttr = attributeManager.GetAttribute(AttributeType.AttackPower);
                attackPowerAttr?.ModifyMaxValueByAmount(item.bonusDamage * modifierMultiplier);
            }

            if (item.bonusCriticalRate != 0)
            {
                var critRateAttr = attributeManager.GetAttribute(AttributeType.CriticalRate);
                critRateAttr?.ModifyMaxValueByAmount(item.bonusCriticalRate * modifierMultiplier);
            }

            if (item.bonusCriticalDamage != 0)
            {
                var critDamageAttr = attributeManager.GetAttribute(AttributeType.CriticalDamage);
                critDamageAttr?.ModifyMaxValueByAmount(item.bonusCriticalDamage * modifierMultiplier);
            }

            if (item.bonusMoveSpeed != 0)
            {
                var moveSpeedAttr = attributeManager.GetAttribute(AttributeType.MoveSpeed);
                moveSpeedAttr?.ModifyMaxValueByAmount(item.bonusMoveSpeed * modifierMultiplier);
            }

            if (item.bonusAttackRange != 0)
            {
                var attackRangeAttr = attributeManager.GetAttribute(AttributeType.AttackRange);
                attackRangeAttr?.ModifyMaxValueByAmount(item.bonusAttackRange * modifierMultiplier);
            }
        }

        private void SpawnEquipmentVisual(EquipmentItemSO item)
        {
            if (item.equipmentPrefab == null) return;

            Transform slotTransform = equipmentHolder.GetSlotTransform(item.slotType);
            if (slotTransform == null)
            {
                Debug.LogWarning($"[EquipmentManager]: Không tìm thấy slot xương cho loại: {item.slotType}");
                return;
            }

            GameObject spawnedObj = Instantiate(item.equipmentPrefab, slotTransform);
            spawnedObj.transform.localPosition = Vector3.zero;
            spawnedObj.transform.localRotation = Quaternion.identity;

            spawnedEquipmentVisuals.Add(item.slotType, spawnedObj);
        }

        private void DestroyEquipmentVisual(EquipmentSlot slot)
        {
            if (spawnedEquipmentVisuals.TryGetValue(slot, out GameObject obj))
            {
                Destroy(obj);
                spawnedEquipmentVisuals.Remove(slot);
            }
        }

        public GameObject CurrentWeaponVisualObject
        {
            get
            {
                if (spawnedEquipmentVisuals != null &&
                    spawnedEquipmentVisuals.TryGetValue(EquipmentSlot.Weapon, out GameObject weaponVisual))
                {
                    return weaponVisual;
                }

                return null;
            }
        }
    }
}