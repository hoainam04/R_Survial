# Sơ Đồ Cấu Trúc Thư Mục Scripts (`Assets/Scripts`)

Sơ đồ trực quan biểu diễn cấu trúc cây thư mục của toàn bộ mã nguồn trong dự án **R_Survival**.

```text
Assets/Scripts/
├── Core/                               # Hệ thống cốt lõi dùng chung
│   ├── Camera/
│   │   └── CameraFollow.cs             # Điều khiển camera bám theo mục tiêu
│   ├── Data/
│   │   ├── AnimationData.cs            # Dữ liệu/Tham số animation phụ
│   │   └── AnimationDataSO.cs          # ScriptableObject lưu thông số Animator Hash
│   ├── Lifecycle/
│   │   └── DestroyTimer.cs             # Tự động hủy GameObject sau thời gian định mức
│   ├── Patterns/                       # Các pattern dùng chung toàn dự án
│   │   ├── GameEventChannel.cs         # Event Channel Pattern (SO) cho giao tiếp Gameplay <-> UI
│   │   ├── GenericObjectPool.cs        # Object Pool tổng quát cho đạn, hiệu ứng, kẻ địch
│   │   └── IDamageCalculationStrategy.cs # Strategy Pattern tính toán sát thương (thường/chí mạng)
│   ├── Settings/
│   │   ├── GameSettingsData.cs         # Dữ liệu Keybind/Audio/Graphics (Serializable)
│   │   └── GameSettingsManager.cs      # Singleton quản lý & lưu/tải settings qua JSON
│   └── Storage/
│       └── JsonStorage.cs              # Hỗ trợ lưu trữ dữ liệu định dạng JSON
│
├── UI/                                  # Presentation layer, lắng nghe Event từ Gameplay
│   ├── PlayerStatusUI.cs               # Hiển thị HP/Stamina, spawn Damage Popup
│   ├── DamagePopup.cs                  # Số damage nổi lên và mờ dần
│   ├── WorldSpaceUIBillboard.cs        # Xoay Canvas World Space luôn hướng về Camera
│   ├── Common/
│   │   └── SafeArea.cs                 # Co giãn UI theo vùng an toàn (notch) màn hình mobile
│   ├── Equipment/
│   │   ├── EquipmentSlotUI.cs          # Widget 1 ô trang bị: Helmet/Armor/Boots/Gloves/Backpack HOẶC Wep chính/phụ (chế độ loadout), nhận thả (drop) để Equip
│   │   └── EquipmentPanelUI.cs         # Toàn bộ ô trang bị (bao gồm 2 ô vũ khí), lắng nghe OnEquipmentChanged/OnWeaponLoadoutChanged
│   └── Inventory/
│       ├── InventorySlotUI.cs          # Widget 1 ô Inventory, hỗ trợ kéo (drag) sang Hotbar/Equipment, click để chọn xem mô tả
│       ├── InventoryPanelUI.cs         # Lưới slot động theo Inventory.TotalSlotCount, bật/tắt bằng phím I
│       ├── HotbarSlotUI.cs             # Widget 1 ô Hotbar, chỉ nhận thả (drop) Consumable/Throwable
│       ├── HotbarUI.cs                 # 5 ô Hotbar cố định (chỉ Consumable/Throwable), hiển thị icon/số lượng thật
│       ├── ItemSelectionEvents.cs      # Kênh sự kiện UI-only báo item đang được chọn cho ItemDescriptionUI
│       └── ItemDescriptionUI.cs        # Hiển thị icon/tên/mô tả/độ hiếm/cân nặng của item đang được chọn
│
└── Gameplay/                           # Logic gameplay và hệ thống nhân vật
    ├── Character/
    │   ├── Attributes/                 # Hệ thống chỉ số nhân vật (Máu, Thể lực,...)
    │   │   ├── AttributeType.cs        # Định nghĩa Enum loại chỉ số & Extension methods
    │   │   ├── CharacterAttribute.cs   # Lớp mô tả một chỉ số đơn lẻ (giá trị, min/max)
    │   │   ├── CharacterAttributeManager.cs # Quản lý tập hợp chỉ số của nhân vật
    │   │   └── CharacterAttributeSetSO.cs   # ScriptableObject cấu hình bộ chỉ số mẫu
    │   │
    │   ├── Combat/                     # Hệ thống chiến đấu & tương tác va chạm
    │   │   └── HitBox/
    │   │       └── MeleeHitBox.cs      # Xử lý vùng gây sát thương cận chiến
    │   │
    │   ├── Controller/                 # Điều khiển nhân vật & Finite State Machine
    │   │   ├── CharacterControllerBrain.cs # Bộ não điều khiển state machine chính
    │   │   ├── CharacterInputHandler.cs # Đọc input người chơi (PC/Mobile), hỗ trợ auto-attack
    │   │   ├── CharacterAimingHandler.cs # Xử lý hướng nhắm (chuột PC / auto-aim mobile)
    │   │   ├── Common/
    │   │   │   └── CharacterStaminaRegen.cs # Hồi phục thể lực thông thường
    │   │   ├── Gravity/
    │   │   │   └── CharacterGravity.cs # Xử lý trọng lực tác động lên nhân vật
    │   │   ├── Movements/
    │   │   │   ├── CharacterAttack.cs  # Điều phối hành động tấn công
    │   │   │   ├── CharacterDash.cs    # Xử lý lướt nhân vật
    │   │   │   ├── CharacterJump.cs    # Xử lý nhảy
    │   │   │   └── CharacterMovement.cs # Xử lý di chuyển & hướng nhìn cơ bản
    │   │   └── StateMachine/           # Các trạng thái của nhân vật
    │   │       ├── DashState.cs        # Trạng thái lướt
    │   │       ├── ICharacterState.cs  # Interface chuẩn cho mọi State
    │   │       ├── IdleState.cs        # Trạng thái đứng yên
    │   │       ├── MeleeAttackState.cs # Trạng thái tấn công cận chiến
    │   │       ├── MoveState.cs        # Trạng thái di chuyển (Đi bộ/Chạy)
    │   │       ├── RangedAttackState.cs# Trạng thái tấn công tầm xa
    │   │       ├── ReloadState.cs      # Trạng thái nạp đạn
    │   │       └── RunState.cs         # Trạng thái chạy nhanh
    │   │
    │   ├── Enemy/                      # Logic kẻ địch
    │   │   └── EnemyHealth.cs          # Quản lý máu và sát thương của kẻ địch
    │   │
    │   ├── Equipment/                  # Hệ thống trang bị, vũ khí, vật phẩm & ScriptableObjects
    │   │   ├── AmmoSO.cs               # Định nghĩa item đạn dược
    │   │   ├── CharacterEquipmentManager.cs # Quản lý trang bị hiện tại của nhân vật
    │   │   ├── ConsumableSO.cs         # Định nghĩa item tiêu hao (thức ăn, nước, thuốc)
    │   │   ├── CraftingMaterial.cs     # Định nghĩa nguyên liệu chế tạo
    │   │   ├── EquipmentHolder.cs      # Gắn mô hình 3D vũ khí/trang bị lên nhân vật
    │   │   ├── EquipmentItemSO.cs      # Lớp cơ sở cho vật phẩm có thể trang bị
    │   │   ├── WeaponEquipmentSO.cs    # Lớp cơ sở chung cho vũ khí (kế thừa EquipmentItemSO)
    │   │   ├── ItemSO.cs               # Lớp cơ sở gốc cho mọi vật phẩm trong game
    │   │   ├── QuestItemSO.cs          # Định nghĩa vật phẩm nhiệm vụ
    │   │   ├── ThrowableSO.cs          # Định nghĩa vật phẩm ném (lựu đạn, bom khói)
    │   │   ├── Debug/
    │   │   │   └── EquipmentTestController.cs # Test nhanh hệ thống trang bị
    │   │   ├── Gear/                   # Trang phục, giáp, phụ kiện
    │   │   │   ├── ArmorSO.cs          # Giáp thân
    │   │   │   ├── BackPackSO.cs       # Ba lô mở rộng kho đồ
    │   │   │   ├── BootsSO.cs          # Giày
    │   │   │   ├── GlovesSO.cs         # Găng tay
    │   │   │   └── HelmetSO.cs         # Mũ bảo hiểm
    │   │   └── Weapon/                 # Vũ khí và đạn dược
    │   │       ├── GunHitscan.cs       # Súng bắn hitscan (tức thì)
    │   │       ├── GunProjectile.cs    # Súng bắn đạn bay (projectile)
    │   │       ├── WeaponRanged.cs     # Lớp trừu tượng chung cho súng
    │   │       ├── Data/
    │   │       │   ├── WeaponMeleeSO.cs  # ScriptableObject dữ liệu vũ khí cận chiến
    │   │       │   └── WeaponRangedSO.cs # ScriptableObject dữ liệu súng
    │   │       └── Projectile/
    │   │           └── ProjectileBullet.cs # Xử lý bay, va chạm của viên đạn
    │   │
    │   ├── Inventory/                  # Hệ thống kho đồ (Inventory)
    │   │   ├── Inventory.cs            # Quản lý danh sách ô chứa, thêm/xóa item
    │   │   ├── InventorySlot.cs        # Cấu trúc dữ liệu của một ô chứa
    │   │   ├── ItemStack.cs            # Cấu trúc lưu trữ Item kèm số lượng
    │   │   ├── StorageBox.cs           # Kho chứa đồ tại căn cứ (Safehouse Stash)
    │   │   ├── Hotbar.cs               # 5 ô dùng nhanh Consumable/Throwable, đọc phím slot1-5 (mặc định phím 3-7)
    │   │   ├── PlayerItemPicker.cs     # Tương tác nhặt item dưới đất
    │   │   └── WorldItem.cs            # Đại diện item nằm trong thế giới 3D
    │   │
    │   ├── Player/                     # Logic dành riêng cho người chơi
    │   │   └── PlayerHealth.cs         # Quản lý máu và trạng thái sinh tồn người chơi
    │   │
    │   ├── Status/                     # Chỉ số sinh tồn & module định kỳ
    │   │   ├── CharacterStatusController.cs # Bộ điều khiển trung tâm (Đói, Khát, Thể lực)
    │   │   ├── HealthRegenModule.cs    # Module tính toán hồi máu
    │   │   ├── RigidbodyGravityModule.cs# Module trọng lực qua Rigidbody
    │   │   ├── StaminaRegenModule.cs   # Module tính toán hồi thể lực
    │   │   └── SurvivalDrainModule.cs  # Module tính toán hao hụt chỉ số sinh tồn theo thời gian
    │   │
    │   └── StatusEffects/              # Hiệu ứng trạng thái (Buff/Debuff)
    │       ├── BurnEffect.cs           # Hiệu ứng cháy (DoT)
    │       ├── CharacterEffectController.cs # Quản lý danh sách hiệu ứng trên nhân vật
    │       ├── PoisonEffect.cs         # Hiệu ứng nhiễm độc
    │       ├── SlowEffect.cs           # Hiệu ứng giảm tốc độ di chuyển
    │       ├── StatusEffect.cs         # Lớp cơ sở trừu tượng cho hiệu ứng
    │       └── StunEffect.cs           # Hiệu ứng làm choáng
```
