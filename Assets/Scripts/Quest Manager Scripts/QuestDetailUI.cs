using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : MonoBehaviour
{
    [SerializeField] TMP_Text  questNameText;
    [SerializeField] TMP_Text  descriptionText;
    [SerializeField] TMP_Text  goalsText;
    [SerializeField] Button    acceptButton;
    [SerializeField] Button    rejectButton;

    [Header("Reward Preview")]
    [SerializeField] SkillCardUI _rewardCard;   // drag SkillCardUI ใน panel นี้มาใส่

    Action onAccept;
    Action onReject;

    void Awake()
    {
        gameObject.SetActive(false);
        acceptButton.onClick.AddListener(OnAcceptClicked);
        rejectButton.onClick.AddListener(OnRejectClicked);
    }

    public void Show(QuestData data, QuestReward reward, Action onAccept, Action onReject)
    {
        this.onAccept = onAccept;
        this.onReject = onReject;

        questNameText.text   = data.questName;
        descriptionText.text = data.description;

        var sb = new StringBuilder();
        foreach (var goal in data.goals)
            sb.AppendLine($"• {goal.description} (0/{goal.targetCount})");
        goalsText.text = sb.ToString().TrimEnd();

        // ★ แสดง reward preview
        if (_rewardCard != null)
        {
            if (reward != null)
            {
                _rewardCard.gameObject.SetActive(true);
                _rewardCard.SetupQuestReward(reward);
            }
            else
            {
                _rewardCard.gameObject.SetActive(false);
            }
        }

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