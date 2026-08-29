using PROJ.Attributes;
using PROJ.Equipment;

public class MoveState : ICharacterState
{
    private readonly CharacterControllerBrain brain;

    public MoveState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        WeaponType currentWeapon = brain.EquipmentManager.CurrentWeaponType;
        brain.Animator.CrossFadeInFixedTime(brain.animationData.GetWalkAnim(currentWeapon), 0.1f);
    }

    public void UpdateState()
    {
        if (brain.GetSprintInput())
        {
            var staminaAttr = brain.AttributeManager.GetAttribute(AttributeType.Stamina);
            if (staminaAttr != null && staminaAttr.CurrentValue > 0f)
            {
                brain.IsSprintingToggle = true;
                brain.ChangeState(brain.RunState);
                return;
            }
        }

        if (brain.InputDirection.magnitude < 0.1f)
        {
            brain.ChangeState(brain.IdleState);
            return;
        }
    }

    public void FixedUpdateState()
    {
        brain.Movement.SetMoveDirection(brain.InputDirection);
        brain.Movement.Move();
    }

    public void Exit() { }
}
