using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float hp;
    public bool isDead => hp <= 0;
    private string currentAnimName;
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {
    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f); // Đợi 2 giây sau khi chơi animation "die" rồi mới gọi OnDespawn để xóa đối tượng khỏi game
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damage)
    {
        Debug.Log("Hit");

        // Nếu nhân vật chưa chết thì mới xử lý nhận sát thương
        if (!isDead)
        {
            hp -= damage;

            // Kiểm tra sau khi trừ máu, nếu máu <= 0 thì gọi hàm chết
            if (isDead)
            {
                hp = 0;
                OnDeath();
            }

            // Cập nhật giá trị máu mới lên thanh máu (UI)
            healthBar.SetNewHp(hp);

            // Sinh ra (Instantiate) hiệu ứng chữ sát thương bay lên tại vị trí của nhân vật + 1 đơn vị lên trên
            //Quaternion.identity có nghĩa là không xoay gì cả, giữ nguyên hướng mặc định của prefab
            //Loại_Biến tên_biến = Instantiate(vật_mẫu, vị_trí, góc_xoay);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }


}
