using UnityEngine;

// Giao diện (Interface) này quy định 3 hành động cơ bản của một trạng thái
public interface IState
{
    void OnEnter(Enemy enemy);    // Gọi 1 lần khi bắt đầu bước vào trạng thái
    void OnExecute(Enemy enemy);  // Gọi liên tục (như Update) trong suốt quá trình ở trạng thái đó
    void OnExit(Enemy enemy);     // Gọi 1 lần trước khi thoát ra để chuyển sang trạng thái khác
}