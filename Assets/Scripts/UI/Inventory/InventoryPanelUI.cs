using System.Collections.Generic;
using UnityEngine;
using PROJ.Item;

namespace PROJ.UI
{
    public class InventoryPanelUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private InventorySlotUI slotPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject panelRoot;

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
        }
    }
}
