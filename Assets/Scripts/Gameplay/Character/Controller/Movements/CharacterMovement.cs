using UnityEngine;
using PROJ.Attributes;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterAttributeManager))]
[RequireComponent(typeof(CharacterStatusController))]
public class CharacterMovement : MonoBehaviour
{
    #region Inspector

    [Header("Run Settings")]
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float staminaCostPerSecond = 15f;

    #endregion

    #region Private Fields

    private Rigidbody rb;
    private Vector3 moveDirection;

    private CharacterAttributeManager attributeManager;
    private CharacterStatusController statusController;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        attributeManager = GetComponent<CharacterAttributeManager>();
        statusController = GetComponent<CharacterStatusController>();
    }

    #endregion

    #region Public Methods

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    public void Move()
    {
        if (!ValidateAttributes(AttributeType.MoveSpeed))
            return;

        float speed = attributeManager.GetAttribute(AttributeType.MoveSpeed).CurrentValue;

        ApplyMovement(speed);
    }

    public void Run()
    {
        if (!ValidateAttributes(AttributeType.MoveSpeed))
            return;

        CharacterAttribute stamina = attributeManager.GetAttribute(AttributeType.Stamina);

        if (stamina == null)
        {
            Move();
            return;
        }

        if (moveDirection.sqrMagnitude <= 0.0001f)
        {
            Move();
            return;
        }

        if (stamina.CurrentValue <= 0f)
        {
            Move();
            return;
        }

        float speed = attributeManager.GetAttribute(AttributeType.MoveSpeed).CurrentValue * runSpeedMultiplier;

        ApplyMovement(speed);

        statusController.ConsumeStamina(staminaCostPerSecond * Time.fixedDeltaTime);
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }

    #endregion

    #region Private Methods

    private void ApplyMovement(float speed)
    {
        Vector3 targetVelocity = moveDirection * speed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    private bool ValidateAttributes(AttributeType attributeType)
    {
        if (attributeManager == null)
        {
            Debug.LogWarning("[CharacterMovement] CharacterAttributeManager is missing.");
            return false;
        }

        if (attributeManager.GetAttribute(attributeType) == null)
        {
            Debug.LogWarning($"[CharacterMovement] Missing Attribute : {attributeType}");
            return false;
        }

        return true;
    }

    #endregion
}