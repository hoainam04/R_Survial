using UnityEngine;
using PROJ.Attributes; 
using PROJ.Equipment;

public class CharacterControllerBrain : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] public AnimationDataSO animationData;
    public Animator Animator => animator;

    [Header("Cấu Hình Trễ Xoay Khi Thôi Tấn Công (Aim Linger)")]
    [Tooltip("Thời gian duy trì hướng ngắm sau khi dừng bắn (giây). Quá thời gian này mới cho xoay theo hướng di chuyển.")]
    [SerializeField] private float aimLingerDuration = 0.6f; 
    private float aimLingerTimer = 0f;

    // --- Các Component Xử Lý Đã Tách Nhỏ ---
    public CharacterInputHandler InputHandler { get; private set; }
    public CharacterAimingHandler AimingHandler { get; private set; }

    // --- Các Component Chức Năng (Capabilities) ---
    public CharacterMovement Movement { get; private set; }
    public CharacterDash Dash { get; private set; }
    public CharacterAttack Attack { get; private set; }

    // --- Cổng kết nối dữ liệu và vật lý phục vụ các State ---
    public CharacterAttributeManager AttributeManager { get; private set; }
    public CharacterEquipmentManager EquipmentManager { get; private set; }
    public Rigidbody Rb { get; private set; }

    // --- Thuộc tính dữ liệu Input công khai cho các State sử dụng ---
    public Vector3 InputDirection { get; private set; }
    
    // Biến ghi nhớ trạng thái muốn chạy (Toggle) của người chơi
    public bool IsSprintingToggle { get; set; } 

    // --- Quản lý State Machine ---
    private ICharacterState currentState;
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public RunState RunState { get; private set; }
    public DashState DashState { get; private set; }
    
    // --- Các State Tấn công và Thay đạn ---
    public MeleeAttackState MeleeAttackState { get; private set; }
    public RangedAttackState RangedAttackState { get; private set; }
    public ReloadState ReloadState { get; private set; }

    // --- BIẾN ĐỆM LƯU TRỮ HƯỚNG XOAY (CHỐNG GIẬT LẮC) ---
    private Vector3 calculatedAimDirection = Vector3.forward;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animationData == null)
        {
            Debug.LogWarning($"AnimationDataSO chưa được gán trên {gameObject.name}. Vui lòng gán trong Inspector.");
        }

        Rb = GetComponent<Rigidbody>();
        AttributeManager = GetComponent<CharacterAttributeManager>();
        EquipmentManager = GetComponent<CharacterEquipmentManager>() ?? gameObject.AddComponent<CharacterEquipmentManager>();

        Movement = GetComponent<CharacterMovement>();
        Dash = GetComponent<CharacterDash>() ?? gameObject.AddComponent<CharacterDash>();
        Attack = GetComponent<CharacterAttack>() ?? gameObject.AddComponent<CharacterAttack>();

        InputHandler = GetComponent<CharacterInputHandler>() ?? gameObject.AddComponent<CharacterInputHandler>();
        AimingHandler = GetComponent<CharacterAimingHandler>() ?? gameObject.AddComponent<CharacterAimingHandler>();

        // Khởi tạo các State
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        RunState = new RunState(this);
        DashState = new DashState(this);
        
        MeleeAttackState = new MeleeAttackState(this);
        RangedAttackState = new RangedAttackState(this);
        ReloadState = new ReloadState(this);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        HandleInput();

        // 🌟 BƯỚC 1: TÍNH TOÁN HƯỚNG BẮN Ở UPDATE
        if (IsAiming)
        {
            calculatedAimDirection = GetAimDirection();
        }

        currentState?.UpdateState();
    }

    private void FixedUpdate()
    {
        // 🌟 BƯỚC 2: THỰC THI XOAY RIGIDBODY Ở FIXEDUPDATE
        HandleCharacterRotation();

        currentState?.FixedUpdateState();
    }

    private void HandleInput()
    {
        InputDirection = InputHandler.GetMovementInput();

        if (currentState == DashState || currentState == ReloadState) return;

        // 1. KIỂM TRA DASH (Ưu tiên cao nhất)
        if (InputHandler.GetDashInput() && Dash.CanDash())
        {
            ChangeState(DashState);
            return;
        }

        // 2. KIỂM TRA CHỦ ĐỘNG NẠP ĐẠN
        if (InputHandler.GetReloadInput())
        {
            if (EquipmentManager != null && EquipmentManager.CurrentWeaponType == WeaponType.Ranged)
            {
                var activeGun = EquipmentManager.CurrentWeaponVisualObject?.GetComponent<WeaponRanged>();
                
                if (activeGun != null)
                {
                    int curAmmo = activeGun.CurrentAmmo;
                    int maxAmmo = activeGun.MaxAmmo;
                    
                    if (curAmmo < maxAmmo)
                    {
                        ChangeState(ReloadState);
                        return;
                    }
                }
            }
        }

        // 3. KIỂM TRA TẤN CÔNG
        if (InputHandler.GetAttackInput() && Attack.CanAttack())
        {
            if (EquipmentManager != null && EquipmentManager.CurrentWeaponType == WeaponType.Ranged)
            {
                ChangeState(RangedAttackState);
            }
            else
            {
                ChangeState(MeleeAttackState);
            }
            return;
        }
    }

    public bool GetDashInput() => InputHandler != null ? InputHandler.GetDashInput() : Input.GetKeyDown(KeyCode.Space);
    public bool GetSprintInput() => InputHandler != null ? InputHandler.GetSprintInput() : Input.GetKey(KeyCode.LeftShift);
    public bool GetAttackInput() => InputHandler != null ? InputHandler.GetAttackInput() : Input.GetMouseButton(0);
    public bool GetReloadInput() => InputHandler != null ? InputHandler.GetReloadInput() : Input.GetKeyDown(KeyCode.R);

    // Thuộc tính kiểm tra xem nhân vật có đang thực hiện hành vi ngắm/bắn hay không
    public bool IsAiming => GetAttackInput() || currentState == RangedAttackState || currentState == MeleeAttackState;

    /// <summary>
    /// Hàm ủy quyền tính toán hướng ngắm cho AimingHandler
    /// </summary>
    public Vector3 GetAimDirection()
    {
        if (AimingHandler != null)
        {
            return AimingHandler.CalculateAimDirection(transform);
        }
        return transform.forward;
    }

    /// <summary>
    /// Bộ xử lý phân cấp hướng xoay thông minh và tích hợp bộ đếm duy trì hướng ngắm tránh giật lắc khi tap đơn
    /// </summary>
    private void HandleCharacterRotation()
    {
        // Khi đang lướt (Dash), hãy để DashState tự xử lý hướng lao đi, không can thiệp xoay ở đây
        if (currentState == DashState) return;

        // 1. QUẢN LÝ BỘ ĐẾM THỜI GIAN DUY TRÌ (LINGER TIMER)
        if (IsAiming && currentState != ReloadState)
        {
            // Nếu đang giữ chuột bắn HOẶC đang trong State tấn công -> Giữ timer liên tục ở mức tối đa
            aimLingerTimer = aimLingerDuration;
        }
        else
        {
            // Ngay khi người chơi ngừng click/hết đòn đánh -> Đếm ngược thời gian duy trì hướng nhìn cũ
            if (aimLingerTimer > 0f)
            {
                aimLingerTimer -= Time.fixedDeltaTime;
            }
        }

        // 2. PHÂN CẤP QUYẾT ĐỊNH HƯỚNG XOAY
        // ƯU TIÊN 1: Nếu đang Aim bắn HOẶC đang trong thời gian duy trì hướng ngắm (Timer chưa về 0)
        if ((IsAiming || aimLingerTimer > 0f) && currentState != ReloadState)
        {
            RotateTowards(calculatedAimDirection);
        }
        // ƯU TIÊN 2: Chỉ khi đã hết sạch thời gian trễ dừng bắn + có bấm phím di chuyển -> Mới xoay theo hướng chạy WASD
        else if (InputDirection != Vector3.zero)
        {
            RotateTowards(InputDirection);
        }
    }

    /// <summary>
    /// Thực thi xoay mượt mà thông qua Rigidbody thay vì ép cứng Transform
    /// </summary>
    public void RotateTowards(Vector3 targetDirection)
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            
            // Sử dụng Time.fixedDeltaTime vì hàm này được gọi từ bên trong FixedUpdate
            Quaternion nextRotation = Quaternion.Slerp(Rb.rotation, targetRotation, Time.fixedDeltaTime * 18f);
            
            // Dùng MoveRotation để mượt cơ chế nội suy (Interpolation) của Rigidbody, triệt tiêu Jitter hoàn toàn
            Rb.MoveRotation(nextRotation);
        }
    }

    public void ChangeState(ICharacterState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public ICharacterState GetCurrentState() => currentState;
}
