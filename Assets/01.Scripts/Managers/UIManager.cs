using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [SerializeField]
    GameObject gameOverPanel;

    [SerializeField]
    TextMeshProUGUI gameOverText;





    public void GameClear()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Victory";
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Lose";
        Time.timeScale = 0f;
    }
}
