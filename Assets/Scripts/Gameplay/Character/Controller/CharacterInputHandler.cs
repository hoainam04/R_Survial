using UnityEngine;
using PROJ.Attributes;
using PROJ.Equipment;

public class CharacterInputHandler : MonoBehaviour
{
    [Header("Cấu Hình Tự Động Tấn Công (Auto Attack)")]
    [SerializeField] private bool enableAutoAttack = true;

    private CharacterControllerBrain brain;

    private void Awake()
    {
        brain = GetComponent<CharacterControllerBrain>();
    }

    public bool GetDashInput()
    {
        if (GameSettingsManager.Instance != null)
            return GameSettingsManager.Instance.GetKeyDown("Dash");
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetSprintInput()
    {
        if (GameSettingsManager.Instance != null)
            return GameSettingsManager.Instance.GetKey("Sprint");
        return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
    }

    public bool GetAttackInput()
    {
        if (GameSettingsManager.Instance != null && GameSettingsManager.Instance.GetKey("Attack"))
            return true;

        if (Input.GetMouseButton(0))
            return true;

        if (enableAutoAttack && brain != null && brain.AimingHandler != null)
        {
            // Tận dụng CharacterAimingHandler để kiểm tra xem có target nào nằm trong tầm bắn không
            Transform target = brain.AimingHandler.GetCurrentTargetEnemy(transform);
            if (target != null)
            {
                return true; // Tự động bóp cò / tấn công khi có địch trong tầm nhắm
            }
        }

        return false;
    }

    public bool GetReloadInput()
    {
        if (GameSettingsManager.Instance != null)
            return GameSettingsManager.Instance.GetKeyDown("Reload");
        return Input.GetKeyDown(KeyCode.R);
    }

    public Vector3 GetMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, 0f, v).normalized;
    }
}
