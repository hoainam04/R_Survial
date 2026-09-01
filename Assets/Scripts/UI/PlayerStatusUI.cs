using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PROJ.Attributes;

namespace PROJ.UI
{
    public class PlayerStatusUI : MonoBehaviour
    {
        [Header("Target Character")]
        [SerializeField] private CharacterAttributeManager attributeManager;

        [Header("UI References - Health")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject damagePopupPrefab; // Prefab chứa component DamagePopup
        [SerializeField] private Transform damagePopupSpawnParent; // Vị trí cha để spawn popup (nếu trống sẽ dùng transform của PlayerStatusUI)

        [Header("UI References - Stamina")]
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private TextMeshProUGUI staminaText;

        private void Start()
        {
            if (attributeManager == null)
            {
                attributeManager = FindObjectOfType<CharacterAttributeManager>();
            }

            if (attributeManager != null)
            {
                SubscribeEvents();
            }
            else
            {
                Debug.LogWarning("[PlayerStatusUI]: Không tìm thấy CharacterAttributeManager trong scene.");
            }

            if (damagePopupSpawnParent == null)
            {
                damagePopupSpawnParent = transform;
            }
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var healthAttr = attributeManager.GetAttribute(AttributeType.Health);
            if (healthAttr != null)
            {
                healthAttr.OnValueChanged += UpdateHealthUI;
                UpdateHealthUI(healthAttr.CurrentValue, healthAttr.MaxValue);
            }

            var staminaAttr = attributeManager.GetAttribute(AttributeType.Stamina);
            if (staminaAttr != null)
            {
                staminaAttr.OnValueChanged += UpdateStaminaUI;
                UpdateStaminaUI(staminaAttr.CurrentValue, staminaAttr.MaxValue);
            }

            attributeManager.OnDamageTaken += HandleDamageTaken;
        }

        private void UnsubscribeEvents()
        {
            if (attributeManager == null) return;

            var healthAttr = attributeManager.GetAttribute(AttributeType.Health);
            if (healthAttr != null)
            {
                healthAttr.OnValueChanged -= UpdateHealthUI;
            }

            var staminaAttr = attributeManager.GetAttribute(AttributeType.Stamina);
            if (staminaAttr != null)
            {
                staminaAttr.OnValueChanged -= UpdateStaminaUI;
            }

            attributeManager.OnDamageTaken -= HandleDamageTaken;
        }

        private void UpdateHealthUI(float current, float max)
        {
            if (healthSlider != null)
            {
                healthSlider.value = max > 0f ? current / max : 0f;
            }

            if (healthText != null)
            {
                healthText.text = $"{Mathf.Ceil(current)} / {Mathf.Ceil(max)}";
            }
        }

        private void HandleDamageTaken(float damageTaken, bool isCritical)
        {
            SpawnDamagePopup(damageTaken, isCritical);
        }

        public void TriggerCustomDamagePopup(float damage, bool isCritical)
        {
            SpawnDamagePopup(damage, isCritical);
        }

        private void SpawnDamagePopup(float damage, bool isCritical)
        {
            if (damagePopupPrefab != null && damagePopupSpawnParent != null)
            {
                GameObject popupObj = Instantiate(damagePopupPrefab, damagePopupSpawnParent);
                
                // Đặt vị trí ngẫu nhiên xung quanh tâm một chút để không bị dính sát trùng khớp đường thẳng
                RectTransform rectTransform = popupObj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 randomOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-2f, 2f));
                    rectTransform.anchoredPosition = randomOffset;
                }

                DamagePopup popup = popupObj.GetComponent<DamagePopup>();
                if (popup != null)
                {
                    popup.Initialize(damage, isCritical);
                }
            }
        }

        private void UpdateStaminaUI(float current, float max)
        {
            if (staminaSlider != null)
            {
                staminaSlider.value = max > 0f ? current / max : 0f;
            }

            if (staminaText != null)
            {
                staminaText.text = $"{Mathf.Ceil(current)} / {Mathf.Ceil(max)}";
            }
        }
    }
}
