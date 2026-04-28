using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameResultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;
    
    [SerializeField] private string mainMenuScene = "MainMenuScene";

    [SerializeField] private GameObject _gameResultFirst;

    [Header("Freeze Setting")]
    public float freezeDelay = 3f;

    void Start()
    {
        _gameResultPanel.SetActive(false);
    }

    public void ShowResult(bool isWin)
    {
        // Time.timeScale = 0f;

        _gameResultPanel.SetActive(true);

        if (isWin)
        {
            resultText.text = "VICTORY";
            StartCoroutine(FreezeAfterDelay());
        }
        else
        {
            resultText.text = "DEFEAT";
        }

        // Controller(Joystick) automatically highlights RESTART button
        // EventSystem.current.SetSelectedGameObject(_gameResultFirst);
        StartCoroutine(SelectAfterFrame(_gameResultFirst));
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ZombieGlobalStat.Reset();
        BossGlobalStat.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    private IEnumerator SelectAfterFrame(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }

    private IEnumerator FreezeAfterDelay()
    {
        yield return new WaitForSeconds(freezeDelay);
        Time.timeScale = 0f;
    }

}
