using System;
using UnityEngine;
public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideDuration = 0.3f;
    [SerializeField] private GameObject smoke;
    [SerializeField] private Transform VFX_Offset; // Điểm xuất hiện hiệu ứng khói khi trượt


    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;


    private float horizontal; //-1 (trái), 0 (đứng yên), 1 (phải)
    [SerializeField] private float jumpForce = 350;

    private int coin = 0;
    private Vector3 savePoint;

    public bool isSliding = false;
    private float slideTimer = 0f;
    // Start is called before the first frame update


    // Update is called once per frame

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    void Update()
    {
        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (isDead) return;

        // 1. ƯU TIÊN CAO NHẤT: XỬ LÝ LƯỚT
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer > 0)
            {
                rb.linearVelocity = transform.right * slideSpeed;
                return; // Đang lướt thì CHẶN ngay, không cho chạy code bên dưới
            }
            else
            {
                // Hết thời gian lướt -> Nhả trạng thái
                isSliding = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                ChangeAnim("idle");
            }
        }

        // 2. NHẬN LỆNH BẮT ĐẦU LƯỚT
        if (Input.GetKeyDown(KeyCode.X) && isGrounded && !isSliding)
        {
            slideTimer = slideDuration;
            isSliding = true;
            ChangeAnim("slide");

            // TẠO KHÓI Ở ĐÂY NÀY:
            if (smoke != null && VFX_Offset != null)
            {
                Instantiate(smoke, VFX_Offset.position, transform.rotation);
            }

            return;
        }

        // 3. XỬ LÝ ATTACK (Đang đánh thì không di chuyển)
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 4. THAO TÁC CƠ BẢN (Chỉ chạy khi không lướt và không đánh)
        if (isGrounded)
        {
            isJumping = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Invoke(nameof(ResetAttack), 0.5f);
                Attack();
                return;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
                Invoke(nameof(ResetAttack), 0.5f);
                return;
            }
        }

        // 5. ANIMATION VÀ DI CHUYỂN BÌNH THƯỜNG
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            ChangeAnim("fall");
        }
        else if (Mathf.Abs(horizontal) > 0.1f) // Dùng else if để xét tuần tự
        {
            ChangeAnim("run");
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

            // horizontal > 0 ? 0 : 180 có nghĩa là nếu đang đi sang phải thì xoay 0 độ, còn nếu đang đi sang trái thì xoay 180 độ để mặt hướng về bên trái
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("idle");

        DeActiveAttack();

        SavePoint();

        UIManager.instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);

    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }


    // Thêm hàm OnTriggerEnter2D để xử lý các va chạm xuyên thấu (Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            //Debug.Log("Coin" + collision.gameObject.name);
            coin++; // Tăng số lượng coin đã thu thập
            UIManager.instance.SetCoin(coin); // Cập nhật số lượng coin lên UI
            PlayerPrefs.SetInt("coin", coin); // Lưu số lượng coin vào PlayerPrefs để có thể truy cập lại sau này
            Destroy(collision.gameObject); // Xóa đồng xu khi va chạm
        }
        if (collision.CompareTag("DeathZone"))
        {
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f); // Sau 1 giây sẽ gọi lại hàm OnInit để reset vị trí và trạng thái của nhân vật
        }
    }

}