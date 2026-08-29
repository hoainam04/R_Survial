// using UnityEngine;
// using PROJ.Attributes;

// [RequireComponent(typeof(Rigidbody))]
// public class CharacterJump : MonoBehaviour
// {
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private LayerMask groundLayer;

//     private Rigidbody rb;
//     private CharacterAttributeManager attributeManager;

//     private float groundCheckCooldown = 0.15f;
//     private float nextGroundCheckTime;
//     [SerializeField] private float staminaCost = 10f;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         attributeManager = GetComponent<CharacterAttributeManager>();
//     }

//     public bool IsGrounded()
//     {
//         if (Time.time < nextGroundCheckTime) return false;
//         return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
//     }

//     // --- THÊM MỚI: Hàm check điều kiện tiên quyết trước khi đổi State ---
//     public bool CanJump()
//     {
//         // 1. Phải chạm đất
//         if (!IsGrounded()) return false;

//         // 2. Kiểm tra an toàn hệ thống thuộc tính
//         if (attributeManager == null) return false;

//         var jumpForceAttr = attributeManager.GetAttribute(AttributeType.JumpForce);
//         var staminaAttr = attributeManager.GetAttribute(AttributeType.Stamina);

//         if (jumpForceAttr == null || staminaAttr == null) return false;

//         // 3. Phải đủ Stamina
//         if (staminaAttr.CurrentValue < staminaCost)
//         {
//             Debug.LogWarning("[CharacterJump]: Không đủ stamina để nhảy!");
//             return false;
//         }

//         return true;
//     }

//     public void Jump()
//     {
//         // Lúc này hàm Jump chỉ thực thi hành động vật lý thuần túy vì điều kiện đã check ở ngoài
//         if (attributeManager == null) return;

//         var jumpForceAttr = attributeManager.GetAttribute(AttributeType.JumpForce);
//         if (jumpForceAttr == null) return;

//         if (!CanJump())
//         {
//             Debug.LogWarning("[CharacterJump]: Điều kiện nhảy không thỏa mãn (Có thể do không chạm đất hoặc thiếu Stamina)!");
//             return; // Không thực hiện nhảy nếu điều kiện
//         }

//         // Thiết lập mốc thời gian khóa ground check
//         nextGroundCheckTime = Time.time + groundCheckCooldown;

//         // Tiêu hao stamina
//         attributeManager.ConsumeStamina(staminaCost);

//         // Áp dụng lực nhảy vật lý
//         rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
//         rb.AddForce(Vector3.up * jumpForceAttr.CurrentValue, ForceMode.Impulse);
//     }
// }