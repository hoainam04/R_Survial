using System.Collections;
using PROJ.Attributes;
using PROJ.Equipment;
using UnityEngine;

public class ReloadState : ICharacterState
{
    private readonly CharacterControllerBrain brain;
    private WeaponRanged activeGun;
    private Coroutine reloadCoroutine;
    private bool isReloadComplete;

    public ReloadState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        isReloadComplete = false;
        activeGun = null;

        var gunVisual = brain.EquipmentManager.CurrentWeaponVisualObject;
        if (gunVisual != null)
        {
            activeGun = gunVisual.GetComponent<WeaponRanged>();
        }

        if (activeGun == null)
        {
            isReloadComplete = true;
            return;
        }

        brain.Animator.CrossFadeInFixedTime(brain.animationData.GetReloadAnim(), 0.1f);
        reloadCoroutine = brain.StartCoroutine(ReloadProgressRoutine());
    }

    public void UpdateState()
    {
        if (brain.GetDashInput() && brain.Dash.CanDash())
        {
            brain.ChangeState(brain.DashState);
            return;
        }

        if (isReloadComplete)
        {
            ExitToMovementStates();
        }
    }

    private IEnumerator ReloadProgressRoutine()
    {
        activeGun.SetReloadingStatus(true);
        float duration = activeGun.weaponData != null ? activeGun.weaponData.reloadTime : 2f;
        yield return new WaitForSeconds(duration);
        activeGun.RefillAmmo();
        activeGun.SetReloadingStatus(false);
        isReloadComplete = true;
    }

    public void FixedUpdateState()
    {
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
        if (!isReloadComplete)
        {
            if (reloadCoroutine != null)
            {
                brain.StopCoroutine(reloadCoroutine);
            }

            if (activeGun != null)
            {
                activeGun.SetReloadingStatus(false);
            }

            Debug.Log("[ReloadState]: Đã bị hủy ngang tiến trình thay đạn do lướt né!");
        }
    }

    private void ExitToMovementStates()
    {
        if (brain.InputDirection.magnitude > 0.1f)
        {
            var staminaAttr = brain.AttributeManager.GetAttribute(AttributeType.Stamina);
            if (brain.IsSprintingToggle && staminaAttr != null && staminaAttr.CurrentValue > 0f)
            {
                brain.ChangeState(brain.RunState);
            }
            else
            {
                brain.ChangeState(brain.MoveState);
            }
        }
        else
        {
            brain.ChangeState(brain.IdleState);
        }
    }
}
