using UnityEngine;
using PROJ.Attributes;

[RequireComponent(typeof(Rigidbody))]
public class CharacterGravity : MonoBehaviour
{
    [Header("Cài đặt Trọng lực")]
    [Tooltip("Hệ số nhân trọng lực. Bằng 1 là bình thường, bằng 2 là nặng gấp đôi.")]
    [SerializeField] private float gravityMultiplier = 2.5f; 
    
    private Rigidbody rb;
    private CharacterStatusController statusController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        statusController = GetComponent<CharacterStatusController>();
        if (statusController != null)
        {
            enabled = false;
            Debug.Log("[CharacterGravity] Disabled because CharacterStatusController already handles gravity.");
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody mặc định đã có 1 phần trọng lực (nếu Use Gravity đang bật).
        // Nên ta chỉ cần cộng thêm phầnôiôi trội (gravityMultiplier - 1).
        if (gravityMultiplier > 1f)
        {
            Vector3 extraGravity = Physics.gravity * (gravityMultiplier - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }
}