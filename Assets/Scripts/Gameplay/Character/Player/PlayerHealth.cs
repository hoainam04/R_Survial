// using UnityEngine;

// namespace PROJ.Combat
// {
//     public class PlayerHealth : LivingEntityHealth
//     {
//         private CharacterControllerBrain brain;

//         protected override void Start()
//         {
//             base.Start(); // Chạy lệnh lấy Máu của lớp cha trước
//             brain = GetComponent<CharacterControllerBrain>();
// }

//         // Viết logic riêng khi PLAYER bị trúng đòn
//         protected override void OnHit(float damage, bool isCritical)
//         {
//             Debug.Log($"<color=cyan>[Player]</color> bị vả dính {damage} sát thương!");

//             // 1. Ép State Machine chuyển sang trạng thái Bị Choáng / Khựng (Hurt State) nếu có
//             // brain.ChangeState(brain.HurtState);

//             // 2. Kích hoạt hiệu ứng màn hình đỏ nhấp nháy trên UI
//             // UIManager.Instance.TriggerBloodScreen();
//         }

//         // Viết logic riêng khi PLAYER bị chết
//         protected override void Die()
//         {
//             if (IsDead) return;
//             base.Die(); // Đánh dấu IsDead = true từ lớp cha

//             Debug.Log("<color=red>[GAME OVER]</color> Player đã oẹo!");

//             // 1. Hiện bảng Menu Die / Hiện nút Respawn
//             // UIManager.Instance.ShowGameOverScreen();

//             // 2. Cho Player nằm xuống hoặc chuyển sang Ragdoll
//         }
//     }
// }