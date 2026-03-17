using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Mục tiêu mà Camera cần bám theo (ở đây sẽ là nhân vật Player)
    public Transform target;

    // Khoảng cách bù trừ giữa Camera và Player 
    // (Lưu ý: Phải set trục Z của offset là số âm, ví dụ -10, nếu không màn hình sẽ đen thui)
    public Vector3 offset;

    // Tốc độ trượt đuổi theo của Camera. Số càng nhỏ Camera đi theo càng mượt (có độ trễ).
    public float speed = 20f;

    void Start()
    {
        // Tự động lục tìm trên bản đồ xem object nào có gắn script "Player", 
        // sau đó lấy tọa độ (transform) của nó gán vào biến target. 
        // Nhờ vậy bạn không cần kéo thả Player vào Camera bằng tay nữa.
        target = FindFirstObjectByType<Player>().transform;
    }

    // Dùng FixedUpdate để đồng bộ với nhịp tính toán vật lý (Rigidbody2D) của Player,
    // giúp hình ảnh khi Camera di chuyển không bị giật (jitter).
    void FixedUpdate()
    {
        // Vector3.Lerp là hàm nội suy, giúp Camera trượt từ từ từ vị trí hiện tại (transform.position) 
        // tới vị trí đích (target.position + offset) dựa trên thời gian và tốc độ (speed),
        // tạo ra hiệu ứng đuổi theo mượt mà như quay phim.
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * speed);
    }
}
