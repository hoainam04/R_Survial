// using UnityEngine;
// using PROJ.Equipment; // Cần dùng WeaponType từ đây

// public static class AnimationData
// {
//     #region KHAI BÁO HASH GỐC (Giữ lại để không lỗi các hệ thống cũ)
//     // Locomotion
//     public static readonly int Idle = Animator.StringToHash("Idle");
//     public static readonly int Walk = Animator.StringToHash("Walk");
//     public static readonly int Run = Animator.StringToHash("Run");
//     public static readonly int Jump = Animator.StringToHash("Jump");
//     public static readonly int Fall = Animator.StringToHash("Fall");
//     public static readonly int Die = Animator.StringToHash("Die");

//     // Dash
//     public static readonly int DashFront = Animator.StringToHash("DashFront");
//     public static readonly int DashBack = Animator.StringToHash("DashBack");
//     public static readonly int DashLeft = Animator.StringToHash("DashLeft");
//     public static readonly int DashRight = Animator.StringToHash("DashRight");

//     // Combat Gốc
//     public static readonly int Attack1H = Animator.StringToHash("Attack(1h)");
//     public static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");
//     public static readonly int Shoot1H = Animator.StringToHash("Shoot(1h)");
//     public static readonly int Shoot2H = Animator.StringToHash("Shoot(2h)");
//     public static readonly int Block = Animator.StringToHash("Block");
//     public static readonly int Shooting1H = Animator.StringToHash("Shooting(1h)");
//     public static readonly int Shooting2H = Animator.StringToHash("Shooting(2h)");

//     // Interaction
//     public static readonly int Interact = Animator.StringToHash("Interact");
//     public static readonly int PickUp = Animator.StringToHash("PickUp");

//     // Misc
//     public static readonly int Hit = Animator.StringToHash("Hit");
//     #endregion

//     #region BỔ SUNG CÁC HASH THEO VŨ KHÍ RIÊNG BIỆT (Ví dụ cho Cung, Súng)
//     // Nếu Animator của ông phân tách rõ tên animation theo vũ khí, khai báo thêm ở đây:
//     public static readonly int BowIdle = Animator.StringToHash("Idle");
//     public static readonly int BowWalk = Animator.StringToHash("Walk");
//     public static readonly int BowRun = Animator.StringToHash("Run");

//     public static readonly int PistolIdle = Animator.StringToHash("Idle");
//     public static readonly int PistolWalk = Animator.StringToHash("Walk");
//     public static readonly int PistolRun = Animator.StringToHash("Run");
//     #endregion

//     #region BỘ PHÂN LOẠI DỰA TRÊN WEAPON TYPE (Các State sẽ gọi cái này)

//     /// <summary>
//     /// Lấy Hash chuyển động đứng yên (Idle) tương ứng với vũ khí
//     /// </summary>
//     public static int GetIdleAnim(WeaponType weaponType)
//     {
//         return weaponType switch
//         {
//             WeaponType.Melee => Idle,       // Tay không hoặc kiếm dùng chung Idle gốc
//             // WeaponType.Bow         => Idle,    // Cầm cung có dáng đứng riêng
//             // WeaponType.Pistol      => Idle, // Cầm súng lục dáng riêng
//             // WeaponType.Ar          => Idle,    // Có thể tận dụng hoặc khai báo thêm hash riêng
//             // WeaponType.HeavyWeapon => Idle,
//             _ => Idle
//         };
//     }

//     /// <summary>
//     /// Lấy Hash chuyển động đi bộ (Walk) tương ứng với vũ khí
//     /// </summary>
//     public static int GetWalkAnim(WeaponType weaponType)
//     {
//         return weaponType switch
//         {
//             WeaponType.Melee => Walk,
//             // WeaponType.Bow    => Walk, // Có thể tận dụng hoặc khai báo thêm hash riêng
//             // WeaponType.Pistol => Walk,
//             _ => Walk
//         };
//     }

//     /// <summary>
//     /// Lấy Hash chuyển động chạy (Run) tương ứng với vũ khí
//     /// </summary>
//     public static int GetRunAnim(WeaponType weaponType)
//     {
//         return weaponType switch
//         {
//             WeaponType.Melee => Run,
//             // WeaponType.Bow    => Run,
//             // WeaponType.Pistol => Run,
//             _ => Run
//         };
//     }

//     /// <summary>
//     /// Lấy Hash đòn tấn công (Attack) tương ứng với vũ khí
//     /// </summary>
//     public static int GetAttackAnim(WeaponType weaponType)
//     {
//         return weaponType switch
//         {
//             WeaponType.Melee => Attack1H,   // Chém kiếm 1 tay
//             // WeaponType.Bow         => Shoot1H,    // Bắn cung
//             // WeaponType.Pistol      => Shooting1H,    // Bắn súng 1 tay
//             WeaponType.Ranged => Shooting2H,    // Bắn súng trường 2 tay
//             // WeaponType.HeavyWeapon => HeavyAttack, // Vung búa/đao nặng
//             _ => Attack1H
//         };
//     }
//     public static int GetMeleeAttackAnim(MeleeType meleeType)
//     {
//         return meleeType switch
//         {
//             MeleeType.Sword => Attack1H,   // Chém kiếm 1 tay
//             MeleeType.Dagger => Attack1H,   // Đâm dao 1 tay
//             MeleeType.HeavyWeapon => HeavyAttack, // Vung búa/đao nặng
//             MeleeType.Stick => Attack1H,   // Đ
//             _ => Attack1H
//         };
//     }

//     #endregion
// }