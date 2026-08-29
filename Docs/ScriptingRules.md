# Quy Tắc Viết Code & Kiến Trúc (`Docs/ScriptingRules.md`)

Bộ quy tắc này đóng vai trò là **Guardrail (Hàng rào bảo vệ)** cho dự án **R_Survival**, hướng tới sự rõ ràng, dễ bảo trì, tối ưu hiệu năng mobile, và **tuyệt đối không lạm dụng thiết kế phức tạp (No Over-engineering)**.

---

## 1. Giới Hạn Độ Dài Script (Script Length)
- **150–200 dòng**: Lý tưởng, dễ đọc và bảo trì.
- **250 dòng**: Cảnh báo (Warning), cần chú ý cấu trúc lại nếu tiếp tục phình to.
- **300 dòng**: Ngưỡng bắt buộc Review / Refactor tách nhỏ.

## 2. Nguyên Tắc Trách Nhiệm Duy Nhất (SRP)
- Mỗi class chỉ đảm nhận **một trách nhiệm chính**.
- **Không bao giờ** tách class chỉ để cố gắng giảm số dòng code nếu nó làm mất tính cohesiveness (tính liên kết logic).

## 3. State Machine
- Chỉ sử dụng cho state/action phức tạp (như Character Controller, AI Behavior phức tạp).
- **Không bắt buộc** mọi hành động nhỏ trong game đều phải đẻ ra một State riêng.

## 4. Singleton Pattern
- Chỉ dành cho các global service thực sự cần vòng đời persistent (`DontDestroyOnLoad`, ví dụ: `GameSettingsManager`, `AudioManager`).
- Hạn chế lạm dụng Singleton cho các hệ thống cục bộ.

## 5. Event-Driven & Loose Coupling
- Sử dụng sự kiện để giảm sự phụ thuộc trực tiếp (Loose coupling), ví dụ Gameplay phát sự kiện để UI lắng nghe (`HealthChanged?.Invoke(...)`), **tuyệt đối không để Gameplay gọi trực tiếp UI**.
- **Không dùng event** để thay thế cho mọi lời gọi phương thức thông thường (`method call`).
- **Bắt buộc** unsubscribe sự kiện đúng vòng đời (`OnDisable` / `OnDestroy`) để tránh memory leak.

## 6. Dependency Architecture (Kiến Trúc Phụ Thuộc)
- **Không phụ thuộc ngược** (tránh circular dependency giữa các layer).
- Tầng Low-level $\to$ High-level Gameplay $\to$ Controller / Facade $\to$ Presentation.
- Gameplay không được truy cập trực tiếp vào implementation nội bộ của các tầng cơ sở mà phải qua Abstraction / Public API.

## 7. Manager Pattern
- **Không được để Manager biến thành God Object** (ôm đồm mọi thứ vào một chỗ).
- Chia nhỏ thành các Sub-managers hoặc Service handlers chuyên biệt.

## 8. Performance & Optimization (Mobile-First)
- **Tránh allocation / GC** ở hot path (trong `Update`, `FixedUpdate`, các hàm lặp liên tục).
- Sử dụng Object Pooling cho các đối tượng spawn/despawn nhiều (đạn, hiệu ứng, kẻ địch).
- **Phải Profiling trước khi thực hiện tối ưu hóa sớm (Premature Optimization)**.

## 9. Partial Class
- **Không dùng partial class** chỉ để né tránh quy tắc giới hạn độ dài script (`script-length rule`).

## 10. Facade / Public API Pattern
- Các module giao tiếp với nhau thông qua **Public API hoặc Abstraction** gọn gàng.

## 11. ScriptableObject Runtime Safety
- ScriptableObject dùng cho **Static / Configuration Data** (Ví dụ: `WeaponSO` chứa damage, fireRate, magazineSize).
- **Không sửa trực tiếp** dữ liệu cấu hình của SO trong runtime gameplay.
- Runtime state phải được lưu trong Runtime Data / Instance / Component tương ứng (Ví dụ: `WeaponRuntime` chứa currentAmmo, currentDurability).
- Không dùng SO asset làm nơi lưu trạng thái riêng của từng entity/player.

---

## 🏗️ Hướng Phụ Thuộc Runtime Mặc Định (Runtime Dependency Hierarchy)

Dự án tuân thủ hướng phụ thuộc logic mặc định dưới đây để giữ hệ thống mạch lạc (Lưu ý: Đây là hướng phụ thuộc cấu trúc, không phải mọi luồng dữ liệu runtime đều bắt buộc đi đúng chiều mũi tên, ví dụ Event từ Gameplay có thể phát thẳng tới UI):

```text
┌──────────────────────────┐
│ ScriptableObjects (SO)   │  ◄── Dữ liệu tĩnh (Static Data)
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ Runtime Data / Stacks    │  ◄── Dữ liệu trạng thái khi chạy
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ Gameplay Modules         │  ◄── Logic xử lý gameplay cốt lõi
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ Controllers / Facades    │  ◄── Điều phối trung tâm
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ UI / Audio Presentation  │  ◄── Hiển thị và âm thanh (Nhận Event từ Gameplay)
└──────────────────────────┘
```

### 📌 Nguyên Tắc Tối Cao & Triết Lý Thiết Kế:
1. **No Pattern for Pattern's Sake**: Không áp dụng Design Pattern chỉ vì pattern tồn tại. Pattern chỉ được sử dụng khi nó giải quyết một vấn đề kiến trúc hoặc gameplay thực tế. **Nếu một `if` đơn giản giải quyết tốt vấn đề, hãy dùng `if`.**
2. **Architecture phục vụ Gameplay**, không phải Gameplay phục vụ Architecture.
3. Nếu một abstraction, state, event, service hay pattern **không làm hệ thống rõ hơn / dễ mở rộng hơn / dễ test hơn**, thì **không cần tạo nó (No Over-engineering)**.
