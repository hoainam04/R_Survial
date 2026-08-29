namespace PROJ.Attributes
{
    public class StaminaRegenModule
    {
        private readonly CharacterAttributeManager attributeManager;
        private readonly float regenAmountPerSecond;
        private readonly float regenDelay;

        private float cooldownTimer;

        public StaminaRegenModule(
            CharacterAttributeManager attributeManager,
            float regenAmountPerSecond,
            float regenDelay)
        {
            this.attributeManager = attributeManager;
            this.regenAmountPerSecond = regenAmountPerSecond;
            this.regenDelay = regenDelay;
        }

        public void Tick(float deltaTime)
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= deltaTime;
                return;
            }

            attributeManager.RegenerateStamina(regenAmountPerSecond * deltaTime);
        }

        public void NotifyStaminaConsumed()
        {
            cooldownTimer = regenDelay;
        }
    }
}