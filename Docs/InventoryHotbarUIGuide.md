# Hướng Dẫn Dựng UI Kho Đồ (Inventory) & Hotbar Trong Unity Editor

Tài liệu này hướng dẫn từng bước dựng Canvas, Prefab và gắn tham chiếu Inspector cho UI Kho Đồ và Hotbar (không có công cụ tự động hoá Editor trong môi trường code, nên các bước dưới đây cần thực hiện thủ công).

---

## 1. Gắn Component `Hotbar` Lên Player

1. Chọn GameObject Player (đã có sẵn `Inventory` và `CharacterAttributeManager`).
2. Thêm component [`Hotbar.cs`](../Assets/Scripts/Gameplay/Character/Inventory/Hotbar.cs) (cùng GameObject với `Inventory`, vì có `[RequireComponent]`).

---

## 2. Dựng Canvas Kho Đồ (Inventory Panel)

1. **Tạo Canvas**: Chuột phải Hierarchy → **UI** → **Canvas**, đổi tên `InventoryCanvas`, giữ **Render Mode = Screen Space - Overlay**.
2. **Tạo Panel gốc**: Chuột phải `InventoryCanvas` → **UI** → **Panel**, đổi tên `InventoryPanelRoot`. Đây là đối tượng sẽ ẩn/hiện khi bấm phím `I`.
3. **Tạo lưới slot**: Trong `InventoryPanelRoot`, tạo 1 GameObject trống tên `SlotGrid`, thêm component **Grid Layout Group** (chỉnh `Cell Size`, `Spacing` cho vừa số cột mong muốn).
4. **Tạo Prefab `InventorySlotUI`**:
   - Tạo 1 GameObject con trong `SlotGrid`, đặt tên `InventorySlot`, thêm **Image** (làm icon) và **Text - TextMeshPro** (làm số lượng) làm con của nó.
   - Gắn component [`InventorySlotUI.cs`](../Assets/Scripts/UI/Inventory/InventorySlotUI.cs) lên GameObject này, kéo `Image` vào ô `Icon`, `TextMeshPro` vào ô `Amount Text`.
   - Kéo GameObject này ra thư mục `Assets/Prefabs` (hoặc tương đương) để tạo Prefab, rồi xoá bản trong Scene (chỉ giữ lại Prefab).
5. **Gắn `InventoryPanelUI`**:
   - Gắn component [`InventoryPanelUI.cs`](../Assets/Scripts/UI/Inventory/InventoryPanelUI.cs) lên `InventoryCanvas` (hoặc `InventoryPanelRoot`).
   - Kéo Player (có `Inventory`) vào ô `Inventory` (có thể để trống, script tự `FindObjectOfType` lúc `Start`).
   - Kéo Prefab `InventorySlotUI` vào ô `Slot Prefab`.
   - Kéo `SlotGrid` vào ô `Grid Parent`.
   - Kéo `InventoryPanelRoot` vào ô `Panel Root`.

---

## 3. Dựng Thanh Hotbar (5 Ô, Chỉ Dành Cho Consumable/Throwable)

> **Lưu ý cơ chế**: Hotbar **không phải** ô trang bị — nó chỉ là nơi dùng nhanh vật phẩm tiêu hao (`ConsumableSO`) hoặc vật phẩm ném (`ThrowableSO`) từ Inventory. Vũ khí chính/phụ **không** thuộc Hotbar, xem mục 4. Mặc định Hotbar dùng phím **3-7** (phím 1/2 dành riêng cho đổi vũ khí, cấu hình tại `GameSettingsData.slot1..slot5` / `weaponSlot1`, `weaponSlot2`).

1. Trong `InventoryCanvas` (hoặc Canvas HUD riêng luôn hiển thị), tạo GameObject `HotbarPanel`, đặt vị trí dưới màn hình (Anchor: Bottom Center).
2. Tạo 5 GameObject con lần lượt tên `HotbarSlot_1` .. `HotbarSlot_5`, mỗi cái có **Image** (icon), **Text - TextMeshPro** (số lượng) và **Text - TextMeshPro** (nhãn phím, gợi ý hiển thị "3".."7" cho khớp keybind mặc định).
3. Gắn component [`HotbarSlotUI.cs`](../Assets/Scripts/UI/Inventory/HotbarSlotUI.cs) lên mỗi ô, kéo đúng `Image`/`Amount Text`/`Key Label` tương ứng.
4. Gắn component [`HotbarUI.cs`](../Assets/Scripts/UI/Inventory/HotbarUI.cs) lên `HotbarPanel`:
   - Kéo Player (có `Hotbar`) vào ô `Hotbar`, Player (có `Inventory`) vào ô `Inventory` (có thể để trống, tự tìm).
   - Kéo lần lượt `HotbarSlot_1` → `HotbarSlot_5` vào mảng `Slots` theo đúng thứ tự.

---

## 4. Dựng Bảng Trang Bị (Equipment Panel — Bao Gồm Cả Vũ Khí Chính/Phụ)

> **Lưu ý cơ chế**: Đây là nơi duy nhất quản lý toàn bộ trang bị, bao gồm **Wep chính (Primary)**, **Wep phụ (Secondary)** và các trang bị khác (Helmet/Armor/Boots/Gloves/Backpack). Kéo vũ khí từ Inventory thả vào ô Wep chính/phụ sẽ tự Equip (qua `CharacterEquipmentManager.AssignWeaponToLoadout`); bấm phím **1**/**2** (`weaponSlot1`/`weaponSlot2`) để chuyển đổi vũ khí đang active.

1. Trong `InventoryCanvas`, tạo GameObject `EquipmentPanel` (đặt cạnh `SlotGrid`, ví dụ khu vực bên trái panel Inventory).
2. Tạo 7 GameObject con: `EquipmentSlot_WeaponPrimary`, `EquipmentSlot_WeaponSecondary`, `EquipmentSlot_Helmet`, `EquipmentSlot_Armor`, `EquipmentSlot_Boots`, `EquipmentSlot_Gloves`, `EquipmentSlot_Backpack`, mỗi cái có **Image** (icon) và **Text - TextMeshPro** (tên item). Với 2 ô vũ khí, thêm 1 **Image** phụ (viền sáng, tắt sẵn) làm `Active Highlight`.
3. Gắn component [`EquipmentSlotUI.cs`](../Assets/Scripts/UI/Equipment/EquipmentSlotUI.cs) lên mỗi ô:
   - Với `EquipmentSlot_WeaponPrimary`/`EquipmentSlot_WeaponSecondary`: bật tick `Is Weapon Loadout Slot`, đặt `Weapon Loadout Index` = 0 (Primary) hoặc 1 (Secondary), kéo `Active Highlight` tương ứng.
   - Với 5 ô còn lại: để `Is Weapon Loadout Slot` tắt, chọn đúng `Slot Type` (Helmet/Armor/Boots/Gloves/Backpack).
   - Tất cả kéo `Image`/`Name Text` tương ứng.
4. Gắn component [`EquipmentPanelUI.cs`](../Assets/Scripts/UI/Equipment/EquipmentPanelUI.cs) lên `EquipmentPanel`, kéo Player (có `CharacterEquipmentManager`) vào ô `Equipment Manager` (có thể để trống, tự tìm), kéo 7 ô vừa tạo vào mảng `Slots`.
5. Có thể kéo item đang nằm trong bất kỳ `EquipmentSlotUI` nào về một ô trong `SlotGrid` để unequip và trả item lại Inventory. Nếu Inventory đầy, thao tác sẽ bị từ chối.
6. Có thể kéo trực tiếp ô Wep chính sang ô Wep phụ hoặc ngược lại để swap. Swap không đưa item qua Inventory; nếu ô đang active được swap, vũ khí đang cầm và highlight cũng cập nhật theo.

---

## 5. Dựng Bảng Mô Tả Vật Phẩm (Item Description)

1. Tạo 1 GameObject `ItemDescriptionPanel` trong `InventoryCanvas`, gồm **Image** (icon), **Text - TextMeshPro** cho Tên/Mô tả/Độ hiếm/Cân nặng.
2. Gắn component [`ItemDescriptionUI.cs`](../Assets/Scripts/UI/Inventory/ItemDescriptionUI.cs) lên `ItemDescriptionPanel`, kéo `Panel Root` (chính nó hoặc GameObject con chứa nội dung) và các Text/Image tương ứng.
3. Bảng này tự động hiện/ẩn khi người chơi bấm (click) vào 1 ô trong Inventory, Hotbar hoặc Equipment Panel — không cần thêm dây nối nào khác (`InventorySlotUI`/`HotbarSlotUI`/`EquipmentSlotUI` tự phát sự kiện `ItemSelectionEvents.OnItemSelected`).

---

## 6. Kiểm Tra Raycast Cho Kéo-Thả (Drag & Drop)

- Đảm bảo Scene có **EventSystem** (Unity tự tạo khi thêm Canvas đầu tiên, nếu chưa có thì Chuột phải Hierarchy → **UI** → **Event System**).
- Mỗi `Image` icon trong `InventorySlotUI`, `HotbarSlotUI` và `EquipmentSlotUI` cần bật **Raycast Target** để nhận sự kiện kéo/thả/click.

---

## 7. Ghi Chú Cho HUD Khi Đang Chơi (Play UI)

Màn hình Inventory (bấm `I`) và HUD lúc đang chơi là 2 khối UI khác nhau nhưng cùng phản ánh 1 dữ liệu gốc (`CharacterEquipmentManager`/`Hotbar`). Nếu muốn có 1 thanh HUD gộp luôn hiển thị khi chơi (không cần mở Inventory), tạo thêm 1 Canvas HUD riêng (luôn `SetActive(true)`), đặt vào đó theo thứ tự:
- 2 ô hiển thị Wep chính/Wep phụ: dùng lại `EquipmentSlotUI` (chế độ `Is Weapon Loadout Slot`) trỏ vào cùng `CharacterEquipmentManager`.
- 5 ô Hotbar: dùng lại `HotbarUI`/`HotbarSlotUI` trỏ vào cùng `Hotbar`.

Vì cả 2 đều tự lắng nghe event (`OnEquipmentChanged`/`OnWeaponLoadoutChanged`/`OnHotbarChanged`), không cần thêm code — chỉ cần dựng thêm 1 bộ UI (Prefab) tham chiếu đúng Player.

---

## 8. Kiểm Thử (Play Mode)

1. Bấm Play, nhặt vài item tiêu hao (Consumable), vật phẩm ném (Throwable) và vũ khí (Weapon) ngoài world.
2. Bấm phím **I** để mở/đóng `InventoryPanelRoot`, kiểm tra icon + số lượng hiển thị đúng.
3. Click vào 1 item bất kỳ (Inventory/Hotbar/Equipment) — `ItemDescriptionPanel` phải hiện tên/mô tả/độ hiếm/cân nặng đúng.
4. Kéo item Vũ khí thả vào ô Wep chính hoặc Wep phụ trong `EquipmentPanel` — nhân vật phải Equip vũ khí đó ngay (đổi model tay + tư thế chiến đấu); ô tương ứng hiện icon vũ khí kèm viền sáng (Active Highlight).
5. Bấm phím **1**/**2** để chuyển đổi qua lại giữa Wep chính/Wep phụ — quan sát model vũ khí trên tay và `CurrentWeaponType` đổi đúng.
6. Kéo giáp/mũ/balo từ Inventory thả vào đúng ô tương ứng trong `EquipmentPanel` — chỉ số (Máu/Thể lực/...) và model 3D phải cập nhật.
7. Khi equip thành công, item được lấy khỏi Inventory. Kéo item từ Equipment Panel về một ô Inventory để unequip và item xuất hiện lại trong Inventory.
8. Kéo ô Wep chính/phụ qua lại để swap — hai item đổi vị trí, không bị trùng và không làm thay đổi số lượng Inventory.
9. Kéo 1 item Consumable hoặc Throwable từ lưới Inventory thả vào 1 trong 5 ô Hotbar — icon + số lượng phải xuất hiện ở ô Hotbar đó.
10. Bấm phím tương ứng (mặc định 3-7) — Consumable hồi Máu/Thể lực (`PlayerStatusUI`) và trừ số lượng; Throwable sinh prefab ném về phía trước và trừ số lượng.
11. Dùng hết item (`GetItemCount` về 0) — ô Hotbar vẫn hiển thị gán (icon nhạt/số lượng 0 tuỳ thiết kế), bấm phím sẽ không có tác dụng cho tới khi nhặt thêm item cùng loại.
