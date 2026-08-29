using UnityEngine;

namespace PROJ.Attributes
{
    public class CharacterStaminaRegen : MonoBehaviour
    {
        [Header("Regen Settings")]
        [SerializeField] private float staminaRegenRate = 10f; // Lượng hồi phục mỗi giây
        [SerializeField] private float regenDelay = 1.5f;     // Thời gian chờ sau hành động cuối cùng mới bắt đầu hồi

        private CharacterAttributeManager attributeManager;
        private CharacterControllerBrain brain;
        private float nextRegenTime;

        private void Awake()
        {
            attributeManager = GetComponent<CharacterAttributeManager>();
            brain = GetComponent<CharacterControllerBrain>();
        }

        private void Update()
        {
            if (brain == null || attributeManager == null) return;

            // 1. Kiểm tra xem nhân vật có đang ở trạng thái tiêu hao Stamina không
            // Sử dụng tính năng kiểm tra type của State Machine cực kỳ sạch sẽ
            bool isExpendingStamina = brain.GetCurrentState() is RunState || brain.GetCurrentState() is DashState;

            if (isExpendingStamina)
            {
                // Nếu đang chạy hoặc lướt, liên tục đẩy mốc thời gian hồi phục về tương lai
                nextRegenTime = Time.time + regenDelay;
                return;
            }

            // 2. Nếu đã hết thời gian delay và Stamina chưa đầy thì tiến hành hồi
            if (Time.time >= nextRegenTime)
            {
                var staminaAttr = attributeManager.GetAttribute(AttributeType.Stamina);
                if (staminaAttr != null && staminaAttr.CurrentValue < staminaAttr.MaxValue)
                {
                    // Hồi stamina mượt mà theo thời gian thực (Time.deltaTime)
                    attributeManager.RegenerateStamina(staminaRegenRate * Time.deltaTime);
                }
            }
        }
    }
}