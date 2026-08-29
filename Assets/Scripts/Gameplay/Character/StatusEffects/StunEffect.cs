namespace PROJ.Attributes
{
    public class StunEffect : StatusEffect
    {
        private readonly CharacterEffectController effectController;

        public StunEffect(
            CharacterAttributeManager attributeManager,
            CharacterEffectController effectController,
            float duration) : base(attributeManager, duration)
        {
            this.effectController = effectController;
        }

        public override void OnApply()
        {
            effectController.SetStunned(true);
        }

        public override void OnRemove()
        {
            effectController.SetStunned(false);
        }
    }
}