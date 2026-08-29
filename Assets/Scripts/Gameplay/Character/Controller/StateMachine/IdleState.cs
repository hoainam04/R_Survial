using PROJ.Attributes;
using PROJ.Equipment;

public class IdleState : ICharacterState
{
    private readonly CharacterControllerBrain brain;

    public IdleState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        brain.Movement.Stop();
        WeaponType currentWeapon = brain.EquipmentManager.CurrentWeaponType;
        brain.Animator.CrossFadeInFixedTime(brain.animationData.GetIdleAnim(currentWeapon), 0.1f);
    }

    public void UpdateState()
    {
        if (brain.GetSprintInput())
        {
            brain.IsSprintingToggle = !brain.IsSprintingToggle;
        }

        if (brain.InputDirection.magnitude > 0.1f)
        {
            if (brain.IsSprintingToggle)
            {
                var staminaAttr = brain.AttributeManager.GetAttribute(AttributeType.Stamina);
                if (staminaAttr != null && staminaAttr.CurrentValue > 0f)
                {
                    brain.ChangeState(brain.RunState);
                    return;
                }
            }
            brain.ChangeState(brain.MoveState);
        }
    }

    public void FixedUpdateState() { }
    public void Exit() { }
}
