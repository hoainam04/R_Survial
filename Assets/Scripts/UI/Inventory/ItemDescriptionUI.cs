using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PROJ.Item;
using PROJ.Equipment;
using PROJ.Attributes;

namespace PROJ.UI
{
    public class ItemDescriptionUI : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [Header("UI Elements")]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI quantityText;
        [Header("Buttons")]
        [SerializeField] private Button equip_UseButton;
        [SerializeField] private Button dropButton;

        [Header("ItemAttribute Display (Optional)")]
        [SerializeField] private GameObject attributePanelRoot;
        [SerializeField] private VerticalScrollList attributeList;
        [SerializeField] private ItemAttributeUI attributePrefab;

        private Inventory inventory;
        private CharacterEquipmentManager equipmentManager;
        private CharacterAttributeManager attributeManager;
        private ItemSO selectedItem;

        private CanvasGroup selfCanvasGroup;
        private bool panelRootIsSelf;

        private void Awake()
        {
            inventory = FindFirstObjectByType<Inventory>();
            equipmentManager = FindFirstObjectByType<CharacterEquipmentManager>();
            attributeManager = FindFirstObjectByType<CharacterAttributeManager>();

            if (equip_UseButton != null) equip_UseButton.onClick.AddListener(HandleEquipOrUse);
            if (dropButton != null) dropButton.onClick.AddListener(HandleDrop);

            panelRootIsSelf = panelRoot == gameObject;
            if (panelRootIsSelf)
            {
                selfCanvasGroup = GetComponent<CanvasGroup>();
                if (selfCanvasGroup == null)
                {
                    selfCanvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            Hide();
        }

        private void OnEnable()
        {
            ItemSelectionEvents.OnItemSelected += Show;

            if (inventory != null) inventory.OnInventoryChanged += RefreshButtons;
            if (equipmentManager != null) equipmentManager.OnEquipmentChanged += RefreshButtons;
            if (equipmentManager != null) equipmentManager.OnWeaponLoadoutChanged += RefreshButtons;
        }

        private void OnDisable()
        {
            ItemSelectionEvents.OnItemSelected -= Show;

            if (inventory != null) inventory.OnInventoryChanged -= RefreshButtons;
            if (equipmentManager != null) equipmentManager.OnEquipmentChanged -= RefreshButtons;
            if (equipmentManager != null) equipmentManager.OnWeaponLoadoutChanged -= RefreshButtons;
        }

        private void OnDestroy()
        {
            if (equip_UseButton != null) equip_UseButton.onClick.RemoveListener(HandleEquipOrUse);
            if (dropButton != null) dropButton.onClick.RemoveListener(HandleDrop);
        }

        private void Show(ItemSO item)
        {
            selectedItem = item;

            if (item == null)
            {
                Hide();
                return;
            }

            SetVisibility(true);

            if (icon != null)
            {
                icon.enabled = true;
                icon.sprite = item.itemIcon;
            }

            if (nameText != null) nameText.text = item.itemName;
            if (descriptionText != null) descriptionText.text = item.description;
            if (rarityText != null) rarityText.text = item.rarity.ToString();
            if (weightText != null) weightText.text = $"{item.weight} kg";
            if (quantityText != null) quantityText.text = $"{inventory.GetItemCount(item)}";
            
            ShowAttributePanel(item);
            RefreshButtons();
        }
        private void ShowAttributePanel(ItemSO item)
        {
            if (attributePanelRoot == null) return;

            if (item is IAttributeDisplayable displayableItem)
            {
                attributePanelRoot.SetActive(true);
                attributeList.Clear();

                string summary = displayableItem.GetAttributeSummary();
                if (string.IsNullOrEmpty(summary)) return;

                string[] lines = summary.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length >= 2)
                    {
                        // var attributeUI = Instantiate(attributePrefab, attributeList.transform);
                        ItemAttributeUI attribute = attributeList.SpawnItem(attributePrefab);
                        attribute.SetData(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
            else
            {
                attributePanelRoot.SetActive(false);
            }
        }

        private void Hide()
        {
            selectedItem = null;
            SetVisibility(false);
        }

        private void SetVisibility(bool visible)
        {
            if (panelRootIsSelf)
            {
                selfCanvasGroup.alpha = visible ? 1f : 0f;
                selfCanvasGroup.interactable = visible;
                selfCanvasGroup.blocksRaycasts = visible;
                return;
            }

            if (panelRoot != null)
            {
                panelRoot.SetActive(visible);
            }
        }

        private void RefreshButtons()
        {
            if (equip_UseButton != null)
            {
                bool isConsumable = selectedItem is ConsumableSO;
                bool isEquipment = selectedItem is EquipmentItemSO;
                bool isEquipped = isEquipment && equipmentManager != null && equipmentManager.IsItemEquipped(selectedItem);

                equip_UseButton.interactable = isConsumable || isEquipment;
                SetButtonText(isEquipped ? "Unequip" : isConsumable ? "Use" : "Equip");
            }

            if (dropButton != null)
            {
                dropButton.interactable = inventory != null && selectedItem != null && inventory.HasItem(selectedItem, 1);
                dropButton.GetComponentInChildren<TextMeshProUGUI>().text = inventory != null && selectedItem != null && inventory.HasItem(selectedItem, 1) ? "Drop" : "Cannot Drop";
            }
        }

        private void SetButtonText(string text)
        {
            var label = equip_UseButton.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = text;
        }

        private void HandleEquipOrUse()
        {
            if (selectedItem == null || inventory == null) return;

            if (selectedItem is ConsumableSO consumable)
            {
                if (!inventory.HasItem(selectedItem, 1)) return;
                consumable.UseConsumable(attributeManager);
                inventory.RemoveItem(selectedItem, 1);
                RefreshButtons();
                return;
            }

            if (selectedItem is not EquipmentItemSO equipmentItem || equipmentManager == null) return;

            if (equipmentManager.IsItemEquipped(selectedItem))
            {
                equipmentManager.TryUnequipItem(selectedItem);
            }
            else
            {
                equipmentManager.TryEquipFromInventory(equipmentItem);
            }

            RefreshButtons();
        }

        private void HandleDrop()
        {
            if (selectedItem == null || inventory == null || !inventory.HasItem(selectedItem, 1)) return;

            // if (inventory.DropItem(selectedItem, 1))
            // {
            //     ItemSelectionEvents.ClearSelection();
            // }
            if (inventory.DropAllItem(selectedItem))
            {
                ItemSelectionEvents.ClearSelection();
            }
        }
    }
}
