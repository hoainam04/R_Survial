using UnityEngine;
using PROJ.Attributes;
using PROJ.Equipment;

public class CharacterInputHandler : MonoBehaviour
{
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
        if (GameSettingsManager.Instance != null)
            return GameSettingsManager.Instance.GetKey("Attack");
        return Input.GetMouseButton(0);
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
