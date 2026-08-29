using UnityEngine;
using PROJ;
using PROJ.Equipment;
using PROJ.Attributes;

public class CharacterAttack : MonoBehaviour
{
    [Header("Cấu Hình Hồi Chiêu (Cooldown)")]
    // [SerializeField] private float attackCooldown = 0.5f;
    private float nextAttackTime;

    [Header("Cấu Hình Hitbox Cận Chiến")]
    [SerializeField] private Transform attackPoint;      // Điểm tâm hitbox (đầu vũ khí hoặc trước mặt)
    // [SerializeField] private float attackRange = 1f;         // Khoảng cách quét sát thương (Cận chiến: bán kính | Súng: khoảng cách bắn xa nhất)]
    // [SerializeField] private float hitboxRadius = 0.5f;    // Bán kính hình cầu quét sát thương
    [SerializeField] private LayerMask enemyLayers;       // Bộ lọc Layer kẻ địch (Ví dụ: "Enemy")
    // [SerializeField] private float baseDamage = 20f;      // Sát thương cơ bản khi chém

    // Chỉ cho phép đánh khi đã hết thời gian cooldown
    private CharacterControllerBrain brain;
    private void Start()
    {
        if (brain == null)
        {
            brain = GetComponent<CharacterControllerBrain>();
            if (brain == null)
            {
                Debug.LogError("[CharacterAttack]: Không tìm thấy CharacterControllerBrain trên GameObject này!");
            }
        }
    }
    public bool CanAttack() => Time.time >= nextAttackTime;

    /// <summary>
    /// Kích hoạt hồi chiêu ngay khi bắt đầu vung đòn
    /// </summary>
    public void ExecuteAttack(Vector3 direction)
    {
        var attributeManager = GetComponent<CharacterAttributeManager>();
        if (attributeManager == null)
        {
            Debug.LogWarning("[CharacterAttack]: Không tìm thấy CharacterAttributeManager để lấy AttackPower!");
            return;
        }
        WeaponEquipmentSO weaponData = brain.EquipmentManager.CurrentWeaponData;
        if (weaponData == null)
        {
            // Logic đấm tay không hoặc cận chiến mặc định nếu không có dữ liệu vũ khí
            Debug.Log("[CharacterAttack]: Thực hiện một đòn tay không");
        }
        else
        {
            // Logic tấn công với vũ khí hiện tại
            // Debug.Log($"[CharacterAttack]: Thực hiện một đòn với vũ khí: {weaponData.itemName}");
            if (weaponData.weaponType == WeaponType.Ranged)
            {
                // Lấy GameObject hiển thị của khẩu súng đang cầm trên tay từ EquipmentManager
                GameObject gunVisual = brain.EquipmentManager.CurrentWeaponVisualObject;

                if (gunVisual != null)
                {
                    // Tìm linh hồn (Component WeaponRanged) gắn trên Prefab súng
                    WeaponRanged activeGun = gunVisual.GetComponent<WeaponRanged>();

                    if (activeGun != null && activeGun.CanFire())
                    {
                        // Thực hiện bóp cò, truyền hướng trước mặt nhân vật làm hướng đạn bay
                        activeGun.Fire(enemyLayers, direction);
                    }
                    else if (activeGun != null && !activeGun.CanFire() && brain.GetAttackInput())
                    {
                        // Gợi ý: Nếu hết đạn mà người chơi vẫn cố tình bấm bắn -> Tự động gọi Reload
                        activeGun.Reload();
                    }
                }
            }
        }

        float cooldown = 0.5f;
        if (weaponData is WeaponMeleeSO meleeData)
        {
            cooldown = Mathf.Max(0.01f, meleeData.attackCooldown);
        }
        else if (weaponData is WeaponRangedSO rangedData)
        {
            cooldown = Mathf.Max(0.01f, rangedData.fireRate);
        }

        nextAttackTime = Time.time + cooldown;
    }

    /// <summary>
    /// Thực hiện quét sát thương thực tế (Gọi từ Animation Event hoặc quét trực tiếp từ Code)
    /// </summary>
    public void TriggerMeleeHitbox()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("[CharacterAttack]: Quên chưa gán AttackPoint kìa ông ơi!");
            return;
        }

        if (brain == null || brain.AttributeManager == null) return;

        // --- BƯỚC 1: LẤY ATTACK RANGE & TỰ ĐỘNG TÍNH RADIUS ---
        float finalAttackRange = 1f; // Tầm đánh mặc định (Tay không)
        var rangeAttr = brain.AttributeManager.GetAttribute(AttributeType.AttackRange);
        if (rangeAttr != null)
        {
            finalAttackRange = rangeAttr.CurrentValue;
        }

        // Bán kính hình cầu quét bằng 1/2 độ dài tầm đánh theo yêu cầu của ông
        float calculatedRadius = finalAttackRange / 2f;

        // TÂM HÌNH CẦU = Điểm gốc + (Hướng trước mặt nhân vật * khoảng cách dịch chuyển ra trước)
        // Dịch tâm ra trước một khoảng vừa bằng bán kính để rìa sau hình cầu chạm đúng nhân vật
        Vector3 sphereCenter = attackPoint.position + (transform.forward * calculatedRadius);


        // --- BƯỚC 2: TÍNH TOÁN TỔNG SÁT THƯƠNG THỰC TẾ ---
        // float weaponDamage = 0f;
        // if (brain.EquipmentManager != null && brain.EquipmentManager.CurrentWeaponData != null)
        // {
        //     weaponDamage = brain.EquipmentManager.CurrentWeaponData.damage;
        // }

        float playerAttackPower = 0f;
        var attackPowerAttr = brain.AttributeManager.GetAttribute(AttributeType.AttackPower);
        if (attackPowerAttr != null)
        {
            playerAttackPower = attackPowerAttr.CurrentValue;
        }

        float finalDamage = playerAttackPower;

        // --- BƯỚC 3: TÍNH TOÁN BẠO KÍCH (CRITICAL) ---
        bool isCrit = false;
        var critRateAttr = brain.AttributeManager.GetAttribute(AttributeType.CriticalRate);
        if (critRateAttr != null && Random.Range(0f, 100f) <= critRateAttr.CurrentValue)
        {
            isCrit = true;
            var critDamageAttr = brain.AttributeManager.GetAttribute(AttributeType.CriticalDamage);
            float critMultiplier = critDamageAttr != null ? (critDamageAttr.CurrentValue / 100f) : 1.5f;
            finalDamage *= critMultiplier;
        }

        // --- BƯỚC 4: QUÉT HÌNH CẦU TẠI TÂM MỚI ---
        Collider[] hitEnemies = Physics.OverlapSphere(sphereCenter, calculatedRadius, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            var attributeManager = enemy.GetComponent<CharacterAttributeManager>();
            if (attributeManager != null && !attributeManager.IsDead)
            {
                attributeManager.ApplyDamage(finalDamage, isCrit);
                // Debug.Log($"[CharacterAttack]: Đã chém trúng: {enemy.name} | Sát thương: {finalDamage} {(isCrit ? "(CHÍ MẠNG!)" : "")}");
            }
        }
    }

    public void SetMeleeHitboxActive(bool isActive)
    {
        if (isActive) TriggerMeleeHitbox();
    }

    // Vẽ vòng tròn mô phỏng vùng sát thương thực tế trong Scene dựa trên thuộc tính hiện tại
    private void OnDrawGizmos()
    {
        /*
        if (attackPoint == null) return;

        // Vì lúc chưa Play game không có AttributeManager, lấy tạm thông số ảo để hiển thị Editor
        float range = 1.5f;
        if (Application.isPlaying && brain != null && brain.AttributeManager != null)
        {
            var rangeAttr = brain.AttributeManager.GetAttribute(AttributeType.AttackRange);
            if (rangeAttr != null) range = rangeAttr.CurrentValue;
        }

        float radius = range / 2f;
        Vector3 center = attackPoint.position + (transform.forward * radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
        */
    }
}