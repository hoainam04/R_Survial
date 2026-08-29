using UnityEngine;

namespace PROJ.Attributes
{
    /// <summary>
    /// Update: 21/07/2026
    /// Chịu trách nhiệm quản lý các trạng thái của Character:
    /// - Health Regen
    /// - Stamina Regen
    /// - Survival Drain
    /// - Custom Gravity
    /// - Consume Stamina
    /// </summary>
    public class CharacterStatusController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private CharacterAttributeManager attributeManager;
        [SerializeField] private Rigidbody rb;

        [Header("Health Regen")]
        [SerializeField] private bool enableHealthRegen = true;
        [SerializeField] private float healthRegenInterval = 1f;

        [Header("Stamina Regen")]
        [SerializeField] private bool enableStaminaRegen = true;
        [SerializeField] private float staminaRegenAmountPerSecond = 15f;
        [SerializeField] private float staminaRegenDelay = 1f;

        [Header("Survival Drain")]
        [SerializeField] private bool enableSurvivalDrain = true;
        [SerializeField] private float hungerDrainPerSecond = 0.5f;
        [SerializeField] private float thirstDrainPerSecond = 0.8f;

        [Header("Gravity")]
        [SerializeField] private bool enableCustomGravity = true;
        [SerializeField] private float gravity = -25f;
        [SerializeField] private float maxFallSpeed = -50f;

        #endregion

        #region Private Fields

        private HealthRegenModule healthRegenModule;
        private StaminaRegenModule staminaRegenModule;
        private SurvivalDrainModule survivalDrainModule;
        private RigidbodyGravityModule gravityModule;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (attributeManager == null)
                attributeManager = GetComponent<CharacterAttributeManager>();

            if (rb == null)
                rb = GetComponent<Rigidbody>();

            healthRegenModule = new HealthRegenModule(attributeManager, healthRegenInterval);
            staminaRegenModule = new StaminaRegenModule(attributeManager, staminaRegenAmountPerSecond, staminaRegenDelay);
            survivalDrainModule = new SurvivalDrainModule(attributeManager, hungerDrainPerSecond, thirstDrainPerSecond);
            gravityModule = new RigidbodyGravityModule(rb, gravity, maxFallSpeed);

            if (rb != null && enableCustomGravity)
                rb.useGravity = false;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (enableHealthRegen)
                healthRegenModule.Tick(deltaTime);

            if (enableStaminaRegen)
                staminaRegenModule.Tick(deltaTime);

            if (enableSurvivalDrain)
                survivalDrainModule.Tick(deltaTime);
        }

        private void FixedUpdate()
        {
            if (enableCustomGravity)
                gravityModule.FixedTick(Time.fixedDeltaTime);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tiêu hao stamina và reset delay hồi stamina.
        /// </summary>
        public void ConsumeStamina(float amount)
        {
            attributeManager.ConsumeStamina(amount);
            staminaRegenModule.NotifyStaminaConsumed();
        }

        public void SetCustomGravityEnabled(bool value)
        {
            enableCustomGravity = value;

            if (rb != null)
                rb.useGravity = !enableCustomGravity;
        }

        #endregion
    }
}