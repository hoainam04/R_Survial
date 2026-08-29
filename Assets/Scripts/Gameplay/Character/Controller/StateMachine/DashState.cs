using PROJ.Attributes;

public class DashState : ICharacterState
{
    private readonly CharacterControllerBrain brain;
    private bool isDashFinished;

    public DashState(CharacterControllerBrain brain) => this.brain = brain;

    public void Enter()
    {
        isDashFinished = false;
        brain.Animator.CrossFadeInFixedTime(brain.animationData.DashAnimationName, 0.001f);
        brain.Dash.StartDash(brain.InputDirection, () => isDashFinished = true);
    }

    public void UpdateState()
    {
        if (isDashFinished)
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

    public void FixedUpdateState() { }
    public void Exit() { }
}
