using UnityEngine;
using PROJ; // Để nhận diện IDamageable
using PROJ.Attributes; // Để nhận diện CharacterAttributeManager

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : MonoBehaviour
{
    [Header("Cấu Hình Hủy Đạn")]
    [SerializeField] private float lifeTime = 3f; // Tự hủy sau 3 giây nếu bắn hụt lên trời để tránh tràn RAM
    [SerializeField] private GameObject hitEffectPrefab; // Hiệu ứng nổ/văng máu (Tùy chọn)

    private float damage;
    private LayerMask targetLayers;
    private bool hasHit; // Chặn trường hợp 1 viên đạn nổ sát thương 2 lần trong cùng 1 frame

    public void Setup(float damageAmt, LayerMask enemyLayers)
    {
        this.damage = damageAmt;
        this.targetLayers = enemyLayers;
        this.hasHit = false;

        // Bắt đầu đếm ngược tự hủy
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // 1. Kiểm tra xem va chạm có nằm trong Layer địch được chỉ định không
        // Sử dụng toán tử bit để check Layer chuẩn chỉ
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            hasHit = true;

            // 2. Gây sát thương cho mục tiêu
            var attributeManager = other.GetComponent<CharacterAttributeManager>();
            if (attributeManager != null && !attributeManager.IsDead)
            {
                attributeManager.ApplyDamage(damage, false);
                Debug.Log($"[Projectile] Viên đạn găm vào {other.name} gây {damage} dmg!");
            }

            // 3. Xử lý hiệu ứng hình ảnh (Nếu có)
            if (hitEffectPrefab != null)
            {
                // Sinh ra hiệu ứng tại vị trí viên đạn chạm vào mục tiêu
                GameObject fx = Instantiate(hitEffectPrefab, transform.position, transform.rotation);
                Destroy(fx, 1f); // Hủy hạt hiệu ứng sau 1 giây
            }

            // 4. Hủy viên đạn ngay lập tức sau khi trúng mục tiêu
            Destroy(gameObject);
        }
        else 
        {
            // Tùy chọn: Nếu chạm vào tường hoặc chướng ngại vật (không phải Layer địch) thì cũng hủy đạn
            // Ông có thể tạo 1 Layer "Ground" hoặc "Obstacle" để check thêm ở đây nếu muốn đạn chạm tường là nổ
            if (/*other.gameObject.CompareTag("Environment") ||*/other.gameObject.CompareTag("Ground")|| other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                hasHit = true;
                Destroy(gameObject);
            }
        }
    }
}