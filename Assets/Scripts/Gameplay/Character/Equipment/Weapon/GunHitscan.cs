using PROJ;
using UnityEngine;
using System.Collections;
using PROJ.Attributes;
using PROJ.Equipment;

public class GunHitscan : WeaponRanged
{
    private const float VisualTracerDistance = 1000f;

    [Header("Hiệu Ứng Tia Đạn (Object / Prefab)")]
    [SerializeField] private GameObject fallbackTracerObjectPrefab;
    [SerializeField] private float fallbackTracerDuration = 0.05f;

    public override void Fire(LayerMask enemyLayers, Vector3 direction)
    {
        if (!PrepareShot(direction, out Vector3 flatDirection)) return;

        GunType gunType = weaponData != null ? weaponData.gunType : GunType.Pistol;
        float baseDamage = ResolveDamage(10f);
        int pellets = ResolvePelletCount();
        
        GameObject tracerObjectPrefab = ResolveTracerObjectPrefab(weaponData != null ? weaponData.tracerPrefab : null);
        float tracerDuration = ResolveTracerDuration(weaponData != null ? weaponData.tracerDuration : fallbackTracerDuration);
        float bulletSpeed = weaponData != null ? weaponData.bulletSpeed : 60f; // Tăng tốc độ bay mặc định lên cao để đạn bay nhanh và ngắn gọn hơn

        int maxAmmo = weaponData != null ? weaponData.maxAmmo : 0;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 finalDirection = GetPelletDirection(flatDirection, i, pellets);
            Vector3 targetPosition = muzzlePoint.position + finalDirection * VisualTracerDistance;
            float damage = ResolveCriticalDamage(baseDamage, out bool isCritical);

            if (Physics.Raycast(muzzlePoint.position, finalDirection, out RaycastHit hitInfo, Mathf.Infinity, enemyLayers))
            {
                targetPosition = hitInfo.point;

                CharacterAttributeManager attributeManager =
                    hitInfo.collider.GetComponent<CharacterAttributeManager>();

                if (attributeManager != null && !attributeManager.IsDead)
                {
                    attributeManager.ApplyDamage(damage, isCritical);
                    Debug.Log($"[{gunType}] Trúng {hitInfo.collider.name} gây {damage} dmg{(isCritical ? " (CHÍ MẠNG!)" : string.Empty)}!");
                }
            }

            if (tracerObjectPrefab != null)
            {
                float travelDistance = Vector3.Distance(muzzlePoint.position, targetPosition);
                // Giảm thời gian bay để hạt đạn vút qua nhanh chóng, giúp nó trông giống một viên đạn nhỏ gọn
                float travelTime = Mathf.Max(0.01f, travelDistance / Mathf.Max(1f, bulletSpeed));
                StartCoroutine(SpawnBulletTracerObject(targetPosition, tracerObjectPrefab, Mathf.Max(tracerDuration, travelTime)));
            }
        }

        // Debug.Log($"[{gunType}] Đã bắn {pellets} tia đạn. Còn: {currentAmmo}/{maxAmmo}");
    }

    private GameObject ResolveTracerObjectPrefab(GameObject fallback)
    {
        if (weaponData != null && weaponData.tracerPrefab != null)
        {
            return weaponData.tracerPrefab;
        }
        return fallbackTracerObjectPrefab != null ? fallbackTracerObjectPrefab : fallback;
    }

    private IEnumerator SpawnBulletTracerObject(Vector3 targetPos, GameObject tracerPrefab, float travelTime)
    {
        if (tracerPrefab == null || muzzlePoint == null)
        {
            yield break;
        }

        Vector3 startPos = muzzlePoint.position;
        Vector3 endPos = targetPos;
        Vector3 direction = (endPos - startPos);
        float distance = direction.magnitude;

        if (distance <= 0.001f)
        {
            yield break;
        }

        Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        GameObject tracer = Instantiate(tracerPrefab, startPos, rotation);

        // Tùy chỉnh thu nhỏ trục Z của viên đạn lại để nó ngắn gọn, không bị dài ngoằng
        Vector3 currentScale = tracer.transform.localScale;
        tracer.transform.localScale = new Vector3(currentScale.x, currentScale.y, Mathf.Min(currentScale.z, 0.3f));

        float elapsed = 0f;
        float effectTime = Mathf.Max(0.01f, travelTime);

        while (elapsed < effectTime)
        {
            if (tracer == null) yield break;

            float t = Mathf.Clamp01(elapsed / effectTime);
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            tracer.transform.position = currentPos;
            tracer.transform.rotation = rotation;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (tracer != null)
        {
            Destroy(tracer);
        }
    }
}
