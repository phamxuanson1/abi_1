using UnityEngine;

public class AttackState : IState
{
    private float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving(); // Dừng lại khi tấn công
        enemy.Attack();     // Gọi hàm kích hoạt Animation tấn công
        //Debug.Log("Enemy is attacking!");
        timer = 0;

        // Nếu có mục tiêu (Player), quái sẽ tự động quay mặt về phía Player trước khi đánh
        if (enemy.Target != null)
        {
            // Kiểm tra xem vị trí X của Player lớn hơn hay nhỏ hơn vị trí X của quái
            bool isPlayerOnRight = enemy.Target.transform.position.x > enemy.transform.position.x;
            enemy.ChangeDirection(isPlayerOnRight);
        }
    }

    public void OnExecute(Enemy enemy)
    {
        // Cộng dồn thời gian đã tấn công
        timer += Time.deltaTime;

        // Đợi 1.5 giây (hoặc thời gian đủ để chạy hết animation Attack)
        if (timer >= 1.5f)
        {
            // Đánh xong thì chuyển về trạng thái đi tuần 
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        // Reset lại các biến nếu cần thiết khi thoát trạng thái tấn công
    }
}
