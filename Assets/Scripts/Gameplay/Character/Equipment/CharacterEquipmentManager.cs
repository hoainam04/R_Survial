using System;
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
        public const int WeaponLoadoutSlotCount = 2; // Wep1 (Primary) / Wep2 (Secondary)

        private CharacterAttributeManager attributeManager;
        private EquipmentHolder equipmentHolder;
        private Inventory inventory;

        private Dictionary<EquipmentSlot, EquipmentItemSO> currentEquipmentDict;
        private Dictionary<EquipmentSlot, GameObject> spawnedEquipmentVisuals;
        private Dictionary<EquipmentSlot, bool> equippedFromInventory;
        private readonly WeaponEquipmentSO[] weaponLoadout = new WeaponEquipmentSO[WeaponLoadoutSlotCount];
        private readonly bool[] weaponLoadoutFromInventory = new bool[WeaponLoadoutSlotCount];

        public WeaponType CurrentWeaponType { get; private set; } = WeaponType.Melee;
        public WeaponEquipmentSO CurrentWeaponData { get; private set; }
        public int ActiveWeaponSlotIndex { get; private set; } = -1;

        [Header("Cấu Hình Mặc Định Khi Không Cầm Vũ Khí")]
        [SerializeField] private WeaponEquipmentSO defaultBareFistsAsset;

        // UI lắng nghe để cập nhật hiển thị, tầng Gameplay không được biết đến UI
        public event Action OnEquipmentChanged;
        public event Action OnWeaponLoadoutChanged;

        private void Awake()
        {
            attributeManager = GetComponent<CharacterAttributeManager>();
            equipmentHolder = GetComponent<EquipmentHolder>();
            inventory = GetComponent<Inventory>();

            currentEquipmentDict = new Dictionary<EquipmentSlot, EquipmentItemSO>();
            spawnedEquipmentVisuals = new Dictionary<EquipmentSlot, GameObject>();
            equippedFromInventory = new Dictionary<EquipmentSlot, bool>();

            CurrentWeaponData = defaultBareFistsAsset;
        }

        private void Update()
        {
            if (GameSettingsManager.Instance == null) return;

            if (GameSettingsManager.Instance.GetKeyDown("WeaponSlot1")) SwitchWeapon(0);
            if (GameSettingsManager.Instance.GetKeyDown("WeaponSlot2")) SwitchWeapon(1);
        }

        public EquipmentItemSO GetEquippedItem(EquipmentSlot slot)
        {
            return currentEquipmentDict.TryGetValue(slot, out var item) ? item : null;
        }

        public bool IsItemEquipped(ItemSO item)
        {
            if (item == null) return false;

            for (int i = 0; i < weaponLoadout.Length; i++)
            {
                if (weaponLoadout[i] == item) return true;
            }

            foreach (var equippedItem in currentEquipmentDict.Values)
            {
                if (equippedItem == item) return true;
            }

            return false;
        }

        public bool TryEquipFromInventory(EquipmentItemSO item)
        {
            if (item == null || IsItemEquipped(item) || !inventory.HasItem(item, 1)) return false;

            if (item is WeaponEquipmentSO weapon)
            {
                for (int i = 0; i < weaponLoadout.Length; i++)
                {
                    if (weaponLoadout[i] == null)
                    {
                        AssignWeaponToLoadout(i, weapon);
                        return true;
                    }
                }

                return false;
            }

            EquipItem(item);
            return true;
        }

        public bool TryUnequipItem(ItemSO item)
        {
            if (item == null) return false;

            for (int i = 0; i < weaponLoadout.Length; i++)
            {
                if (weaponLoadout[i] == item)
                {
                    UnequipWeaponFromLoadout(i);
                    return true;
                }
            }

            if (item is EquipmentItemSO equipmentItem &&
                currentEquipmentDict.TryGetValue(equipmentItem.slotType, out var equippedItem) &&
                equippedItem == item)
            {
                UnequipItem(equipmentItem.slotType);
                return true;
            }

            return false;
        }

        public WeaponEquipmentSO GetWeaponInLoadoutSlot(int loadoutIndex)
        {
            if (loadoutIndex < 0 || loadoutIndex >= weaponLoadout.Length) return null;
            return weaponLoadout[loadoutIndex];
        }

        public void SwapWeaponLoadoutSlots(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex >= weaponLoadout.Length) return;
            if (secondIndex < 0 || secondIndex >= weaponLoadout.Length) return;
            if (firstIndex == secondIndex) return;

            WeaponEquipmentSO firstWeapon = weaponLoadout[firstIndex];
            weaponLoadout[firstIndex] = weaponLoadout[secondIndex];
            weaponLoadout[secondIndex] = firstWeapon;

            bool firstOwnedByInventory = weaponLoadoutFromInventory[firstIndex];
            weaponLoadoutFromInventory[firstIndex] = weaponLoadoutFromInventory[secondIndex];
            weaponLoadoutFromInventory[secondIndex] = firstOwnedByInventory;

            if (ActiveWeaponSlotIndex == firstIndex || ActiveWeaponSlotIndex == secondIndex)
            {
                SwitchWeapon(ActiveWeaponSlotIndex);
            }
            else
            {
                OnWeaponLoadoutChanged?.Invoke();
            }
        }

        public void AssignWeaponToLoadout(int loadoutIndex, WeaponEquipmentSO weapon)
        {
            if (loadoutIndex < 0 || loadoutIndex >= weaponLoadout.Length) return;

            int otherSlotIndex = loadoutIndex == 0 ? 1 : 0;
            if (weapon != null && weaponLoadout[otherSlotIndex] == weapon)
            {
                Debug.LogWarning($"[EquipmentManager]: {weapon.itemName} đã nằm ở ô vũ khí còn lại.");
                return;
            }

            if (weaponLoadout[loadoutIndex] == weapon) return;

            if (weaponLoadout[loadoutIndex] != null && weaponLoadoutFromInventory[loadoutIndex])
            {
                inventory.AddItem(weaponLoadout[loadoutIndex]);
            }

            weaponLoadout[loadoutIndex] = weapon;
            weaponLoadoutFromInventory[loadoutIndex] = weapon != null && inventory.RemoveItem(weapon, 1);

            // Tự động chuyển sang vũ khí vừa gán nếu đây là ô đang active hoặc chưa có vũ khí nào active
            if (ActiveWeaponSlotIndex == loadoutIndex || ActiveWeaponSlotIndex < 0)
            {
                SwitchWeapon(loadoutIndex);
            }
            else
            {
                OnWeaponLoadoutChanged?.Invoke();
            }
        }

        public void SwitchWeapon(int loadoutIndex)
        {
            if (loadoutIndex < 0 || loadoutIndex >= weaponLoadout.Length) return;

            WeaponEquipmentSO weapon = weaponLoadout[loadoutIndex];

            if (currentEquipmentDict.ContainsKey(EquipmentSlot.Weapon))
            {
                UnequipCurrentWeapon();
            }

            if (weapon != null)
            {
                EquipItemInternal(weapon, weaponLoadoutFromInventory[loadoutIndex]);
            }

            ActiveWeaponSlotIndex = loadoutIndex;
            OnWeaponLoadoutChanged?.Invoke();
        }

        public void EquipItem(EquipmentItemSO newItem)
        {
            if (newItem == null) return;

            bool removedFromInventory = inventory.HasItem(newItem, 1) && inventory.RemoveItem(newItem, 1);

            if (currentEquipmentDict.ContainsKey(newItem.slotType))
            {
                UnequipItem(newItem.slotType);

                if (currentEquipmentDict.ContainsKey(newItem.slotType))
                {
                    if (removedFromInventory) inventory.AddItem(newItem);
                    return;
                }
            }

            EquipItemInternal(newItem, removedFromInventory);
        }

        private void EquipItemInternal(EquipmentItemSO newItem, bool fromInventory)
        {
            if (newItem == null) return;

            currentEquipmentDict.Add(newItem.slotType, newItem);
            equippedFromInventory[newItem.slotType] = fromInventory;

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
            OnEquipmentChanged?.Invoke();
        }

        public void UnequipItem(EquipmentSlot slot)
        {
            if (!currentEquipmentDict.TryGetValue(slot, out var oldItem)) return;

            if (slot == EquipmentSlot.Weapon && oldItem is WeaponEquipmentSO weapon)
            {
                for (int i = 0; i < weaponLoadout.Length; i++)
                {
                    if (weaponLoadout[i] == weapon)
                    {
                        UnequipWeaponFromLoadout(i);
                        return;
                    }
                }
            }

            bool shouldReturnToInventory = equippedFromInventory.TryGetValue(slot, out bool wasFromInventory) && wasFromInventory;
            if (shouldReturnToInventory && !inventory.CanAddItem(oldItem))
            {
                Debug.LogWarning("[EquipmentManager]: Không thể tháo trang bị vì Inventory đã đầy.");
                return;
            }

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
            equippedFromInventory.Remove(slot);

            if (slot == EquipmentSlot.Weapon)
            {
                CurrentWeaponData = defaultBareFistsAsset;
                CurrentWeaponType = defaultBareFistsAsset != null ? defaultBareFistsAsset.weaponType : WeaponType.Melee;

                Debug.Log("[EquipmentManager]: Tháo vũ khí, trả tư thế về mặc định.");
            }

            Debug.Log($"[EquipmentManager]: Đã tháo trang bị ở slot: {slot}");
            if (shouldReturnToInventory)
            {
                inventory.AddItem(oldItem);
            }
            OnEquipmentChanged?.Invoke();
        }

        public void UnequipWeaponFromLoadout(int loadoutIndex)
        {
            if (loadoutIndex < 0 || loadoutIndex >= weaponLoadout.Length) return;

            WeaponEquipmentSO weapon = weaponLoadout[loadoutIndex];
            if (weapon == null) return;

            if (weaponLoadoutFromInventory[loadoutIndex] && !inventory.CanAddItem(weapon))
            {
                Debug.LogWarning("[EquipmentManager]: Không thể tháo vũ khí vì Inventory đã đầy.");
                return;
            }

            bool wasActive = ActiveWeaponSlotIndex == loadoutIndex;
            weaponLoadout[loadoutIndex] = null;

            if (weaponLoadoutFromInventory[loadoutIndex])
            {
                inventory.AddItem(weapon);
            }

            weaponLoadoutFromInventory[loadoutIndex] = false;

            if (wasActive)
            {
                UnequipCurrentWeapon();
                ActiveWeaponSlotIndex = -1;
            }

            OnWeaponLoadoutChanged?.Invoke();
        }

        private void UnequipCurrentWeapon()
        {
            if (!currentEquipmentDict.ContainsKey(EquipmentSlot.Weapon)) return;

            EquipmentItemSO oldWeapon = currentEquipmentDict[EquipmentSlot.Weapon];
            ApplyEquipmentStats(oldWeapon, false);
            DestroyEquipmentVisual(EquipmentSlot.Weapon);
            currentEquipmentDict.Remove(EquipmentSlot.Weapon);
            equippedFromInventory.Remove(EquipmentSlot.Weapon);
            CurrentWeaponData = defaultBareFistsAsset;
            CurrentWeaponType = defaultBareFistsAsset != null ? defaultBareFistsAsset.weaponType : WeaponType.Melee;
            OnEquipmentChanged?.Invoke();
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
            if (weaponItem.critChance != 0)
            {
                var critChanceAttr = attributeManager.GetAttribute(AttributeType.CriticalRate);
                critChanceAttr?.ModifyMaxValueByAmount(weaponItem.critChance * modifierMultiplier);
            }

            if (weaponItem.critDamageMultiplier != 0)
            {
                var critDamageAttr = attributeManager.GetAttribute(AttributeType.CriticalDamage);
                critDamageAttr?.ModifyMaxValueByAmount(weaponItem.critDamageMultiplier * modifierMultiplier);
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