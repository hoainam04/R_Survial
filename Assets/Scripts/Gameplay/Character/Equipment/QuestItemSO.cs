using UnityEngine;
using PROJ.Item;
    [CreateAssetMenu(fileName = "New Quest Item", menuName = "PROJ/Item/QuestItem")]
public class QuestItemSO : ItemSO
{
    public override bool isStackable => true; // Ghi đè để luôn trả về true, vì vật phẩm ném có thể xếp chồng

    public override ItemType itemType => ItemType.QuestItem; // Ghi đè để luôn trả về QuestItem
    [Header("Thông tin vật phẩm nhiệm vụ")]
    public string questName; // Tên nhiệm vụ liên quan đến vật phẩm này
    public string questDescription; // Mô tả nhiệm vụ liên quan đến vật phẩm này
    public bool isRequiredForQuest; // Xác định xem vật phẩm này có cần thiết cho nhiệm vụ hay không

    public void UseQuestItem()
    {
        // Logic sử dụng vật phẩm nhiệm vụ
        // Ví dụ: Cập nhật tiến trình nhiệm vụ, mở khóa cốt truyện, hoặc kích hoạt sự kiện đặc biệt
    }
}