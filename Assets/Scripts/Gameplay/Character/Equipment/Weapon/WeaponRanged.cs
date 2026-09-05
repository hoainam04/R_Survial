using UnityEngine;
using System.Collections;
using PROJ.Attributes;
using PROJ.Equipment;

public abstract class WeaponRanged : MonoBehaviour
{
    [Header("Cấu Hình Băng Đạn & Khai Hỏa")]
    public WeaponRangedSO weaponData;
    [SerializeField] protected Transform muzzlePoint;

    [Header("Model Animation")]
    [SerializeField] private Transform gunModel;
    [SerializeField] private float restoreSpeed = 15f;

    [Header("Visual Recoil")]
    [SerializeField] private Vector3 recoilPositionOffset = new Vector3(0f, -0.15f, 0f);
    [SerializeField] private Vector3 recoilRotationOffset = new Vector3(1f, 0f, 0f);
    protected int currentAmmo;
    public bool IsReloading { get; protected set; }
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Coroutine recoilCoroutine;
    private float currentSpread;
    private int recoilStep;
    private float horizontalRecoilOffset;
    private float verticalRecoilOffset;

    protected virtual void Start()
    {
        InitializeWeaponState();
        CacheGunModelPose();
    }

    protected virtual void Update()
    {
        RecoverSpread();
    }
    public bool CanFire() => weaponData != null && currentAmmo > 0 && !IsReloading;
    public abstract void Fire(LayerMask enemyLayers, Vector3 direction);
    public void Reload()
    {
        if (weaponData == null || IsReloading || currentAmmo == weaponData.maxAmmo) return;

        StartCoroutine(ReloadCoroutine());
    }
    public void SetReloadingStatus(bool status)
    {
        IsReloading = status;
    }
    public void RefillAmmo()
    {
        if (weaponData == null) return;
        currentAmmo = weaponData.maxAmmo;
    }
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => weaponData != null ? weaponData.maxAmmo : 0;
    private IEnumerator ReloadCoroutine()
    {
        IsReloading = true;
        Debug.Log($"[Súng]: Đang nạp đạn... Chờ {weaponData.reloadTime}s");

        yield return new WaitForSeconds(weaponData.reloadTime);

        currentAmmo = weaponData.maxAmmo;
        IsReloading = false;

        Debug.Log("[Súng]: Đã nạp đầy băng đạn!");
    }
    protected void AddShotRecoil()
    {
        if (weaponData == null) return;

        recoilStep++;
        horizontalRecoilOffset = Random.Range(-1f, 1f);
        verticalRecoilOffset = Random.Range(-1f, 1f);
        currentSpread = Mathf.Clamp(
            currentSpread + weaponData.spreadIncreasePerShot,
            weaponData.minSpread,
            weaponData.maxSpread
        );
    }
    private void RecoverSpread()
    {
        if (weaponData == null) return;

        currentSpread = Mathf.MoveTowards(
            currentSpread,
            weaponData.minSpread,
            weaponData.spreadRecoverySpeed * Time.deltaTime
        );

        if (Mathf.Approximately(currentSpread, weaponData.minSpread))
        {
            recoilStep = 0;
        }
    }
    protected Vector3 ApplyBulletRecoil(Vector3 direction)
    {
        if (weaponData == null) return direction.normalized;

        direction = GetFlatDirection(direction);

        if (direction.sqrMagnitude <= 0.001f)
            return transform.forward;

        float verticalOffset = verticalRecoilOffset * weaponData.verticalRecoil * currentSpread;
        float horizontalOffset = horizontalRecoilOffset * weaponData.horizontalRecoil * currentSpread;
        Vector3 sideAxis = new Vector3(-direction.z, 0f, direction.x).normalized;

        return (
            direction +
            sideAxis * horizontalOffset +
            Vector3.up * verticalOffset
        ).normalized;
    }
    protected bool PrepareShot(Vector3 inputDirection, out Vector3 flatDirection)
    {
        if (!CanFire())
        {
            flatDirection = Vector3.zero;
            return false;
        }

        flatDirection = GetFlatDirection(inputDirection);
        RotateGunModel(flatDirection);
        SetPlayerAnimation();

        currentAmmo--;
        AddShotRecoil();
        return true;
    }
    protected float ResolveDamage(float fallback) => weaponData != null ? weaponData.damage : fallback;

    protected float ResolveCriticalDamage(float baseDamage, out bool isCritical)
    {
        isCritical = false;
        float finalDamage = Mathf.Max(1f, baseDamage);

        CharacterControllerBrain attackerBrain = GetComponentInParent<CharacterControllerBrain>();
        if (attackerBrain == null || attackerBrain.AttributeManager == null)
        {
            return finalDamage;
        }

        var critRateAttr = attackerBrain.AttributeManager.GetAttribute(AttributeType.CriticalRate);
        if (critRateAttr == null || Random.Range(0f, 100f) > critRateAttr.CurrentValue)
        {
            return finalDamage;
        }

        isCritical = true;
        var critDamageAttr = attackerBrain.AttributeManager.GetAttribute(AttributeType.CriticalDamage);
        float critMultiplier = critDamageAttr != null ? (critDamageAttr.CurrentValue / 100f) : 1.5f;
        finalDamage = baseDamage * Mathf.Max(1f, critMultiplier);
        return finalDamage;
    }
    protected float ResolveMaxRange(float fallback) => weaponData != null ? weaponData.attackRange : fallback;
    protected float ResolveProjectileSpeed(float fallback)
    {
        if (weaponData == null)
        {
            return fallback;
        }

        return weaponData.bulletSpeed > 0f ? weaponData.bulletSpeed : fallback;
    }
    protected GameObject ResolveProjectilePrefab(GameObject fallback)
    {
        if (weaponData != null && weaponData.projectilePrefab != null)
        {
            return weaponData.projectilePrefab;
        }

        return fallback;
    }
    protected GameObject ResolveTracerPrefab(GameObject fallback)
    {
        if (weaponData != null && weaponData.tracerPrefab != null)
        {
            return weaponData.tracerPrefab;
        }

        return fallback;
    }
    protected float ResolveTracerDuration(float fallback)
    {
        if (weaponData != null && weaponData.tracerDuration > 0f)
        {
            return weaponData.tracerDuration;
        }

        return fallback;
    }
    protected int ResolvePelletCount() => weaponData != null ? weaponData.ResolvePelletCount() : 1;
    protected Vector3 GetPelletDirection(Vector3 direction, int pelletIndex, int pelletCount)
    {
        Vector3 baseDirection = ApplyBulletRecoil(direction);

        if (weaponData == null || weaponData.gunType != GunType.Shotgun || pelletCount <= 1)
        {
            return baseDirection;
        }

        float spreadAngle = weaponData.ResolveSpreadConeAngle();
        float spreadRadians = spreadAngle * Mathf.Deg2Rad;
        GetSpreadBasis(baseDirection, out Vector3 right, out Vector3 up);
        return GetRandomConeDirection(baseDirection, right, up, spreadRadians);
    }

    private void InitializeWeaponState()
    {
        if (weaponData == null) return;

        currentAmmo = weaponData.maxAmmo;
        currentSpread = weaponData.minSpread;
    }

    private void CacheGunModelPose()
    {
        if (gunModel == null) return;

        originalLocalPosition = gunModel.localPosition;
        originalLocalRotation = gunModel.localRotation;
    }

    private void GetSpreadBasis(Vector3 direction, out Vector3 right, out Vector3 up)
    {
        right = Vector3.Cross(direction, Vector3.up).normalized;
        if (right.sqrMagnitude <= 0.001f) right = Vector3.right;

        up = Vector3.Cross(right, direction).normalized;
    }

    private Vector3 GetRandomConeDirection(Vector3 direction, Vector3 right, Vector3 up, float spreadRadians)
    {
        float azimuth = Random.Range(0f, Mathf.PI * 2f);
        float cosAngle = Random.Range(Mathf.Cos(spreadRadians), 1f);
        float sinAngle = Mathf.Sqrt(1f - cosAngle * cosAngle);
        Vector3 spreadDirection = right * Mathf.Cos(azimuth) + up * Mathf.Sin(azimuth);

        return (direction * cosAngle + spreadDirection * sinAngle).normalized;
    }

    protected Vector3 GetFlatDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return transform.forward;

        return direction.normalized;
    }

    protected void RotateGunModel(Vector3 direction) { }

    public void SetPlayerAnimation()
    {
        if (gunModel == null)
        {
            Debug.LogWarning($"[WeaponRanged]: Chưa kéo Gun Model của khẩu {gameObject.name} vào Inspector.");
            return;
        }

        if (recoilCoroutine != null)
        {
            gunModel.localPosition = originalLocalPosition;
            gunModel.localRotation = originalLocalRotation;
            StopCoroutine(recoilCoroutine);
        }

        recoilCoroutine = StartCoroutine(RecoilRoutine());
    }

    private IEnumerator RecoilRoutine()
    {
        yield return WeaponRecoilRoutine.Play(
            gunModel,
            originalLocalPosition,
            originalLocalRotation,
            recoilPositionOffset,
            recoilRotationOffset,
            restoreSpeed);
        recoilCoroutine = null;
    }
}