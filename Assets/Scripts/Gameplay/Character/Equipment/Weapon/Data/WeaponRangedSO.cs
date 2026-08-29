using UnityEngine;

namespace PROJ.Equipment
{
    public enum BulletType
    {
        RaycastHit,
        Projectitle
    }

    public enum GunType
    {
        Pistol,
        SMG,
        Rifle,
        LMG,
        Shotgun,
        Sniper,
        GrenadeLauncher,
        RocketLauncher,
    }

    [CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "PROJ/Equipment/Weapon - Ranged")]
    public class WeaponRangedSO : WeaponEquipmentSO
    {
        public override bool isStackable => false;
        public override EquipmentSlot slotType => EquipmentSlot.Weapon;
        public override WeaponType weaponType => WeaponType.Ranged;

        [Header("Đặc Tính Súng Ống")]
        public BulletType bulletType;
        public GunType gunType;

        public int maxAmmo = 30;
        public float reloadTime = 1.5f;
        public float fireRate = 0.2f;

        [Header("Shotgun / Multi Pellet")]
        public int pelletCount = 1;
        public bool useAutoShotgunPellets = true;

        [Header("Spread & Accuracy")]
        public float spreadConeAngle = 18f;
        public float minSpread = 0.01f;
        public float maxSpread = 0.35f;
        public float spreadIncreasePerShot = 0.04f;
        public float spreadRecoverySpeed = 0.8f;

        [Header("Legacy Spread")]
        [Tooltip("Giữ lại để không hỏng asset cũ. Nếu dùng recoil mới thì có thể để 0.")]
        public float recoilForce = 0.1f;

        [Header("Recoil Direction")]
        public float horizontalRecoil = 1f;
        public float verticalRecoil = 0.6f;

        [Header("Projectile / Tracer")]
        public float bulletSpeed = 30f;
        public GameObject projectilePrefab;
        public GameObject tracerPrefab;
        public float tracerDuration = 0.05f;

        public int ResolvePelletCount()
        {
            int pellets = Mathf.Max(1, pelletCount);
            if (gunType == GunType.Shotgun && useAutoShotgunPellets && pellets <= 1)
            {
                pellets = 8;
            }

            return pellets;
        }

        public float ResolveSpreadConeAngle()
        {
            if (spreadConeAngle <= 0f)
            {
                return Mathf.Clamp(maxSpread * 45f, 8f, 22f);
            }

            return Mathf.Clamp(spreadConeAngle, 8f, 22f);
        }
    }
}