using PROJ.Attributes;
using PROJ.Equipment;

public class RunState : ICharacterState
{
    private readonly CharacterControllerBrain brain;

    public RunState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        brain.IsSprintingToggle = true;
        WeaponType currentWeapon = brain.EquipmentManager.CurrentWeaponType;
        brain.Animator.CrossFadeInFixedTime(brain.animationData.GetRunAnim(currentWeapon), 0.1f);
    }

    public void UpdateState()
    {
        if (brain.InputDirection.magnitude < 0.1f)
        {
            brain.ChangeState(brain.IdleState);
            return;
        }

        if (brain.GetSprintInput())
        {
            brain.IsSprintingToggle = false;
            brain.ChangeState(brain.MoveState);
            return;
        }

        var staminaAttr = brain.AttributeManager.GetAttribute(AttributeType.Stamina);
        if (staminaAttr != null && staminaAttr.CurrentValue <= 0f)
        {
            brain.IsSprintingToggle = false;
            brain.ChangeState(brain.MoveState);
            return;
        }
    }

    public void FixedUpdateState()
    {
        brain.Movement.SetMoveDirection(brain.InputDirection);
        brain.Movement.Run();
    }

    public void Exit() { }
}
