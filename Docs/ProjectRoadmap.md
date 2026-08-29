# 🗺️ Lộ Trình Phát Triển Trò Chơi (Game Development Roadmap) - R_Survival

Lộ trình chi tiết theo từng giai đoạn (Phase) từ khởi đầu đến khi hoàn thiện tựa game **Extraction Survival** (*R_Survival*).

---

## 📌 Phase 1: Hệ Thống Cốt Lõi & Điều Khiển Cơ Bản (Core Systems & Character Foundation)
*Mục tiêu: Xây dựng nền tảng di chuyển, chiến đấu mượt mà và hệ thống cấu hình toàn cục.*

- [x] **Character Controller & FSM**: Xây dựng bộ não nhân vật (`CharacterControllerBrain`) kết hợp các State (Idle, Move, Run, Dash, Attack, Reload).
- [x] **Input & Aiming Modularization**: Tách rời `CharacterInputHandler` và `CharacterAimingHandler` (Hỗ trợ PC chuột và Mobile Auto-aim).
- [x] **Global Game Settings & Keybinds**: Quản lý âm thanh, đồ họa và gán phím linh hoạt qua `GameSettingsManager` (Lưu trữ JSON).
- [x] **Hotbar & Quick Slots**: Tích hợp phím tắt chọn nhanh từ số **1 đến 5** (`slot1` đến `slot5`).

---

## 📌 Phase 2: Hệ Thống Kho Đồ & Trang Bị (Inventory & Equipment Loop)
*Mục tiêu: Hoàn thiện vòng lặp nhặt đồ, quản lý tài nguyên và trang bị nhân vật.*

- [ ] **Inventory System**: Xây dựng kho đồ dạng lưới/slot (`Inventory`, `InventorySlot`, `ItemStack`), hỗ trợ cộng dồn và quản lý item.
- [ ] **Equipment System**: Hoàn thiện các slot trang bị (Vũ khí cận chiến/súng, giáp thân, mũ, giày, ba lô, item tiêu hao).
- [ ] **World Loot & Interaction**: Hệ thống vật phẩm rơi trong thế giới 3D (`WorldItem`, `PlayerItemPicker`) và nhặt đồ vào kho.
- [ ] **UI Kho Đồ & Hotbar**: Giao diện trực quan hiển thị túi đồ và thanh phím tắt 1-5.

---

## 📌 Phase 3: Vòng Lặp Extraction & Căn Cứ (Extraction Loop & Safehouse Hub)
*Mục tiêu: Hiện thực hóa tính năng cốt lõi của thể loại Extraction Survival (Rủi ro & Phần thưởng).*

- [ ] **Safehouse (Căn Cứ)**: Xây dựng giao diện và logic tại Hub an toàn trước khi bắt đầu trận Raid.
- [ ] **Raid Deployment (Xuất kích)**: Cơ chế load map, đưa người chơi vào vùng nguy hiểm cùng trang bị đã chọn.
- [ ] **Extraction Point (Điểm rút lui)**: Lập trình vùng thoát hiểm, thời gian đếm ngược và kiểm tra điều kiện thành công.
- [ ] **Death Penalty & Stash (Tử trận & Kho đồ chung)**: Logic mất toàn bộ trang bị khi chết trong Raid và hệ thống kho chứa đồ an toàn tại Căn cứ.

---

## 📌 Phase 4: Kinh Tế, Shop & Hệ Thống Nhiệm Vụ (Economy & Quests)
*Mục tiêu: Tạo động lực chơi lại (Replayability) thông qua kinh tế và mục tiêu nhiệm vụ.*

- [ ] **Hub Shop (Cửa hàng căn cứ)**: Hệ thống mua bán trang bị, vũ khí, đạn dược bằng tiền thu thập từ các chuyến Raid.
- [ ] **Quest Manager (Quản lý nhiệm vụ)**: Hệ thống giao nhận và theo dõi các loại Quest:
  - *Tiêu diệt kẻ địch chỉ định.*
  - *Khám phá & tương tác với các địa điểm trên bản đồ.*
  - *Thu thập vật phẩm quý hiếm (`QuestItemSO`) và bắt buộc phải rút lui thành công.*
- [ ] **Quest Rewards**: Cơ chế trả nhiệm vụ nhận tiền tệ, mở khóa trang bị mới tại Shop.

---

## 📌 Phase 5: Kẻ Địch, Môi Trường & Sinh Tồn Nâng Cao (AI, Environment & Survival Mechanics)
*Mục tiêu: Tăng độ thử thách và chiều sâu gameplay sinh tồn.*

- [ ] **Advanced Enemy AI**: Kẻ địch thông minh hơn với các trạng thái tuần tra (Patrol), phát hiện người chơi (Detection), tấn công (Combat) và ẩn nấp.
- [ ] **Survival Demands (Đói, Khát, Thể lực)**: Hoàn thiện hoàn toàn các module hao hụt sinh tồn (`SurvivalDrainModule`) và tác động của môi trường.
- [ ] **Status Effects (Hiệu ứng trạng thái)**: Hoàn thiện các hiệu ứng Bỏng, Độc, Làm chậm, Choáng tác động trực tiếp trong giao tranh.

---

## 📌 Phase 6: Hoàn Thiện, UI/UX & Polish (Polishing & Release Prep)
*Mục tiêu: Đánh bóng sản phẩm, tối ưu hiệu năng và chuẩn bị phát hành.*

- [ ] **Main Menu & HUD**: Màn hình chính, màn hình kết quả sau trận Raid (Thành công/Tử trận), hiển thị máu, thể lực, thanh đạn.
- [ ] **Audio & Visual Effects**: Thêm âm thanh tiếng súng, bước chân, tiếng nhặt đồ và hiệu ứng hạt (VFX) cháy nổ, máu me.
- [ ] **Balancing & Bug Testing**: Cân bằng chỉ số sát thương, độ hiếm item, giá trị kinh tế và sửa lỗi tổng thể.
