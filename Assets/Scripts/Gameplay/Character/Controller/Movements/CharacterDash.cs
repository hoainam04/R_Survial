using System;
using System.Collections;
using UnityEngine;
using PROJ.Attributes;

[RequireComponent(typeof(Rigidbody))]
public class CharacterDash : MonoBehaviour
{
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float consumeStamina = 20f; 

    private Rigidbody rb;
    private CharacterAttributeManager attributeManager;
    private float nextDashTime;
    
    public bool IsDashing { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        attributeManager = GetComponent<CharacterAttributeManager>();
    }

    public bool CanDash()
    {
        // Kiểm tra cooldown, trạng thái lướt hiện tại và điều kiện thể lực
        if (Time.time < nextDashTime || IsDashing) return false;

        if (attributeManager != null)
        {
            var stamina = attributeManager.GetAttribute(AttributeType.Stamina);
            if (stamina != null && stamina.CurrentValue < consumeStamina) return false;
        }

        return true;
    }

    public void StartDash(Vector3 moveDirection, Action onDashComplete)
    {
        // Kiểm tra an toàn hệ thống Attribute trước
        if (attributeManager == null)
        {
            Debug.LogWarning("[CharacterDash]: CharacterAttributeManager chưa được gán!");
            onDashComplete?.Invoke(); // Gọi để giải phóng State Machine, tránh kẹt State
            return;
        }

        if (attributeManager.GetAttribute(AttributeType.DashSpeed) == null)
        {
            Debug.LogWarning("[CharacterDash]: Thuộc tính DashSpeed chưa được gán trong AttributeManager!");
            onDashComplete?.Invoke(); 
            return;
        }

        // Nếu không đủ điều kiện lướt thực tế (cooldown hoặc thiếu thể lực)
        if (!CanDash())
        {
            Debug.LogWarning("[CharacterDash]: Không đủ điều kiện để Dash (Có thể do Cooldown hoặc Hết Stamina)!");
            onDashComplete?.Invoke(); // Giải phóng State Machine về Idle/Move thay vì bị kẹt
            return;
        }

        // Tiêu hao thể lực khi bắt đầu lướt
        attributeManager.ConsumeStamina(consumeStamina); 

        // Nếu không bấm nút di chuyển, lướt về phía trước mặt nhân vật
        Vector3 direction = moveDirection.magnitude > 0.1f ? moveDirection.normalized : transform.forward;
        
        StartCoroutine(DashRoutine(direction, onDashComplete));
    }

    private IEnumerator DashRoutine(Vector3 direction, Action onDashComplete)
    {
        IsDashing = true;
        nextDashTime = Time.time + dashCooldown;

        // Lấy tốc độ lướt real-time từ hệ thống Attribute
        float currentDashSpeed = attributeManager.GetAttribute(AttributeType.DashSpeed).CurrentValue;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            // Áp dụng lực vận tốc chính xác (Tốc độ * Hướng)
            rb.linearVelocity = direction * currentDashSpeed; 
            yield return null;
        }

        // Trả lại trạng thái bình thường
        IsDashing = false;
        onDashComplete?.Invoke(); // Báo cho State Machine biết là đã lướt xong để thoát DashState
    }
}