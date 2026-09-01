using UnityEngine;
using PROJ.Equipment;

public class GunProjectile : WeaponRanged
{
    [Header("Cấu Hình Riêng Súng Projectile")]
    [SerializeField] private GameObject fallbackProjectilePrefab;
    [SerializeField] private float fallbackBulletSpeed = 30f;

    public override void Fire(LayerMask enemyLayers, Vector3 direction)
    {
        if (!PrepareShot(direction, out Vector3 flatDirection)) return;

        GameObject projectilePrefab = ResolveProjectilePrefab(fallbackProjectilePrefab);
        if (projectilePrefab == null) return;

        int pellets = ResolvePelletCount();
        float damage = ResolveDamage(15f);
        float finalBulletSpeed = ResolveProjectileSpeed(fallbackBulletSpeed);
        float maxRange = ResolveMaxRange(30f);
        GunType gunType = weaponData != null ? weaponData.gunType : GunType.Pistol;
        int maxAmmo = weaponData != null ? weaponData.maxAmmo : 0;

        // Lấy tâm nhân vật đưa về cùng cao độ với muzzlePoint
        Vector3 characterCenter = transform.root.position;
        if (muzzlePoint != null)
        {
            characterCenter.y = muzzlePoint.position.y;
        }

        Vector3 spawnOrigin = muzzlePoint != null ? muzzlePoint.position : transform.position;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 rawPelletDir = GetPelletDirection(flatDirection, i, pellets);

            // 1. Điểm ngắm mục tiêu tính từ tâm nhân vật phóng ra xa
            Vector3 aimPoint = characterCenter + rawPelletDir * maxRange;

            // 2. Hướng bay thực tế: từ nòng súng hội tụ chéo vào điểm aimPoint
            Vector3 finalDirection = (aimPoint - spawnOrigin).normalized;

            GameObject bulletGo = Instantiate(
                projectilePrefab,
                spawnOrigin,
                Quaternion.LookRotation(finalDirection, Vector3.up)
            );

            Rigidbody rb = bulletGo.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Unity 6 dùng linearVelocity, các bản cũ dùng velocity
                rb.linearVelocity = finalDirection * finalBulletSpeed;
            }

            ProjectileBullet bulletScript = bulletGo.GetComponent<ProjectileBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(damage, enemyLayers);
            }
        }

        Debug.Log($"[{gunType}]: Đã bắn {pellets} viên đạn vật lý. Còn: {currentAmmo}/{maxAmmo}");
    }
}