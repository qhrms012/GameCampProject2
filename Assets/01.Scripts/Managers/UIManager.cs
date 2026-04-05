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
        AudioManager.Instance.PlayBgm(Bgm.Main,false);
        gameOverPanel.SetActive(true);
        gameOverText.text = "Victory";
        AudioManager.Instance.PlaySfx(Sfx.Clear);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        AudioManager.Instance.PlayBgm(Bgm.Main,false);
        gameOverPanel.SetActive(true);
        gameOverText.text = "Lose";
        AudioManager.Instance.PlaySfx(Sfx.Dead);
        Time.timeScale = 0f;
    }
}
