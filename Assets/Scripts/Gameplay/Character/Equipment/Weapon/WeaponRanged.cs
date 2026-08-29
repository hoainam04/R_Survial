using UnityEngine;
using System.Collections;
using PROJ.Equipment;

public abstract class WeaponRanged : MonoBehaviour
{
    #region Inspector

    [Header("Cấu Hình Băng Đạn & Khai Hỏa")]
    public WeaponRangedSO weaponData;
    [SerializeField] protected Transform muzzlePoint;

    [Header("Model Animation")]
    [SerializeField] private Transform gunModel;
    [SerializeField] private float restoreSpeed = 15f;

    [Header("Model Rotation")]
    // [SerializeField] private float modelRotationOffsetZ = 0f;

    [Header("Visual Recoil")]
    [SerializeField] private Vector3 recoilPositionOffset = new Vector3(0f, -0.15f, 0f);
    [SerializeField] private Vector3 recoilRotationOffset = new Vector3(1f, 0f, 0f);

    #endregion

    #region Runtime Data

    protected int currentAmmo;
    public bool IsReloading { get; protected set; }

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Coroutine recoilCoroutine;

    private float currentSpread;
    private int recoilStep;
    private int horizontalDirection = 1;
    private int verticalDirection = 1;
    #endregion

    #region Unity Events

    protected virtual void Start()
    {
        if (weaponData != null)
        {
            currentAmmo = weaponData.maxAmmo;
            currentSpread = weaponData.minSpread;
        }

        if (gunModel != null)
        {
            originalLocalPosition = gunModel.localPosition;
            originalLocalRotation = gunModel.localRotation;
        }
    }

    protected virtual void Update()
    {
        RecoverSpread();
    }

    #endregion

    #region Public API

    public bool CanFire()
    {
        return weaponData != null && currentAmmo > 0 && !IsReloading;
    }

    public abstract void Fire(LayerMask enemyLayers, Vector3 direction);

    public void Reload()
    {
        if (weaponData == null) return;
        if (IsReloading || currentAmmo == weaponData.maxAmmo) return;

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

    #endregion

    #region Reload

    private IEnumerator ReloadCoroutine()
    {
        IsReloading = true;
        Debug.Log($"[Súng]: Đang nạp đạn... Chờ {weaponData.reloadTime}s");

        yield return new WaitForSeconds(weaponData.reloadTime);

        currentAmmo = weaponData.maxAmmo;
        IsReloading = false;

        Debug.Log("[Súng]: Đã nạp đầy băng đạn!");
    }

    #endregion

    #region Progressive Recoil

    protected void AddShotRecoil()
    {
        if (weaponData == null) return;

        if (recoilStep == 0)
        {
            horizontalDirection = UnityEngine.Random.value < 0.5f ? -1 : 1;
            verticalDirection = UnityEngine.Random.value < 0.5f ? -1 : 1;
        }

        recoilStep++;

        currentSpread += weaponData.spreadIncreasePerShot;
        currentSpread = Mathf.Clamp(currentSpread, weaponData.minSpread, weaponData.maxSpread);
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

        Vector3 sideAxis = new Vector3(-direction.z, 0f, direction.x).normalized;

        float verticalWave = Mathf.Sin(recoilStep * 0.45f);
        float horizontalWave = Mathf.Sin(recoilStep * 0.75f);

        float verticalOffset = verticalWave * weaponData.verticalRecoil * currentSpread * verticalDirection;
        float horizontalOffset = horizontalWave * weaponData.horizontalRecoil * currentSpread * horizontalDirection;

        Vector3 finalDirection =
            direction +
            sideAxis * horizontalOffset +
            Vector3.up * verticalOffset;

        return finalDirection.normalized;
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

    protected float ResolveDamage(float fallback)
    {
        return weaponData != null ? weaponData.damage : fallback;
    }

    protected float ResolveMaxRange(float fallback)
    {
        return weaponData != null ? weaponData.attackRange : fallback;
    }

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

    protected int ResolvePelletCount()
    {
        return weaponData != null ? weaponData.ResolvePelletCount() : 1;
    }

    protected Vector3 GetPelletDirection(Vector3 direction, int pelletIndex, int pelletCount)
    {
        Vector3 baseDirection = ApplyBulletRecoil(direction);

        if (weaponData == null || weaponData.gunType != GunType.Shotgun || pelletCount <= 1)
        {
            return baseDirection;
        }

        float spreadAngle = weaponData.ResolveSpreadConeAngle();
        float spreadRadians = spreadAngle * Mathf.Deg2Rad;
        float normalizedIndex = pelletIndex / Mathf.Max(1f, pelletCount - 1f);
        float angle = normalizedIndex * Mathf.PI * 2f;

        Vector3 right = Vector3.Cross(baseDirection, Vector3.up).normalized;
        if (right.sqrMagnitude <= 0.001f)
        {
            right = Vector3.right;
        }

        Vector3 up = Vector3.Cross(right, baseDirection).normalized;
        Vector3 spreadOffset = right * Mathf.Cos(angle) * spreadRadians + up * Mathf.Sin(angle) * spreadRadians;

        return (baseDirection + spreadOffset).normalized;
    }

    #endregion

    #region Direction Utility

    protected Vector3 GetFlatDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return transform.forward;

        return direction.normalized;
    }

    #endregion

    #region Model Rotation

    protected void RotateGunModel(Vector3 direction)
    {
        if (gunModel == null) return;

        // Không ghi đè rotation gốc bằng state hiện tại của model.
        // originalLocalRotation phải giữ giá trị mặc định ban đầu của gunModel
        // để recoil luôn reset về đúng pose chuẩn, không cộng dồn qua từng shot.
    }

    #endregion

    #region Visual Recoil

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
        Vector3 targetRecoilPos = originalLocalPosition + recoilPositionOffset;
        Quaternion targetRecoilRot = originalLocalRotation * Quaternion.Euler(recoilRotationOffset);

        gunModel.localPosition = targetRecoilPos;
        gunModel.localRotation = targetRecoilRot;

        while (Vector3.Distance(gunModel.localPosition, originalLocalPosition) > 0.001f ||
               Quaternion.Angle(gunModel.localRotation, originalLocalRotation) > 0.1f)
        {
            gunModel.localPosition = Vector3.Lerp(
                gunModel.localPosition,
                originalLocalPosition,
                Time.deltaTime * restoreSpeed
            );

            gunModel.localRotation = Quaternion.Slerp(
                gunModel.localRotation,
                originalLocalRotation,
                Time.deltaTime * restoreSpeed
            );

            yield return null;
        }

        gunModel.localPosition = originalLocalPosition;
        gunModel.localRotation = originalLocalRotation;
        recoilCoroutine = null;
    }

    #endregion
}