using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private GameObject gameResultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;

    void Start()
    {
        gameResultPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowResult(bool isWin)
    {
        Time.timeScale = 0f;

        gameResultPanel.SetActive(true);

        if (isWin)
        {
            resultText.text = "VICTORY";
        }
        else
        {
            resultText.text = "DEFEAT";
        }

        //Controller(Joystick) automatically highlights RESTART button
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
