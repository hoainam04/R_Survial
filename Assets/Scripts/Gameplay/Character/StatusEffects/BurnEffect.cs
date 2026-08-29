namespace PROJ.Attributes
{
    public class BurnEffect : StatusEffect
    {
        private readonly float damagePerSecond;

        public BurnEffect(
            CharacterAttributeManager attributeManager,
            float duration,
            float damagePerSecond) : base(attributeManager, duration)
        {
            this.damagePerSecond = damagePerSecond;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            AttributeManager.ApplyDamage(damagePerSecond * deltaTime);
        }
    }
}