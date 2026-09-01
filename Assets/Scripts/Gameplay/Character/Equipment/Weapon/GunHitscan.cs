using PROJ;
using UnityEngine;
using System.Collections;
using PROJ.Attributes;
using PROJ.Equipment;
using PROJ.Patterns;

public class GunHitscan : WeaponRanged
{
    [Header("Hiệu Ứng Tia Đạn (Object / Prefab)")]
    [SerializeField] private GameObject fallbackTracerObjectPrefab;
    [SerializeField] private float fallbackTracerDuration = 0.05f;

    [Header("Pool Config")]
    [SerializeField] private int initialTracerPoolSize = 10;
    [SerializeField] private int maxTracerPoolSize = 30;

    private GenericObjectPool<Transform> tracerPool;

    protected override void Start()
    {
        base.Start();
        InitializeTracerPool();
    }

    private void InitializeTracerPool()
    {
        GameObject prefab = ResolveTracerObjectPrefab(fallbackTracerObjectPrefab);
        if (prefab != null)
        {
            tracerPool = new GenericObjectPool<Transform>(
                prefab.transform, 
                initialTracerPoolSize, 
                maxTracerPoolSize
            );
        }
    }

    public override void Fire(LayerMask enemyLayers, Vector3 direction)
    {
        if (!PrepareShot(direction, out Vector3 flatDirection)) return;

        GunType gunType = weaponData != null ? weaponData.gunType : GunType.Pistol;
        float maxRange = ResolveMaxRange(20f);
        float damage = ResolveDamage(10f);
        int pellets = ResolvePelletCount();
        
        float tracerDuration = ResolveTracerDuration(weaponData != null ? weaponData.tracerDuration : fallbackTracerDuration);
        float bulletSpeed = weaponData != null ? weaponData.bulletSpeed : 60f;
        int maxAmmo = weaponData != null ? weaponData.maxAmmo : 0;

        Vector3 characterCenter = transform.root.position;
        if (muzzlePoint != null)
        {
            characterCenter.y = muzzlePoint.position.y;
        }

        Vector3 spawnOrigin = muzzlePoint != null ? muzzlePoint.position : transform.position;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 rawPelletDir = GetPelletDirection(flatDirection, i, pellets);

            // 1. Điểm ngắm từ tâm người chơi
            Vector3 aimPoint = characterCenter + rawPelletDir * maxRange;

            // 2. Hội tụ tia đạn từ nòng súng vào tâm ngắm
            Vector3 correctedDirection = (aimPoint - spawnOrigin).normalized;
            Vector3 targetPosition = spawnOrigin + correctedDirection * maxRange;

            // 3. Raycast xử lý sát thương tức thời
            if (Physics.Raycast(spawnOrigin, correctedDirection, out RaycastHit hitInfo, maxRange, enemyLayers))
            {
                targetPosition = hitInfo.point;

                CharacterAttributeManager attributeManager =
                    hitInfo.collider.GetComponent<CharacterAttributeManager>();

                if (attributeManager != null && !attributeManager.IsDead)
                {
                    attributeManager.ApplyDamage(damage, false);
                    // Debug.Log($"[{gunType}] Trúng {hitInfo.collider.name} gây {damage} dmg!");
                }
            }

            // 4. Sinh Tracer từ Pool
            if (tracerPool != null)
            {
                float travelDistance = Vector3.Distance(spawnOrigin, targetPosition);
                float travelTime = Mathf.Max(0.01f, travelDistance / Mathf.Max(1f, bulletSpeed));
                StartCoroutine(SpawnPooledTracerRoutine(targetPosition, Mathf.Max(tracerDuration, travelTime)));
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

    private IEnumerator SpawnPooledTracerRoutine(Vector3 targetPos, float travelTime)
    {
        if (tracerPool == null || muzzlePoint == null) yield break;

        Vector3 startPos = muzzlePoint.position;
        Vector3 direction = targetPos - startPos;
        float distance = direction.magnitude;

        if (distance <= 0.001f) yield break;

        Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        
        // 1. Lấy ra khỏi Pool
        Transform tracer = tracerPool.Get(startPos, rotation);

        // 2. Fix triệt để TrailRenderer: Tắt emitting -> Clear -> Chờ 1 frame -> Bật lại
        TrailRenderer trail = tracer.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false;
            tracer.position = startPos;
            trail.Clear();
            yield return null; // Chờ Unity đồng bộ vị trí mới trong pipeline render
            trail.Clear();
            trail.emitting = true;
        }

        Vector3 currentScale = tracer.localScale;
        tracer.localScale = new Vector3(currentScale.x, currentScale.y, Mathf.Min(currentScale.z, 0.3f));

        float elapsed = 0f;
        float effectTime = Mathf.Max(0.01f, travelTime);

        // 3. Bay từ họng súng đến đích
        while (elapsed < effectTime)
        {
            if (tracer == null || !tracer.gameObject.activeSelf) yield break;

            float t = Mathf.Clamp01(elapsed / effectTime);
            tracer.position = Vector3.Lerp(startPos, targetPos, t);
            tracer.rotation = rotation;

            elapsed += Time.deltaTime;
            yield return null;
        }

        tracer.position = targetPos;

        // 4. Thu hồi về Pool an toàn
        if (tracer != null)
        {
            if (trail != null)
            {
                trail.emitting = false;
                trail.Clear();
            }
            tracerPool.ReturnToPool(tracer);
        }
    }
}