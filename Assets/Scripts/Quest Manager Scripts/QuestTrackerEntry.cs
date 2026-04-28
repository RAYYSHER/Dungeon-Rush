using System.Text;
using TMPro;
using UnityEngine;

public class QuestTrackerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text goalsText;

    public void Init(QuestInstance quest)
    {
        questNameText.text = quest.data.questName;
        UpdateProgress(quest);
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
