namespace PROJ.Attributes
{
    public class HealthRegenModule
    {
        private readonly CharacterAttributeManager attributeManager;
        private readonly float regenInterval;

        private float timer;

        public HealthRegenModule(CharacterAttributeManager attributeManager, float regenInterval)
        {
            this.attributeManager = attributeManager;
            this.regenInterval = regenInterval;
        }

        public void Tick(float deltaTime)
        {
            timer += deltaTime;

            if (timer < regenInterval)
                return;

            timer = 0f;

            var hpRegen = attributeManager.GetAttribute(AttributeType.HPRegen);
            var health = attributeManager.GetAttribute(AttributeType.Health);

            if (hpRegen == null || health == null)
                return;

            if (health.CurrentValue <= 0)
                return;

            health.Modify(hpRegen.CurrentValue);
        }
    }
}