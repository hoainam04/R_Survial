# Báo Cáo Chi Tiết Cấu Trúc Scripts Dự Án R_Survival

Dự án **R_Survival** sử dụng cấu trúc mã nguồn hướng đối tượng, mô-đun hóa rõ ràng cho các tính năng gameplay sinh tồn (Survival), điều khiển nhân vật, trang bị (Equipment), hệ thống chỉ số (Attributes), trạng thái hiệu ứng (Status Effects) và lưu trữ dữ liệu (Storage).

---

## 1. Tổng Quan Cấu Trúc Thư Mục `Assets/Scripts`

Thư mục `Assets/Scripts` được chia thành 2 nhánh chính:
- **`Core/`**: Chứa các hệ thống cốt lõi dùng chung (Camera, Data definitions, Lifecycle helpers, Storage).
- **`Gameplay/`**: Chứa toàn bộ logic game, tập trung chủ yếu vào Nhân vật (`Character`), bao gồm Character Controller, State Machine, Attributes, Equipment, Inventory, Status Effects và Weaponry.

---

## 2. Chi Tiết Các Module & Script

### A. Thư mục `Core/` (Hệ Thống Cốt Lõi)
- **`Camera/`**:
  - [`CameraFollow.cs`](../Assets/Scripts/Core/Camera/CameraFollow.cs): Xử lý camera bám theo mục tiêu.
- **`Data/`**:
  - [`AnimationData.cs`](../Assets/Scripts/Core/Data/AnimationData.cs): (Tuỳ chọn/Mở rộng) Dữ liệu animation.
  - [`AnimationDataSO.cs`](../Assets/Scripts/Core/Data/AnimationDataSO.cs): ScriptableObject chứa thiết lập định danh chuỗi/hash cho Animator parameters.
- **`Lifecycle/`**:
  - [`DestroyTimer.cs`](../Assets/Scripts/Core/Lifecycle/DestroyTimer.cs): Tự động hủy GameObject sau một khoảng thời gian (dùng cho đạn, hiệu ứng tạm thời).
- **`Storage/`**:
  - [`JsonStorage.cs`](../Assets/Scripts/Core/Storage/JsonStorage.cs): Thư viện/Tiện ích hỗ trợ lưu trữ và tải dữ liệu định dạng JSON.

---

### B. Thư mục `Gameplay/Character/` (Hệ Thống Nhân Vật & Gameplay)

#### 1. Attributes (`Attributes/`)
Quản lý các chỉ số của nhân vật (Máu, Thể lực, Đói, Khát, v.v.):
- [`AttributeType.cs`](../Assets/Scripts/Gameplay/Character/Attributes/AttributeType.cs): Định nghĩa kiểu chỉ số (Enum) và các hàm mở rộng (Extensions).
- [`CharacterAttribute.cs`](../Assets/Scripts/Gameplay/Character/Attributes/CharacterAttribute.cs): Lớp dữ liệu cho một chỉ số đơn lẻ (Giá trị hiện tại, tối đa, min/max).
- [`CharacterAttributeManager.cs`](../Assets/Scripts/Gameplay/Character/Attributes/CharacterAttributeManager.cs): Quản lý tập hợp các `CharacterAttribute` của nhân vật.
- [`CharacterAttributeSetSO.cs`](../Assets/Scripts/Gameplay/Character/Attributes/CharacterAttributeSetSO.cs): ScriptableObject cấu hình bộ chỉ số mẫu ban đầu.

#### 2. Combat (`Combat/HitBox/`)
- [`MeleeHitBox.cs`](../Assets/Scripts/Gameplay/Character/Combat/HitBox/MeleeHitBox.cs): Xử lý va chạm cận chiến cho vũ khí/nhân vật.

#### 3. Controller & State Machine (`Controller/`)
Hệ thống điều khiển nhân vật theo mô hình State Machine rõ ràng:
- **Common**:
  - [`CharacterStaminaRegen.cs`](../Assets/Scripts/Gameplay/Character/Controller/Common/CharacterStaminaRegen.cs): Hồi phục thể lực theo thời gian.
- **Gravity**:
  - [`CharacterGravity.cs`](../Assets/Scripts/Gameplay/Character/Controller/Gravity/CharacterGravity.cs): Xử lý trọng lực tác động lên nhân vật.
- **Movements**:
  - [`CharacterMovement.cs`](../Assets/Scripts/Gameplay/Character/Controller/Movements/CharacterMovement.cs): Điều khiển di chuyển cơ bản (Di chuyển, hướng nhìn).
  - [`CharacterJump.cs`](../Assets/Scripts/Gameplay/Character/Controller/Movements/CharacterJump.cs): Xử lý nhảy.
  - [`CharacterDash.cs`](../Assets/Scripts/Gameplay/Character/Controller/Movements/CharacterDash.cs): Xử lý lướt (Dash).
  - [`CharacterAttack.cs`](../Assets/Scripts/Gameplay/Character/Controller/Movements/CharacterAttack.cs): Điều phối tấn công chung.
- **StateMachine**:
  - [`ICharacterState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/ICharacterState.cs): Giao diện (Interface) chung cho mọi trạng thái nhân vật.
  - [`CharacterControllerBrain.cs`](../Assets/Scripts/Gameplay/Character/Controller/CharacterControllerBrain.cs): Bộ não điều khiển state machine chính.
  - Các trạng thái cụ thể:
    - [`IdleState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/IdleState.cs)
    - [`MoveState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/MoveState.cs)
    - [`RunState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/RunState.cs)
    - [`DashState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/DashState.cs)
    - [`MeleeAttackState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/MeleeAttackState.cs)
    - [`RangedAttackState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/RangedAttackState.cs)
    - [`ReloadState.cs`](../Assets/Scripts/Gameplay/Character/Controller/StateMachine/ReloadState.cs)

#### 4. Enemy (`Enemy/`)
- [`EnemyHealth.cs`](../Assets/Scripts/Gameplay/Character/Enemy/EnemyHealth.cs): Quản lý máu và sát thương nhận vào của kẻ địch.

#### 5. Equipment & Items (`Equipment/`)
Quản lý trang bị, vũ khí, vật phẩm và các loại ScriptableObject định nghĩa item:
- **Item Base**:
  - [`ItemSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/ItemSO.cs): Lớp cơ sở ScriptableObject cho mọi vật phẩm (Loại item, độ hiếm, icon, tên, mô tả).
  - [`EquipmentItemSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/EquipmentItemSO.cs): Lớp cơ sở cho các vật phẩm có thể trang bị (Theo Slot).
- **Cụ thể Item SO**:
  - [`AmmoSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/AmmoSO.cs): Đạn dược.
  - [`ConsumableSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/ConsumableSO.cs): Vật phẩm tiêu hao (Thức ăn, nước uống, thuốc).
  - [`CraftingMaterial.cs`](../Assets/Scripts/Gameplay/Character/Equipment/CraftingMaterial.cs): Nguyên liệu chế tạo.
  - [`QuestItemSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/QuestItemSO.cs): Vật phẩm nhiệm vụ.
  - [`ThrowableSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/ThrowableSO.cs): Vật phẩm ném (Lựu đạn, bom khói).
- **Gear (Trang phục/Giáp)**:
  - [`ArmorSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Gear/ArmorSO.cs): Giáp thân.
  - [`BackPackSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Gear/BackPackSO.cs): Ba lô (mở rộng kho đồ).
  - [`BootsSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Gear/BootsSO.cs): Giày.
  - [`GlovesSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Gear/GlovesSO.cs): Găng tay.
  - [`HelmetSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Gear/HelmetSO.cs): Mũ bảo hiểm.
- **Weapon (Vũ khí)**:
  - [`WeaponEquipmentSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/WeaponEquipmentSO.cs): Lớp cơ sở cho vũ khí.
  - `Data/`:
    - [`WeaponMeleeSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/Data/WeaponMeleeSO.cs): Dữ liệu vũ khí cận chiến.
    - [`WeaponRangedSO.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/Data/WeaponRangedSO.cs): Dữ liệu vũ khí tầm xa (Súng).
  - [`WeaponRanged.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/WeaponRanged.cs): Component xử lý logic súng chung.
  - [`GunHitscan.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/GunHitscan.cs): Súng bắn dạng hitscan (bắn tức thì).
  - [`GunProjectile.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/GunProjectile.cs): Súng bắn đạn bay (projectile).
  - `Projectile/`:
    - [`ProjectileBullet.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Weapon/Projectile/ProjectileBullet.cs): Xử lý viên đạn bay và chạm.
- **Manager & Holder**:
  - [`CharacterEquipmentManager.cs`](../Assets/Scripts/Gameplay/Character/Equipment/CharacterEquipmentManager.cs): Quản lý các trang bị nhân vật đang mặc/cầm.
  - [`EquipmentHolder.cs`](../Assets/Scripts/Gameplay/Character/Equipment/EquipmentHolder.cs): Gắn kết mô hình 3D vũ khí/trang bị lên nhân vật.
  - `Debug/`:
    - [`EquipmentTestController.cs`](../Assets/Scripts/Gameplay/Character/Equipment/Debug/EquipmentTestController.cs): Script kiểm thử trang bị nhanh.

#### 6. Inventory (`Inventory/`)
- [`Inventory.cs`](../Assets/Scripts/Gameplay/Character/Inventory/Inventory.cs): Quản lý danh sách các ô chứa item, thêm/bớt item.
- [`InventorySlot.cs`](../Assets/Scripts/Gameplay/Character/Inventory/InventorySlot.cs): Dữ liệu của một ô chứa trong kho.
- [`ItemStack.cs`](../Assets/Scripts/Gameplay/Character/Inventory/ItemStack.cs): Cấu trúc lưu trữ Item và số lượng tương ứng.
- [`PlayerItemPicker.cs`](../Assets/Scripts/Gameplay/Character/Inventory/PlayerItemPicker.cs): Xử lý nhặt vật phẩm từ thế giới game.
- [`WorldItem.cs`](../Assets/Scripts/Gameplay/Character/Inventory/WorldItem.cs): Đại diện vật phẩm rơi trên mặt đất.

#### 7. Player Health (`Player/`)
- [`PlayerHealth.cs`](../Assets/Scripts/Gameplay/Character/Player/PlayerHealth.cs): Quản lý máu và trạng thái sinh tồn của người chơi.

#### 8. Status & Modules (`Status/`)
Quản lý các thông số sinh tồn và trạng thái định kỳ:
- [`CharacterStatusController.cs`](../Assets/Scripts/Gameplay/Character/Status/CharacterStatusController.cs): Bộ điều khiển trung tâm các chỉ số sinh tồn (Đói, Khát, Máu, Thể lực).
- Các module thành phần:
  - [`HealthRegenModule.cs`](../Assets/Scripts/Gameplay/Character/Status/HealthRegenModule.cs): Module hồi máu.
  - [`StaminaRegenModule.cs`](../Assets/Scripts/Gameplay/Character/Status/StaminaRegenModule.cs): Module hồi thể lực.
  - [`SurvivalDrainModule.cs`](../Assets/Scripts/Gameplay/Character/Status/SurvivalDrainModule.cs): Module trừ chỉ số sinh tồn theo thời gian (đói khát).
  - [`RigidbodyGravityModule.cs`](../Assets/Scripts/Gameplay/Character/Status/RigidbodyGravityModule.cs): Module trọng lực qua Rigidbody.

#### 9. Status Effects (`StatusEffects/`)
Hệ thống hiệu ứng trạng thái (buff/debuff):
- [`StatusEffect.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/StatusEffect.cs): Lớp trừu tượng cơ sở cho các hiệu ứng.
- [`CharacterEffectController.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/CharacterEffectController.cs): Quản lý danh sách hiệu ứng đang tác động lên nhân vật.
- Các hiệu ứng cụ thể:
  - [`BurnEffect.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/BurnEffect.cs): Hiệu ứng bỏng (gây sát thương theo thời gian).
  - [`PoisonEffect.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/PoisonEffect.cs): Hiệu ứng độc.
  - [`SlowEffect.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/SlowEffect.cs): Hiệu ứng làm chậm tốc độ.
  - [`StunEffect.cs`](../Assets/Scripts/Gameplay/Character/StatusEffects/StunEffect.cs): Hiệu ứng choáng (ngắt hành động).
