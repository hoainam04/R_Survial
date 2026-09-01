using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Attributes
{
    public class CharacterAttributeManager : MonoBehaviour
    {
        [Header("Base Attribute From SO")]
        [SerializeField] private CharacterAttributeSetSO attributeSetSO;

        [Header("Fallback If SO Is Null")]
        [SerializeField] private List<CharacterAttribute> attributes = new();

        private Dictionary<AttributeType, CharacterAttribute> attributeDict;

        public bool IsDead => GetAttribute(AttributeType.Health)?.CurrentValue <= 0f;

        private void Awake()
        {
            InitializeAttributes();
        }

        private void InitializeAttributes()
        {
            attributeDict = new Dictionary<AttributeType, CharacterAttribute>();

            if (attributeSetSO != null)
            {
                foreach (var attr in attributeSetSO.Attributes)
                {
                    AddRuntimeAttribute(attr);
                }
            }
            else
            {
                foreach (var attr in attributes)
                {
                    AddRuntimeAttribute(attr);
                }
            }
        }

        private void AddRuntimeAttribute(CharacterAttribute sourceAttribute)
        {
            if (sourceAttribute == null)
                return;

            CharacterAttribute runtimeAttribute = new CharacterAttribute(sourceAttribute);
            runtimeAttribute.Initialize();

            if (!attributeDict.ContainsKey(runtimeAttribute.Type))
            {
                attributeDict.Add(runtimeAttribute.Type, runtimeAttribute);
            }
            else
            {
                Debug.LogWarning($"Attribute {runtimeAttribute.Type} bị trùng trên {gameObject.name}");
            }
        }

        public CharacterAttribute GetAttribute(AttributeType type)
        {
            if (attributeDict.TryGetValue(type, out var attr))
            {
                return attr;
            }

            Debug.LogWarning($"Attribute {type} không tồn tại trên {gameObject.name}");
            return null;
        }

        public void ApplyDamage(float amount,bool isCritical = false)
        {
            var health = GetAttribute(AttributeType.Health);
            if (health == null)
            {
                Debug.LogWarning($"[{gameObject.name}] Missing Health attribute, cannot apply damage.");
                return;
            }

            var defense = GetAttribute(AttributeType.Defense)?.CurrentValue ?? 0f;
            float finalDamage = Mathf.Max(1f, amount - defense);
            health.Modify(-finalDamage);
            // Debug.Log($"[{gameObject.name} Take Damage ]-{finalDamage} HP {health.CurrentValue}/ {health.MaxValue} HP{(isCritical ? " (CRIT)" : "")}");

            if (health.CurrentValue <= 0)
            {
                HandleDeath();
            }
        }
        // {
        //     var health = GetAttribute(AttributeType.Health);
        //     var defense = GetAttribute(AttributeType.Defense)?.CurrentValue ?? 0f;
        //     float finalDamage = Mathf.Max(1f, amount - defense);
        //     health?.Modify(-finalDamage);

        //     if (health != null && health.CurrentValue <= 0)
        //     {
        //         HandleDeath();
        //     }
        // }

        public void ConsumeStamina(float amount)
        {
            var stamina = GetAttribute(AttributeType.Stamina);
            if (stamina == null)
            {
                Debug.LogWarning($"[{gameObject.name}] Missing Stamina attribute, cannot consume stamina.");
                return;
            }

            stamina.Modify(-amount);
            Debug.Log($"[{gameObject.name} Consume Stamina ]-{amount} Stamina {stamina.CurrentValue}/{stamina.MaxValue} Stamina");
        }

        public void RegenerateStamina(float amount)
        {
            GetAttribute(AttributeType.Stamina)?.Modify(amount);
        }

        private void HandleDeath()
        {
            Debug.Log($"{gameObject.name} đã hẻo!");
            gameObject.SetActive(false);
        }
    }
}