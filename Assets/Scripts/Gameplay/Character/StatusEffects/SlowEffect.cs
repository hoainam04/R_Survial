namespace PROJ.Attributes
{
    public class SlowEffect : StatusEffect
    {
        private readonly float slowMultiplier;
        private float originalMoveSpeed;

        public SlowEffect(
            CharacterAttributeManager attributeManager,
            float duration,
            float slowMultiplier) : base(attributeManager, duration)
        {
            this.slowMultiplier = slowMultiplier;
        }

        public override void OnApply()
        {
            var moveSpeed = AttributeManager.GetAttribute(AttributeType.MoveSpeed);

            if (moveSpeed == null)
                return;

            originalMoveSpeed = moveSpeed.CurrentValue;
            moveSpeed.ModifyMaxValue(originalMoveSpeed * slowMultiplier - originalMoveSpeed);
        }

        public override void OnRemove()
        {
            var moveSpeed = AttributeManager.GetAttribute(AttributeType.MoveSpeed);

            if (moveSpeed == null)
                return;

            moveSpeed.ModifyMaxValue(originalMoveSpeed);
        }
    }
}