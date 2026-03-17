using System.Threading;
using UnityEngine;

public class IdleState : IState
{
    private float timer;
    private float randomTime;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.Range(2.5f, 4f); // Đứng im từ 2.5s đến 4s
    }

    public void OnExecute(Enemy enemy)
    {
        // Cộng dồn thời gian đã đứng im
        timer += Time.deltaTime;

        // Nếu đứng đủ thời gian thì chuyển sang trạng thái đi tuần
        if (timer > randomTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
