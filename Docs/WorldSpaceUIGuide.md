# Hướng Dẫn Tạo và Cấu Hình World Space UI Trong Unity (Ví dụ: Thanh Máu Trên Đầu Nhân Vật)

Tài liệu này hướng dẫn từng bước chi tiết cách tạo một Canvas ở chế độ **World Space** để hiển thị thanh máu/stamina hoặc thông tin trôi nổi trên đầu nhân vật trong game.

---

## 1. Các Bước Tạo World Space Canvas Trong Unity Editor

1. **Tạo Canvas**:
   - Nhấp chuột phải vào phân vùng Hierarchy $\to$ **UI** $\to$ **Canvas**.
   - Đổi tên Canvas thành `WorldSpaceCanvas` (hoặc `HealthBarCanvas`).

2. **Chuyển Render Mode sang World Space**:
   - Chọn đối tượng `Canvas` vừa tạo.
   - Tại component **Canvas**, đổi mục **Render Mode** từ *Screen Space - Overlay* sang **World Space**.

3. **Gắn Canvas vào Nhân Vật (Character / Enemy)**:
   - Kéo thả `Canvas` này làm **Child (Con)** của Prefab nhân vật hoặc quái vật của bạn.
   - Đặt lại vị trí Transform (`Position`) của Canvas sao cho nó nằm ngay phía trên đầu nhân vật (Ví dụ: `Y = 2.2` hoặc tùy thuộc chiều cao model nhân vật).

4. **Điều Chỉnh Kích Thước (Rect Transform Scale)**:
   - Vì chế độ World Space tính kích thước theo đơn vị Unity (mét), Canvas lúc này sẽ rất to.
   - Bạn cần chỉnh lại **Scale** của Rect Transform nhỏ lại rất nhiều, ví dụ:
     - `Scale X: 0.005`, `Scale Y: 0.005`, `Scale Z: 0.005` (tuỳ thuộc vào kích thước nhân vật).
   - Chỉnh lại **Width** và **Height** (Ví dụ: `Width = 200`, `Height = 50`).

---

## 2. Thêm UI Components (Slider / Text)

1. **Tạo Thanh Máu (Slider)**:
   - Nhấp chuột phải vào `WorldSpaceCanvas` $\to$ **UI** $\to$ **Slider**.
   - Chỉnh lại kích thước Slider cho vừa vặn với khung nhìn (Ví dụ: Width = 150, Height = 15).
   - Xóa bỏ thành phần Handle (nếu không cần núm kéo) trong thành phần con của Slider để nhìn giống thanh máu tiêu chuẩn.

2. **Tạo Text Hiển Thị Số (TextMeshPro)**:
   - Nhấp chuột phải vào `WorldSpaceCanvas` $\to$ **UI** $\to$ **Text - TextMeshPro**.
   - Căn chỉnh vị trí nằm đè lên trên Slider để hiển thị dạng số (Ví dụ: `100 / 100`).

---

## 3. Gắn Script Điều Khiển & Xướng Hướng Camera (Billboard)

1. **Gắn Script Hiển Thị Trạng Thái**:
   - Kéo component script [`PlayerStatusUI.cs`](Assets/Scripts/UI/PlayerStatusUI.cs) thả vào `WorldSpaceCanvas`.
   - Kéo thả `Slider` và `TextMeshPro` tương ứng vào các ô `Health Slider`, `Health Text` trong Inspector của script.

2. **Gắn Script Xướng Hướng Nhìn Camera (Billboard)**:
   - Để thanh máu luôn quay mặt về phía Camera chính (không bị ngược hoặc khuất khi xoay góc nhìn), hãy gắn script [`WorldSpaceUIBillboard.cs`](Assets/Scripts/UI/WorldSpaceUIBillboard.cs) trực tiếp lên `WorldSpaceCanvas`.
