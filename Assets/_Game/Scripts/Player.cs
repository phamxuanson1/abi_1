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


    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;


    private float horizontal;
    [SerializeField] private float jumpForce = 350;

    private int coin = 0;
    private Vector3 savePoint;
    // Start is called before the first frame update


    // Update is called once per frame

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    void Update()
    {
        //Debug.Log( CheckGrounded());

        isGrounded = CheckGrounded();

        //-1 -> 0 -> 1
        //horizontal = Input.GetAxisRaw("Horizontal");
        //verticle = Input.GetAxisRaw("Vertical");

        if(isDead)
        {
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            if(isAttack)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                Jump();
            }
            

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack
            if(Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                //Debug.Log("attack");
                Invoke(nameof(ResetAttack), 0.5f);
                Attack();
                isAttack = true;
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
                Invoke(nameof(ResetAttack), 0.5f);
                isAttack = true;
            }

        }

        //check falling
        if (!isGrounded && rb.linearVelocity.y < 0) // vận tốc y < 0 nghĩa là đang rơi xuống
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //moving
        if (Mathf.Abs(horizontal) >0.1f)
        {
            ChangeAnim("run");
            rb.linearVelocity = new Vector2(horizontal  * speed, rb.linearVelocity.y);

            //transform.localScale = new Vector3(horizontal, 1, 1);
            // Nếu horizontal là 1 (di chuyển sang phải), localScale sẽ là (1, 1, 1) => nhân vật hướng về bên phải
            // Nếu horizontal là -1 (di chuyển sang trái), localScale sẽ là (-1, 1, 1) => nhân vật hướng về bên trái
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0); // Quay nhân vật sang phải hoặc trái dựa trên giá trị horizontal
        }
        //idle 
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.linearVelocity = Vector2.zero;
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