# Tài Liệu Thiết Kế Trò Chơi (Game Design Document - GDD) - R_Survival

Tài liệu này phác thảo tầm nhìn thiết kế, vòng lặp cốt lõi (Core Loop), và các cơ chế gameplay chính của dự án **R_Survival** (Thể loại **Extraction Survival** tương tự *Escape from Tarkov / Escape from Duckov*).

---

## 1. Tầm Nhìn & Tổng Quan (Vision & Overview)
- **Tên dự án**: R_Survival
- **Thể loại**: Hành động sinh tồn bóc tách hiểm họa (**Extraction Shooter / Extraction Survival**).
- **Mục tiêu**: Người chơi xuất phát từ **Căn cứ (Safehouse / Hub)**, tham gia vào các chuyến đột kích (Raid) vào các bản đồ hoang dã/nguy hiểm để nhặt tài nguyên, tiêu diệt kẻ địch, hoàn thành nhiệm vụ và tìm đường đến **Điểm Rút Lui (Extraction Point)** để mang chiến lợi phẩm trở về an toàn. Nếu tử trận trong màn chơi, người chơi sẽ mất toàn bộ số vật phẩm đang mang theo người.

---

## 2. Vòng Lặp Trò Chơi Cốt Lõi (Core Loop - Extraction Loop)

```text
[Căn Cứ / Hub] ──(Chuẩn bị & Nhận Quest)──► [Đột Kích Bản Đồ (Raid)]
       ▲                                               │
       │                                     (Khám phá & Nhặt đồ)
       │                                               │
       │                                     (Tiêu diệt mục tiêu)
       │                                               │
  [Bán Đồ & Nâng Cấp] ◄──(Thành Công Rút Lui)◄── [Điểm Rút Lui (Extraction)]
       │                                               │
       └──────────────────(Tử Trận: Mất Đồ)──────────────┘
```

1. **Giai đoạn Chuẩn Bị (Pre-Raid / Hub)**:
   - Quản lý kho đồ tại căn cứ.
   - Nhận nhiệm vụ (Quests) từ hệ thống (Ví dụ: Tiêu diệt kẻ địch chỉ định, tương tác với địa điểm, tìm kiếm và mang về một vật phẩm nhiệm vụ cụ thể).
   - Trang bị vũ khí, đạn dược, giáp và sắp xếp vào các ô phím tắt nhanh (`Slot 1` đến `Slot 5`).

2. **Giai đoạn Đột Kích (In-Raid / Exploration & Combat)**:
   - Thâm nhập vào bản đồ, quản lý các chỉ số sinh tồn (`CharacterStatusController`: Đói, Khát, Thể lực, Máu).
   - Chiến đấu với kẻ địch (`Enemy`) sử dụng hệ thống FSM (`CharacterControllerBrain`).
   - Khám phá, tương tác nhặt vật phẩm (`PlayerItemPicker`, `WorldItem`, `Inventory`).

3. **Giai đoạn Rút Lui & Căn Cứ (Extraction & Post-Raid)**:
   - Tìm đến các **Điểm Rút Lui (Extraction Point)** được chỉ định trên bản đồ để hoàn thành chuyến đi.
   - Mang chiến lợi phẩm về căn cứ: Bán vật phẩm qua hệ thống **Shop tại căn cứ** để tích lũy tiền tệ, mua sắm trang bị tốt hơn hoặc trả nhiệm vụ nhận thưởng.

---

## 3. Các Cơ Chế Gameplay Chính (Key Gameplay Mechanics)

### A. Cơ Chế Extraction & Risk/Reward (Rủi Ro & Phần Thưởng)
- **Rủi ro cao, phần thưởng lớn**: Đồ nhặt được trong Raid chỉ thực sự thuộc về người chơi khi rút lui thành công (`Extract`).
- **Mất trang bị khi tử trận**: Nếu hết máu (`PlayerHealth`), người chơi sẽ đánh mất toàn bộ chiến lợi phẩm và trang bị đang mang trên người (ngoại trừ các vật phẩm được bảo hiểm hoặc lưu giữ ở kho chung tại căn cứ).

### B. Hệ Thống Nhiệm Vụ (Quest System)
Người chơi nhận nhiệm vụ tại Căn cứ trước khi vào Raid với các dạng mục tiêu phong phú:
1. **Tiêu diệt mục tiêu**: Tiêu diệt số lượng hoặc loại kẻ địch chỉ định trên map.
2. **Khám phá & Tương tác**: Di chuyển đến các địa điểm đặc biệt trên bản đồ để thu thập thông tin hoặc kích hoạt thiết bị.
3. **Thu thập & Mang về (Fetch & Extract)**: Tìm kiếm các loại vật phẩm quý hiếm (`QuestItemSO`) và bắt buộc phải **rút lui thành công** mang theo vật phẩm đó về căn cứ để hoàn thành nhiệm vụ.

### C. Kinh Tế & Căn Cứ (Economy & Hub Shop)
- **Shop tại Căn cứ**: Nơi người chơi quy đổi các vật phẩm nhặt được (vật liệu chế tạo, vật phẩm giá trị, đạn dư thừa) thành tiền tệ.
- **Tiêu tiền**: Mua sắm vũ khí mới (`WeaponRangedSO`, `WeaponMeleeSO`), trang bị giáp (`ArmorSO`, `HelmetSO`, `BackPackSO`) và vật phẩm tiêu hao (`ConsumableSO`) chuẩn bị cho chuyến Raid tiếp theo.

### D. Hệ Thống Điều Khiển & Gán Phím (Settings & Keybinds)
- Quản lý toàn bộ thiết lập qua `GameSettingsManager` (Hỗ trợ lưu trữ JSON).
- Hỗ trợ gán phím linh hoạt và **5 phím tắt chọn nhanh slot 1 đến 5** (`slot1` đến `slot5` ứng với `KeyCode.Alpha1` đến `KeyCode.Alpha5`) giúp người chơi thao tác nhanh chóng chuyển đổi vũ khí hoặc vật phẩm hồi máu ngay trong lúc giao tranh căng thẳng.
