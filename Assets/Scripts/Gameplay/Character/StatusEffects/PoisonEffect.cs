namespace PROJ.Attributes
{
    public class PoisonEffect : StatusEffect
    {
        private readonly float damagePerSecond;

        public PoisonEffect(
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