using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _gameResultPanel;
    [SerializeField] private GameObject _gameResultFirst;

    [Header("Result")]
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;
    
    [SerializeField] private string mainMenuScene = "MainMenuScene";

    [Header("Rank")]
    [SerializeField] private TMP_Text rankLetterText;
    [SerializeField] private TMP_Text rankLabelText;

    [Header("Feedback")]
    [SerializeField] private TMP_Text positiveFeedback1Text;
    [SerializeField] private TMP_Text positiveFeedback2Text;
    [SerializeField] private TMP_Text negativeFeedbackText;

    [Header("Stats")]
    [SerializeField] private TMP_Text majorStatValueText;  // STS score
    [SerializeField] private TMP_Text subStat1ValueText;   // Clear Time
    [SerializeField] private TMP_Text subStat2ValueText;   // Eliminations


    [Header("Feedback Data")]
    [SerializeField] private FeedbackData feedbackData;

    
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

        // // ดึงข้อมูลจาก GameStatTracker
        // float stsScore  = GameStatTracker.Instance.GetSTSScore();
        // float timeScore = GameStatTracker.Instance.GetTimeScore();

        // // คำนวณ Rank
        // RankCalculator calculator = new RankCalculator();
        // calculator.Calculate(timeScore, stsScore);

        // // เลือก Feedback
        // FeedbackSelector selector = new FeedbackSelector(feedbackData);
        // selector.Select(calculator.RankLetter, stsScore, timeScore);
        

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
