namespace PROJ.Attributes
{
    public abstract class StatusEffect
    {
        public float Duration { get; protected set; }
        public float RemainingTime { get; protected set; }

        public bool IsFinished => RemainingTime <= 0f;

        protected CharacterAttributeManager AttributeManager;

        public StatusEffect(CharacterAttributeManager attributeManager, float duration)
        {
            AttributeManager = attributeManager;
            Duration = duration;
            RemainingTime = duration;
        }

        public virtual void OnApply()
        {
        }

        public virtual void Tick(float deltaTime)
        {
            RemainingTime -= deltaTime;
        }

        public virtual void OnRemove()
        {
        }
    }
}