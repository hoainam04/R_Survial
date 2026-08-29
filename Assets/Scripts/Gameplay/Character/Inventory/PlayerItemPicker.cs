using UnityEngine;

namespace PROJ.Item
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerItemPicker : MonoBehaviour
    {
        [Header("Pickup Settings")]
        [SerializeField] private Vector3 pickupCenter = new Vector3(0f, 1f, 0f);
        [SerializeField] private float pickupRange = 2f;
        [SerializeField] private LayerMask itemLayer;
        [SerializeField] private KeyCode pickupKey = KeyCode.F;
        [SerializeField] private float checkInterval = 0.2f; // Thời gian ngắt quãng quét tìm kiếm item

        private Inventory inventory;
        private float checkTimer;
        private WorldItem cachedNearestItem;

        private Vector3 PickupCenter => transform.TransformPoint(pickupCenter);

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            // Tối ưu CPU: Thay vì gọi OverlapSphere mỗi frame, ta chỉ quét định kỳ mỗi checkInterval (0.2s)
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkInterval)
            {
                checkTimer = 0f;
                FindNearestItem();
            }

            bool isPickupPressed = GameSettingsManager.Instance != null
                ? GameSettingsManager.Instance.GetKeyDown("Interact")
                : Input.GetKeyDown(pickupKey);

            if (isPickupPressed && cachedNearestItem != null)
            {
                TryPickupItem(cachedNearestItem);
            }
        }

        private void FindNearestItem()
        {
            Collider[] hits = Physics.OverlapSphere(
                PickupCenter,
                pickupRange,
                itemLayer
            );

            if (hits.Length == 0)
            {
                cachedNearestItem = null;
                return;
            }

            WorldItem nearestItem = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider hit in hits)
            {
                WorldItem worldItem = hit.GetComponentInParent<WorldItem>();

                if (worldItem == null || !worldItem.CanPickup)
                    continue;

                float distance = Vector3.Distance(
                    PickupCenter,
                    worldItem.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestItem = worldItem;
                }
            }

            cachedNearestItem = nearestItem;
        }

        private void TryPickupItem(WorldItem targetItem)
        {
            if (targetItem == null || !targetItem.CanPickup)
                return;

            bool added = inventory.AddItem(
                targetItem.Item,
                targetItem.Amount
            );

            if (added)
            {
                targetItem.DestroyWorldItem();
                // Debug.Log($"[Pickup] Đã nhặt {targetItem.Item.itemName} x{targetItem.Amount}");
                cachedNearestItem = null;
            }
            else
            {
                // Debug.Log("[Pickup] Inventory đã đầy.");
            }
        }

        private void OnDrawGizmosSelected()
        {
            /*
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(PickupCenter, pickupRange);
            */
        }
    }
}
