using UnityEngine;
using PROJ.Equipment;
using PROJ.Attributes;

# region RANGED ATTACK STATE
public class RangedAttackState : ICharacterState
{
    private CharacterControllerBrain brain;
    private float fireRateTimer;
    private Vector3 lockedAimDirection; // Biến khóa hướng ngắm/bắn

    public RangedAttackState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        // Chơi animation bắn súng tầm xa
        brain.Animator.Play(brain.animationData.GetRangedAttackAnim(WeaponType.Ranged));
        
        // Khởi tạo phát bắn đầu tiên ngay lập tức
        fireRateTimer = 0f; 
        FireWeapon();
    }

    public void UpdateState()
    {
        // Ưu tiên 1: Đang bắn mà bấm Space thì phải cho Dash ngay (Animation Cancel)
        if (brain.GetDashInput() && brain.Dash.CanDash())
        {
            brain.ChangeState(brain.DashState);
            return;
        }

        // Ưu tiên 2: Đang bắn mà chủ động bấm R thì chuyển sang ReloadState
        if (brain.GetReloadInput())
        {
            WeaponRanged activeGun = GetActiveGun();
            if (activeGun != null && activeGun.CurrentAmmo < activeGun.MaxAmmo)
            {
                brain.ChangeState(brain.ReloadState);
                return;
            }
        }

        // Ưu tiên 3: Tự động xả liên thanh khi đè chuột trái
        if (brain.GetAttackInput())
        {
            if (fireRateTimer > 0) fireRateTimer -= Time.deltaTime;

            if (fireRateTimer <= 0f)
            {
                FireWeapon();
            }
        }
        else
        {
            // Buông chuột trái thì trả về trạng thái di chuyển/đứng im bình thường
            ExitToMovementStates();
        }
    }

    private void FireWeapon()
    {
        WeaponRanged activeGun = GetActiveGun();

        // Chốt chặn nếu không có súng hoặc súng không đủ điều kiện bắn
        if (activeGun == null || !activeGun.CanFire()) return;

        // 🌟 KHÓA HƯỚNG BẮN NGAY TẠI THỜI ĐIỂM BÓP CÒ
        lockedAimDirection = brain.GetAimDirection();

        // 1. Thực hiện phát bắn thực tế (Trừ đạn, tạo raycast/bullet) với hướng đã khóa
        brain.Attack.ExecuteAttack(lockedAimDirection);

        // 2. Tính toán thời gian giãn cách giữa các viên đạn (Fire Rate)
        WeaponEquipmentSO weaponData = brain.EquipmentManager.CurrentWeaponData;
        if (weaponData is WeaponRangedSO weaponRanged)
        {
            float baseCooldown = weaponRanged != null ? weaponRanged.fireRate : 0.2f;
            fireRateTimer = baseCooldown;
        }

        // 3. TỰ ĐỘNG THAY ĐẠN KHI HẾT BĂNG: 
        // Nếu viên vừa rồi làm súng cạn sạch đạn, chuyển thẳng sang ReloadState
        if (activeGun.CurrentAmmo <= 0)
        {
            brain.ChangeState(brain.ReloadState);
        }
    }

    /// <summary>
    /// Hàm bổ trợ lấy nhanh Component súng đang cầm trên tay từ EquipmentManager
    /// </summary>
    private WeaponRanged GetActiveGun()
    {
        GameObject gunVisual = brain.EquipmentManager.CurrentWeaponVisualObject;
        if (gunVisual != null)
        {
            return gunVisual.GetComponent<WeaponRanged>();
        }
        return null;
    }

    private void ExitToMovementStates()
    {
        if (brain.InputDirection.magnitude > 0.1f)
        {
            var staminaAttr = brain.AttributeManager.GetAttribute(AttributeType.Stamina);
            if (brain.IsSprintingToggle && staminaAttr != null && staminaAttr.CurrentValue > 0f)
                brain.ChangeState(brain.RunState);
            else
                brain.ChangeState(brain.MoveState);
        }
        else
        {
            brain.ChangeState(brain.IdleState);
        }
    }

    public void FixedUpdateState()
    {
        // 🌟 CỐ ĐỊNH HƯỚNG XOAY NHÂN VẬT THEO HƯỚNG BẮN ĐÃ KHÓA TRONG SUỐT QUÁ TRÌNH XẢ SÚNG
        if (lockedAimDirection != Vector3.zero)
        {
            brain.RotateTowards(lockedAimDirection);
        }

        // Vừa đè chuột bắn vừa di chuyển chậm (Tốc độ Walk thông thường)
        brain.Movement.SetMoveDirection(brain.InputDirection);
        if (brain.InputDirection.magnitude > 0.1f)
        {
            brain.Movement.Move();
        }
        else
        {
            brain.Movement.Stop();
        }
    }

    public void Exit() { }
}
#endregion
