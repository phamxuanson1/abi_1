using UnityEngine;
using UnityEngine.SceneManagement; // Dùng nếu bạn muốn có nút chuyển về Main Menu

public class PauseMenu : MonoBehaviour
{
    // Biến static để các script khác có thể dễ dàng kiểm tra xem game có đang pause không
    public static bool GameIsPaused = false;

    // Kéo thả Panel PauseMenuUI vào đây trên Inspector
    public GameObject pauseMenuUI;

    void Update()
    {
        // Nhấn nút Esc để bật/tắt pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Ẩn UI
        Time.timeScale = 1f;          // Cho thời gian chạy lại bình thường
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Hiện UI
        Time.timeScale = 0f;          // Dừng thời gian trong game
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Đang thoát game...");
        Application.Quit(); // Lệnh này sẽ hoạt động khi bạn build game ra file chạy
    }
}