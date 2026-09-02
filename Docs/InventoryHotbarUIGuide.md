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

## 3. Dựng Thanh Hotbar (5 Ô Cố Định)

1. Trong `InventoryCanvas` (hoặc Canvas HUD riêng luôn hiển thị), tạo GameObject `HotbarPanel`, đặt vị trí dưới màn hình (Anchor: Bottom Center).
2. Tạo 5 GameObject con lần lượt tên `HotbarSlot_1` .. `HotbarSlot_5`, mỗi cái có **Image** (icon), **Text - TextMeshPro** (số lượng) và **Text - TextMeshPro** (nhãn phím "1".."5").
3. Gắn component [`HotbarSlotUI.cs`](../Assets/Scripts/UI/Inventory/HotbarSlotUI.cs) lên mỗi ô, kéo đúng `Image`/`Amount Text`/`Key Label` tương ứng.
4. Gắn component [`HotbarUI.cs`](../Assets/Scripts/UI/Inventory/HotbarUI.cs) lên `HotbarPanel`:
   - Kéo Player (có `Hotbar`) vào ô `Hotbar`, Player (có `Inventory`) vào ô `Inventory` (có thể để trống, tự tìm).
   - Kéo lần lượt `HotbarSlot_1` → `HotbarSlot_5` vào mảng `Slots` theo đúng thứ tự.

---

## 4. Kiểm Tra Raycast Cho Kéo-Thả (Drag & Drop)

- Đảm bảo Scene có **EventSystem** (Unity tự tạo khi thêm Canvas đầu tiên, nếu chưa có thì Chuột phải Hierarchy → **UI** → **Event System**).
- Mỗi `Image` icon trong `InventorySlotUI` và `HotbarSlotUI` cần bật **Raycast Target** để nhận sự kiện kéo/thả.

---

## 5. Kiểm Thử (Play Mode)

1. Bấm Play, nhặt vài item tiêu hao (Consumable) ngoài world.
2. Bấm phím **I** để mở/đóng `InventoryPanelRoot`, kiểm tra icon + số lượng hiển thị đúng.
3. Kéo 1 item Consumable từ lưới Inventory thả vào 1 trong 5 ô Hotbar — icon + số lượng phải xuất hiện ở ô Hotbar đó.
4. Bấm phím tương ứng (1-5) — quan sát thanh Máu/Thể lực (`PlayerStatusUI`) thay đổi và số lượng item trong Hotbar/Inventory giảm đi 1.
5. Dùng hết item (`GetItemCount` về 0) — ô Hotbar vẫn hiển thị gán (icon nhạt/số lượng 0 tuỳ thiết kế), bấm phím sẽ không có tác dụng cho tới khi nhặt thêm item cùng loại.
