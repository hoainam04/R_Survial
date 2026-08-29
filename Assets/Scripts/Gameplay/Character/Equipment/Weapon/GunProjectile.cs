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
        GunType gunType = weaponData != null ? weaponData.gunType : GunType.Pistol;
        int maxAmmo = weaponData != null ? weaponData.maxAmmo : 0;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 finalDirection = GetPelletDirection(flatDirection, i, pellets);

            GameObject bulletGo = Instantiate(
                projectilePrefab,
                muzzlePoint.position,
                Quaternion.LookRotation(finalDirection, Vector3.up)
            );

            Rigidbody rb = bulletGo.GetComponent<Rigidbody>();
            if (rb != null)
            {
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