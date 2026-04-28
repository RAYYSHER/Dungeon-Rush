using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : MonoBehaviour
{
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text goalsText;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] Button acceptButton;
    [SerializeField] Button rejectButton;

    Action onAccept;
    Action onReject;

    void Awake()
    {
        gameObject.SetActive(false);
        acceptButton.onClick.AddListener(OnAcceptClicked);
        rejectButton.onClick.AddListener(OnRejectClicked);
    }

    public void Show(QuestData data, Action onAccept, Action onReject)
    {
        this.onAccept = onAccept;
        this.onReject = onReject;

        questNameText.text = data.questName;
        descriptionText.text = data.description;

        var sb = new StringBuilder();
        foreach (var goal in data.goals)
            sb.AppendLine($"• {goal.description} (0/{goal.targetCount})");
        goalsText.text = sb.ToString().TrimEnd();

        rewardText.text = string.IsNullOrEmpty(data.rewardDescription)
            ? "Reward: ???"
            : $"Reward: {data.rewardDescription}";

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnAcceptClicked()
    {
        onAccept?.Invoke();
        Hide();
    }

    void OnRejectClicked()
    {
        onReject?.Invoke();
        Hide();
    }
}
