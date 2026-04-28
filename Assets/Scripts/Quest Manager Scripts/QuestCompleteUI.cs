using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCompleteUI : MonoBehaviour
{
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] Button claimButton; // replaced by skill button when skill system is ready

    Action onClaim;

    void Awake()
    {
        gameObject.SetActive(false);
        claimButton.onClick.AddListener(OnClaimClicked);
    }

    public void Show(QuestInstance quest, Action onClaim)
    {
        this.onClaim = onClaim;

        // questNameText.text = $"{quest.data.questName} — Complete!";
        // rewardText.text = string.IsNullOrEmpty(quest.data.rewardDescription)
        //     ? "Reward: ???"
        //     : $"Reward: {quest.data.rewardDescription}";

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnClaimClicked()
    {
        onClaim?.Invoke();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
