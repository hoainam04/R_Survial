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
│   └── Storage/
│       └── JsonStorage.cs              # Hỗ trợ lưu trữ dữ liệu định dạng JSON
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
