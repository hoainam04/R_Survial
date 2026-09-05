using UnityEngine;
using UnityEngine.UI;
// using TMPro; // Bỏ comment nếu muốn hiện thêm số lượng

namespace PROJ.Item
{
    public class WorldItem : MonoBehaviour
    {
        [Header("Item Data")]
        [SerializeField] private ItemSO item;
        [SerializeField] private int amount = 1;

        [Header("UI Display")]
        [SerializeField] private Image iconImage;
        // [SerializeField] private TextMeshProUGUI amountText; // Tùy chọn: hiển thị x2, x5...
        [SerializeField] private Transform canvasTransform;

        private Camera mainCamera;

        public ItemSO Item => item;
        public int Amount => amount;
        public bool CanPickup => item != null && amount > 0;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            // Cập nhật hiển thị nếu item được đặt sẵn trong scene qua Inspector
            if (item != null)
            {
                UpdateVisual();
            }
        }

        public void Initialize(ItemSO itemSO, int itemAmount)
        {
            item = itemSO;
            amount = Mathf.Max(1, itemAmount);

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (item == null) return;

            if (iconImage != null)
            {
                iconImage.sprite = item.itemIcon; // Đổi theo tên biến sprite icon trong ItemSO của bạn
                iconImage.enabled = item.itemIcon != null;
            }

            // if (amountText != null)
            // {
            //     amountText.text = amount > 1 ? $"x{amount}" : "";
            // }
        }

        public void DestroyWorldItem()
        {
            Destroy(gameObject);
        }
    }
}