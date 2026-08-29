using PROJ.Attributes;
using PROJ.Equipment;
using UnityEngine;

# region MELEE ATTACK STATE
public class MeleeAttackState : ICharacterState
{
    private CharacterControllerBrain brain;

    private float stateTimer;
    private float hitboxDelayTimer;
    private bool hasTriggeredHitbox;
    private Vector3 lockedAttackDirection; // Biến khóa hướng tấn công tại thời điểm ra đòn

    public MeleeAttackState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        hasTriggeredHitbox = false;
        ExecuteMeleeStrike();
    }

    public void UpdateState()
    {
        if (brain.GetDashInput() && brain.Dash.CanDash())
        {
            brain.ChangeState(brain.DashState);
            return;
        }

        if (stateTimer > 0) stateTimer -= Time.deltaTime;

        // Bật quét hitbox dựa theo cử động vung kiếm vật lý
        if (!hasTriggeredHitbox && hitboxDelayTimer > 0)
        {
            hitboxDelayTimer -= Time.deltaTime;
            if (hitboxDelayTimer <= 0f)
            {
                brain.Attack.SetMeleeHitboxActive(true);
                hasTriggeredHitbox = true;
            }
        }

        // Khi kết thúc thời gian khựng của đòn hiện tại
        if (stateTimer <= 0)
        {
            // Nếu người chơi vẫn đè chuột -> Chém tiếp đòn tiếp theo (Auto Attack / Combo)
            if (brain.GetAttackInput() && brain.Attack.CanAttack())
            {
                hasTriggeredHitbox = false;
                ExecuteMeleeStrike();
                return;
            }

            ExitToMovementStates();
        }
    }

    private void ExecuteMeleeStrike()
    {
        WeaponEquipmentSO weaponData = brain.EquipmentManager.CurrentWeaponData;

        float baseCooldown = 0.5f;   // Chỉ số mặc định khi đấm tay không
        float baseHitboxDelay = 0.15f;

        if (weaponData is WeaponMeleeSO meleeData)
        {
            baseCooldown = meleeData.attackCooldown;
            baseHitboxDelay = meleeData.hitboxDelay;
        }

        stateTimer = baseCooldown;
        hitboxDelayTimer = baseHitboxDelay;

        // 🌟 KHÓA HƯỚNG TẤN CÔNG NGAY TẠI KHOẢNH KHẮC RA ĐÒN
        lockedAttackDirection = brain.GetAimDirection();
        
        // Phát lệnh xử lý logic tính toán sát thương với hướng đã khóa
        brain.Attack.ExecuteAttack(lockedAttackDirection);

        // Xử lý Animation dựa vào MeleeType
        if (weaponData is WeaponMeleeSO weaponMeleeSO)
        {
            var animClip = brain.animationData.GetMeleeAttackAnim(weaponMeleeSO.meleeType);
            brain.Animator.CrossFadeInFixedTime(animClip, 0.1f);
        }
        else
        {
            brain.Animator.CrossFadeInFixedTime(brain.animationData.MeleeAnimationName, 0.1f);
        }
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
        // 🌟 CỐ ĐỊNH HƯỚNG XOAY NHÂN VẬT THEO HƯỚNG ĐÃ KHÓA TRONG SUỐT ĐÒN ĐÁNH
        if (lockedAttackDirection != Vector3.zero)
        {
            brain.RotateTowards(lockedAttackDirection);
        }

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

    public void Exit()
    {
        brain.Attack.SetMeleeHitboxActive(false);
    }
}
#endregion
