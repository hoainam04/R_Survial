// using UnityEngine;

// namespace PROJ.Combat
// {
//     public class EnemyHealth : LivingEntityHealth
//     {
//         [Header("Loot")]
//         [SerializeField] private GameObject lootPrefab;

//         protected override void OnHit(float damage, bool isCritical)
//         {
//             // Debug.Log($"Quái {gameObject.name} trúng đòn: -{damage} HP"+$"{(isCritical ? "(CHÍ MẠNG!)" : "")}");
            
//             // Hiện số nhảy sát thương (Floating Damage Text) tại vị trí con quái
//             // DamageTextManager.Instance.Spawn(transform.position, damage, isCritical);
//         }

//         protected override void Die()
//         {
//             if (IsDead) return;
//             base.Die();

//             Debug.Log($"Quái {gameObject.name} chết, rớt đồ!");

//             // Sinh ra vật phẩm rơi rớt (Loot Drop)
//             if (lootPrefab != null)
//             {
//                 Instantiate(lootPrefab, transform.position, Quaternion.identity);
//             }

//             // Xóa xác quái sau 3 giây
//             Destroy(gameObject, 3f);
//         }
//     }
// }