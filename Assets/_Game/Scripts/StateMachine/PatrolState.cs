using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float randomTime;

    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f); // Đi tuần từ 3s đến 6s
    }

    public void OnExecute(Enemy enemy)
    {
        // Cộng dồn thời gian đã đi tuần
        timer += Time.deltaTime;

        if (enemy.Target != null)
        {
            // Nếu có mục tiêu (Player), quái sẽ tự động quay mặt về phía Player
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else 
            {
                enemy.Moving();
            }

                
        }
        else
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
