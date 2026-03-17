using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Hàm này tự động chạy khi có một vật thể (Collider2D) lọt vào vùng Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player hoặc Enemy không
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            // Lấy component Character của đối tượng đó và gọi hàm nhận sát thương
            Character character = collision.GetComponent<Character>();

            // Kiểm tra character khác null để tránh lỗi NullReferenceException nếu lỡ va chạm vật khác
            if (character != null)
            {
                character.OnHit(30f); // Gây 30 sát thương (bạn có thể thay đổi số này)
            }
        }
    }
}