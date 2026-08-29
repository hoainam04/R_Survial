# 📋 Ghi Chú Công Việc & Hướng Dẫn Refactor Theo Rules Mới (Current Task & Refactor Notes) - R_Survival

Tài liệu này ghi nhận các điểm cần lưu ý và kế hoạch refactor các scripts hiện tại dựa trên bộ [`Docs/ScriptingRules.md`](ScriptingRules.md) vừa được nâng cấp (Bao gồm kiểm soát ScriptableObject runtime, quy định Event UI một chiều, và nguyên lý chống Over-engineering).

---

## 🔍 Ghi Chú Rà Soát Scripts Dựa Trên Rules Mới

### 1. Phân Tách Runtime State khỏi ScriptableObject (`SO`)
- **Tình trạng cần lưu ý**: Khi phát triển các hệ thống tiếp theo (như Vũ khí, Đạn dược, Vật phẩm tiêu hao, Chỉ số nhân vật), **tuyệt đối không** lưu trữ các trạng thái thay đổi theo thời gian thực (như `currentAmmo`, `currentDurability`, `currentHP`) trực tiếp vào các file `.asset` của ScriptableObject.
- **Giải pháp**: Phải tách biệt rõ ràng giữa **Static Config SO** (dữ liệu cấu hình tĩnh) và **Runtime Instance / State** (dữ liệu trạng thái khi chạy).

### 2. Quy Định Giao Tiếp Giữa Gameplay và UI
- **Tình trạng cần lưu ý**: Theo rule mới, tầng Gameplay không được gọi trực tiếp UI để cập nhật (`_hpBar.SetValue(...)`).
- **Giải pháp refactor sắp tới**: 
  - Các class quản lý gameplay (như `PlayerHealth`, `CharacterStatusController`, `Inventory`) phải phát ra các C# Event (Ví dụ: `OnHealthChanged`, `OnInventoryChanged`).
  - Tầng UI Presentation sẽ chủ động đăng ký lắng nghe (`subscribe`) các event này để tự cập nhật hiển thị, đồng thời phải `unsubscribe` trong `OnDisable` / `OnDestroy` để tránh memory leak trên mobile.

### 3. Nguyên Lý "No Over-engineering" Khi Làm Tính Năng Tiếp Theo
- **Tình trạng cần lưu ý**: Khi bắt đầu xây dựng hệ thống kho chứa căn cứ (`StorageBox`) hay điểm rút lui (`ExtractionZone`) tiếp theo:
  - **Không** tự động áp dụng hàng loạt Factory, Strategy hay Event Channel nếu logic xử lý chỉ đơn giản là thêm/bớt item hoặc trigger va chạm.
  - Ưu tiên viết code ngắn gọn, rõ ràng, đúng chuẩn SRP và giới hạn dưới 200 dòng.

---

## ⚡ Các Bước Chuẩn Bị Tiếp Theo (Chưa Code)
1. Kiểm tra các script quản lý dữ liệu hiện tại để đảm bảo không vi phạm `ScriptableObject Runtime Safety`.
2. Lên thiết kế chuẩn cho Event Channel / C# Event giữa Gameplay và UI trước khi dựng giao diện.
3. Tiến hành xây dựng logic `StorageBox` (Kho căn cứ) tuân thủ tuyệt đối các rules mới này.
