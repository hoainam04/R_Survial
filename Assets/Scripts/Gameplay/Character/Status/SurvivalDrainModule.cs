namespace PROJ.Attributes
{
    public class SurvivalDrainModule
    {
        private readonly CharacterAttributeManager attributeManager;
        private readonly float hungerDrainPerSecond;
        private readonly float thirstDrainPerSecond;

        public SurvivalDrainModule(
            CharacterAttributeManager attributeManager,
            float hungerDrainPerSecond,
            float thirstDrainPerSecond)
        {
            this.attributeManager = attributeManager;
            this.hungerDrainPerSecond = hungerDrainPerSecond;
            this.thirstDrainPerSecond = thirstDrainPerSecond;
        }

        public void Tick(float deltaTime)
        {
            attributeManager.GetAttribute(AttributeType.Hunger)?.Modify(-hungerDrainPerSecond * deltaTime);
            attributeManager.GetAttribute(AttributeType.Thirst)?.Modify(-thirstDrainPerSecond * deltaTime);
        }
    }
}