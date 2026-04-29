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
    [SerializeField] private TMP_Text rankDescriptionText;

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

        // ดึงข้อมูลจาก GameStatTracker
        float stsScore  = GameStatTracker.Instance.GetSTSScore();
        float timeScore = GameStatTracker.Instance.GetTimeScore();

        // คำนวณ Rank
        RankCalculator calculator = new RankCalculator();
        calculator.Calculate(timeScore, stsScore, isWin);

        // เลือก Feedback
        FeedbackSelector selector = new FeedbackSelector(feedbackData);
        selector.Select(calculator.RankLetter, stsScore, timeScore);
        
        // แสดง Rank
        DisplayRank(calculator);

        // แสดง Feedback
        DisplayFeedback(selector);

        // แสดง Stats
        DisplayStats();

        StartCoroutine(SelectAfterFrame(_gameResultFirst));
        
    }

    private void DisplayRank(RankCalculator calculator)
    {
        if (rankLetterText != null)
            rankLetterText.text = calculator.RankLetter;

        if (rankLabelText != null)
            rankLabelText.text = $"[{calculator.RankLabel}]";

        if (rankDescriptionText != null)
            rankDescriptionText.text = feedbackData.GetRankDescription(calculator.RankLetter);
    }

    private void DisplayFeedback(FeedbackSelector selector)
    {
        // Positive
        if (positiveFeedback1Text != null)
            positiveFeedback1Text.text = selector.PositiveFeedbacks.Count > 0
                ? $"+ {selector.PositiveFeedbacks[0]}"
                : "";

        if (positiveFeedback2Text != null)
            positiveFeedback2Text.text = selector.PositiveFeedbacks.Count > 1
                ? $"+ {selector.PositiveFeedbacks[1]}"
                : "";

        // Negative
        if (negativeFeedbackText != null)
            negativeFeedbackText.text = selector.NegativeFeedbacks.Count > 0
                ? $"- {selector.NegativeFeedbacks[0]}"
                : "";
    }

    private void DisplayStats()
    {
        if (majorStatValueText != null)
            majorStatValueText.text = 
                $"{GameStatTracker.Instance.GetSTSScore():F1}";

        // if (subStat1ValueText != null)
        //     subStat1ValueText.text  =  $"{GameStatTracker.Instance.ClearTime / 60f:F2}";

        if (subStat1ValueText != null)
        subStat1ValueText.text = FormatTime(GameStatTracker.Instance.ClearTime);

        if (subStat2ValueText != null)
            subStat2ValueText.text  = 
                $"{GameStatTracker.Instance.Eliminations}";
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs    = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
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
