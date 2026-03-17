using UnityEngine;
public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        // Khi kunai được tạo ra, nó sẽ tự động bay về hướng mà nó đang quay mặt (transform.right) với tốc độ 5 đơn vị/giây
        rb.linearVelocity = transform.right * 5f;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    // Hàm gọi khi có một vật thể (Collider2D) vừa chạm vào kunai
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            // Gọi hàm OnHit trên đối tượng Enemy, truyền vào lượng sát thương (ví dụ: 30)
            collision.GetComponent<Character>().OnHit(30f);
            Instantiate(hitVFX, transform.position, transform.rotation); // Tạo hiệu ứng va chạm tại vị trí của kunai
            OnDespawn();
        }
    }
}