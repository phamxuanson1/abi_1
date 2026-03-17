using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    // Khai báo 2 vị trí mốc để bục chạy qua chạy lại
    [SerializeField] private Transform aPoint, bPoint;

    // Tốc độ di chuyển của bục
    [SerializeField] private float speed;

    // Biến lưu trữ mục tiêu hiện tại mà bục đang hướng tới
    private Transform target;

    void Start()
    {
        // Khi mới bắt đầu game, đặt mục tiêu mặc định là điểm B
        target = bPoint;
    }

    void Update()
    {
        // Vector3.MoveTowards giúp di chuyển vật thể từ vị trí hiện tại tới vị trí target với tốc độ (speed) đều đặn
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Kiểm tra khoảng cách: Nếu bục đã chạy đến rất gần điểm A (khoảng cách < 0.1f) thì quay đầu chạy sang B
        if (Vector2.Distance(transform.position, aPoint.position) < 0.1f)
        {
            target = bPoint;
        }
        // Ngược lại, nếu đã chạy đến gần điểm B thì quay đầu chạy về A
        else if (Vector2.Distance(transform.position, bPoint.position) < 0.1f)
        {
            target = aPoint;
        }
    }

    // --- XỬ LÝ ĐỂ PLAYER KHÔNG BỊ TRƯỢT KHỎI BỤC ---

    // Hàm gọi khi có một vật thể (Collision2D) vừa chạm vào bục
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu vật thể chạm vào là Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cho Player làm "con" (child) của MovingPlatform. 
            // Nhờ vậy, khi nền di chuyển thì Player cũng tự động bị kéo đi theo.
            collision.transform.SetParent(transform);
        }
    }

    // Hàm gọi khi vật thể vừa rời khỏi bục (nhảy lên không trung hoặc rớt xuống)
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Nếu vật thể rời đi là Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // THÊM DÒNG NÀY: Chỉ gỡ parent khi bản thân cái bục vẫn đang bật (active)
            if (gameObject.activeInHierarchy)
            {
                collision.transform.SetParent(null);
            }
        }
    }
}
