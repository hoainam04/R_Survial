using System.Collections.Generic;
using UnityEngine;
using PROJ.Item;
using UnityEngine.UI;
using TMPro;

namespace PROJ.UI
{
    public class InventoryPanelUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private InventorySlotUI slotPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject panelRoot;
        [Header("Optional UI Elements")]
        [SerializeField] private TextMeshProUGUI equipmentText;
        [SerializeField] private TextMeshProUGUI backpackText;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button sortBtn;
        [SerializeField] private Button storeAllBtn;
        
        // [SerializeField] private Button closeBtn;

        private readonly List<InventorySlotUI> spawnedSlots = new();

        private void Start()
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<Inventory>();
            }

            if (inventory == null)
            {
                Debug.LogWarning("[InventoryPanelUI]: Không tìm thấy Inventory trong scene.");
                return;
            }

            inventory.OnInventoryChanged += RefreshAll;
            RefreshAll();
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= RefreshAll;
            }
        }

        private void Update()
        {
            if (GameSettingsManager.Instance != null && GameSettingsManager.Instance.GetKeyDown("Inventory"))
            {
                if (panelRoot != null)
                {
                    panelRoot.SetActive(!panelRoot.activeSelf);

                    if (!panelRoot.activeSelf)
                    {
                        ItemSelectionEvents.ClearSelection();
                    }
                }
            }
        }

        private void RefreshAll()
        {
            if (inventory == null || slotPrefab == null || gridParent == null) return;

            int totalSlots = inventory.TotalSlotCount;

            while (spawnedSlots.Count < totalSlots)
            {
                InventorySlotUI newSlot = Instantiate(slotPrefab, gridParent);
                spawnedSlots.Add(newSlot);
            }

            while (spawnedSlots.Count > totalSlots)
            {
                int lastIndex = spawnedSlots.Count - 1;
                Destroy(spawnedSlots[lastIndex].gameObject);
                spawnedSlots.RemoveAt(lastIndex);
            }

            for (int i = 0; i < spawnedSlots.Count; i++)
            {
                spawnedSlots[i].SetData(inventory.GetSlot(i));
            }
            SetUpTextUI();
        }
        private void ArrangeInventory()
        {
            // if (inventory != null)
            // {
            //     inventory.Arrange();
            // }
        }
        private void StoreInventory()
        {
            // if (inventory != null)
            // {
            //     inventory.Store();
            // }
        }
        private void UpdateWeightAndMoney()
        {
            // if (weightText != null)
            // {
            //     weightText.text = $"Weight: {inventory?.CurrentWeight ?? 0}/{inventory?.MaxWeight ?? 0}";
            // }

            // if (moneyText != null)
            // {
            //     moneyText.text = $"Money: {inventory?.CurrentMoney ?? 0}";
            // }
        }
        private void SetUpTextUI()
        {
            equipmentText.text = $"Equipment";
            // weightText.text = $"Weight: {inventory?.CurrentWeight ?? 0}/{inventory?.MaxWeight ?? 0}";
            // moneyText.text = $"Money: {inventory?.CurrentMoney ?? 0}";
            backpackText.text = $"Backpack ({inventory?.EmptySlotCount ?? 0}/{inventory?.TotalSlotCount ?? 0})";
            sortBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Sort";
            storeAllBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Store All";
            weightText.text = $"Weight:??";
            moneyText.text = $"Money:??";

        }
    }
}
