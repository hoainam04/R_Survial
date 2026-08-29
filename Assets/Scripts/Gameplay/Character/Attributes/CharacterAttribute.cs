using System;
using UnityEngine;

namespace PROJ.Attributes
{
    [Serializable]
    public class CharacterAttribute
    {
        [SerializeField] private AttributeType type;
        [SerializeField] private float maxValue;

        private float currentValue;

        public event Action<float, float> OnValueChanged;

        public AttributeType Type => type;
        public float MaxValue => maxValue;
        public float CurrentValue => currentValue;

        public CharacterAttribute()
        {
        }

        public CharacterAttribute(CharacterAttribute source)
        {
            type = source.type;
            maxValue = source.maxValue;
            currentValue = maxValue;
        }

        public void Initialize()
        {
            currentValue = maxValue;
            OnValueChanged?.Invoke(currentValue, maxValue);
        }

        public void Modify(float amount)
        {
            float oldValue = currentValue;
            currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);

            if (!Mathf.Approximately(oldValue, currentValue))
            {
                OnValueChanged?.Invoke(currentValue, maxValue);
            }
        }

        public void ModifyMaxValue(float newMax)
        {
            maxValue = Mathf.Max(0f, newMax);
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
            OnValueChanged?.Invoke(currentValue, maxValue);
        }

        public void ModifyMaxValueByAmount(float amount)
        {
            maxValue = Mathf.Max(0f, maxValue + amount);
            currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);
            OnValueChanged?.Invoke(currentValue, maxValue);
        }

        public void ModifyMaxValueByAmountKeepCurrent(float amount)
        {
            maxValue = Mathf.Max(0f, maxValue + amount);
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
            OnValueChanged?.Invoke(currentValue, maxValue);
        }

        public void ModifyMaxValueByPercentage(float percentage)
        {
            float amount = maxValue * percentage;
            ModifyMaxValueByAmount(amount);
        }

        public void ModifyMaxValueMultiplier(float multiplier)
        {
            maxValue = Mathf.Max(0f, maxValue * multiplier);
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
            OnValueChanged?.Invoke(currentValue, maxValue);
        }
    }
}