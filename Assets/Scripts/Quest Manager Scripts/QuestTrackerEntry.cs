using System.Text;
using TMPro;
using UnityEngine;

public class QuestTrackerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text goalsText;
    [SerializeField] TMP_Text timerText;     

    private IQuestTimer _timer;

    public void Init(QuestInstance quest, IQuestTimer timer = null)
    {
        questNameText.text = quest.data.questName;
        _timer = timer;

        if (timerText != null)
            timerText.gameObject.SetActive(timer != null);

        UpdateProgress(quest);
    }

    void Update()
    {
        if (_timer == null || timerText == null) return;
        if (!_timer.IsTimerActive()) return;

        float remaining = _timer.GetTimeRemaining();
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateProgress(QuestInstance quest)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < quest.data.goals.Length; i++)
        {
            var goal = quest.data.goals[i];
            sb.AppendLine($"{goal.description}: {quest.goalProgress[i]}/{goal.targetCount}");
        }
        goalsText.text = sb.ToString().TrimEnd();
    }
}