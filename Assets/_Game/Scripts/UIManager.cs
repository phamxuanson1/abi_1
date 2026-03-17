using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [SerializeField] protected GameObject pausePanel;

    public static UIManager instance;
    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<UIManager>();
    //        }
    //
    //        return instance;
    //    }
    //}

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Text coinText;

    public void SetCoin(int coin)
    {
        coinText.text = "Coin: " + coin.ToString();
    }

    // Hàm để dừng game
    public void PauseGame()
    {
        pausePanel.SetActive(true); // Hiện bảng Pause
        Time.timeScale = 0f;        // Ngừng thời gian (mọi thứ đứng yên)
    }

    // Hàm để chơi tiếp
    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Ẩn bảng Pause
        Time.timeScale = 1f;         // Trả thời gian về bình thường
    }
}
