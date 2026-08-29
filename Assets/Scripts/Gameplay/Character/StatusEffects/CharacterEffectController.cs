using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Attributes
{
    public class CharacterEffectController : MonoBehaviour
    {
        [SerializeField] private CharacterAttributeManager attributeManager;

        private readonly List<StatusEffect> activeEffects = new();

        public bool IsStunned { get; private set; }

        private void Awake()
        {
            if (attributeManager == null)
                attributeManager = GetComponent<CharacterAttributeManager>();
        }

        private void Update()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect effect = activeEffects[i];

                effect.Tick(Time.deltaTime);

                if (effect.IsFinished)
                {
                    effect.OnRemove();
                    activeEffects.RemoveAt(i);
                }
            }
        }

        public void ApplyEffect(StatusEffect effect)
        {
            if (effect == null)
                return;

            activeEffects.Add(effect);
            effect.OnApply();
        }

        public void SetStunned(bool value)
        {
            IsStunned = value;
        }
    }
}   