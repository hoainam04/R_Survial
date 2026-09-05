# 📋 Ghi Chú Công Việc & Hướng Dẫn Refactor Theo Rules Mới (Current Task & Refactor Notes) - R_Survival

Tài liệu này ghi nhận các điểm cần lưu ý và kế hoạch refactor các scripts hiện tại dựa trên bộ [`Docs/ScriptingRules.md`](ScriptingRules.md) vừa được nâng cấp (Bao gồm kiểm soát ScriptableObject runtime, quy định Event UI một chiều, và nguyên lý chống Over-engineering).

---

## ✅ Đã Hoàn Thành (Cập nhật lại theo code hiện tại)
- **`StorageBox` (Kho căn cứ)**: Đã code xong (`Gameplay/Character/Inventory/StorageBox.cs`), logic thêm/xóa/swap item giống `Inventory`, phát `OnStorageChanged` — không gọi UI trực tiếp.
- **Event Channel Pattern**: Đã có `Core/Patterns/GameEventChannel.cs` (ScriptableObject-based) dùng cho giao tiếp toàn cục. Các hệ thống gameplay (`CharacterAttributeManager`, `Inventory`, `StorageBox`) đang dùng C# event trực tiếp (`OnValueChanged`, `OnInventoryChanged`, `OnStorageChanged`) — `PlayerStatusUI.cs` đã subscribe/unsubscribe đúng chuẩn (`OnDestroy`).
- **Equipment System**: `CharacterEquipmentManager` đã xử lý equip/unequip, cộng trừ attribute, cộng slot kho đồ khi mặc balo, spawn/destroy model 3D.
- **UI Kho Đồ & Hotbar**: Đã code xong phần data/UI script:
  - `ConsumableSO.UseConsumable(CharacterAttributeManager)` áp dụng hồi Máu/Thể lực/Đói/Khát thật qua `CharacterAttribute.Modify`.
  - `Hotbar.cs`: 5 ô dùng nhanh Consumable/Throwable (KHÔNG phải ô trang bị), đọc phím `slot1`-`slot5` qua `GameSettingsManager` (mặc định phím 3-7, vì phím 1-2 dành cho đổi vũ khí), phát `OnHotbarChanged`.
  - `InventorySlotUI.cs`/`InventoryPanelUI.cs` (lưới slot động theo `Inventory.TotalSlotCount`, hỗ trợ kéo item, click để chọn xem mô tả, bật/tắt bằng phím `I`).
  - `HotbarSlotUI.cs`/`HotbarUI.cs` (5 ô cố định, chỉ nhận thả Consumable/Throwable, hiển thị số lượng thật từ `Inventory.GetItemCount`).
  - `CharacterEquipmentManager.cs`: thêm loadout 2 vũ khí Wep chính/phụ (`GetWeaponInLoadoutSlot`/`AssignWeaponToLoadout`/`SwitchWeapon`/`ActiveWeaponSlotIndex`), đọc phím `WeaponSlot1`/`WeaponSlot2` để đổi vũ khí active, event `OnEquipmentChanged`/`OnWeaponLoadoutChanged` cho UI lắng nghe (không gọi UI trực tiếp).
  - `EquipmentSlotUI.cs`/`EquipmentPanelUI.cs` (bảng trang bị đầy đủ: Wep chính, Wep phụ, Helmet, Armor, Boots, Gloves, Backpack — nhận thả kéo từ Inventory để Equip).
  - `ItemSelectionEvents.cs`/`ItemDescriptionUI.cs` (kênh sự kiện UI-only + bảng mô tả item khi click chọn từ Inventory/Hotbar/Equipment).
  - Xem hướng dẫn dựng Canvas/Prefab thủ công trong Unity Editor tại [`Docs/InventoryHotbarUIGuide.md`](InventoryHotbarUIGuide.md).

## 🔍 Ghi Chú Rà Soát Scripts Dựa Trên Rules Mới

### 1. Phân Tách Runtime State khỏi ScriptableObject (`SO`)
- **Tình trạng cần lưu ý**: Khi phát triển các hệ thống tiếp theo (như Vũ khí, Đạn dược, Vật phẩm tiêu hao, Chỉ số nhân vật), **tuyệt đối không** lưu trữ các trạng thái thay đổi theo thời gian thực (như `currentAmmo`, `currentDurability`, `currentHP`) trực tiếp vào các file `.asset` của ScriptableObject.
- **Giải pháp**: Phải tách biệt rõ ràng giữa **Static Config SO** (dữ liệu cấu hình tĩnh) và **Runtime Instance / State** (dữ liệu trạng thái khi chạy).

### 2. Quy Định Giao Tiếp Giữa Gameplay và UI
- **Tình trạng cần lưu ý**: Theo rule mới, tầng Gameplay không được gọi trực tiếp UI để cập nhật (`_hpBar.SetValue(...)`).
- **Đã áp dụng đúng**: `PlayerStatusUI.cs` subscribe vào `OnValueChanged`/`OnDamageTaken` của `CharacterAttributeManager`, unsubscribe trong `OnDestroy`. Cần giữ đúng pattern này khi làm UI Inventory/Hotbar sắp tới — lắng nghe `Inventory.OnInventoryChanged` / `StorageBox.OnStorageChanged`, không để `Inventory`/`StorageBox` biết đến UI.

### 3. Nguyên Lý "No Over-engineering" Khi Làm Tính Năng Tiếp Theo
- **Tình trạng cần lưu ý**: Khi bắt đầu xây dựng UI Kho đồ, Hotbar hay Extraction Zone tiếp theo:
  - **Không** tự động áp dụng hàng loạt Factory, Strategy hay Event Channel nếu logic xử lý chỉ đơn giản là thêm/bớt item hoặc trigger va chạm.
  - Ưu tiên viết code ngắn gọn, rõ ràng, đúng chuẩn SRP và giới hạn dưới 200 dòng.

---

## ⚡ Các Bước Tiếp Theo (Đang Làm)
1. **Wiring thủ công trong Unity Editor**: Code UI Kho Đồ & Hotbar đã xong, nhưng Canvas/Prefab/tham chiếu Inspector cần được dựng thủ công theo [`Docs/InventoryHotbarUIGuide.md`](InventoryHotbarUIGuide.md) (môi trường code hiện tại không có công cụ điều khiển Unity Editor).
2. **UI cho `StorageBox`**: Chưa làm (để dành cho Phase 3 khi dựng Safehouse) — hiện chỉ có UI cho `Inventory` của Player.
3. Sau khi wiring & test UI Inventory/Hotbar trong Editor xong, tiếp tục theo `ProjectRoadmap.md` Phase 3 (Safehouse, Raid Deployment, Extraction Point).
