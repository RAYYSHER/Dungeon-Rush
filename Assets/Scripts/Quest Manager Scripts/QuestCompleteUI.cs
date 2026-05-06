using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestCompleteUI : MonoBehaviour
{
    [SerializeField] Button      claimButton;

    [Header("Reward")]
    [SerializeField] SkillCardUI _rewardSkillCard;          // drag SkillCardUI ที่อยู่ใน panel นี้มาใส่
    [Header("Controller")]
    [SerializeField] private GameObject _firstSelected;     // ลาก ClaimButton ใส่

    private Action      _onClaim;
    private QuestReward _pendingReward;

    void Awake()
    {
        gameObject.SetActive(false);
        claimButton.onClick.AddListener(OnClaimClicked);
    }

    public void Show(QuestInstance quest, QuestReward reward, Action onClaim)
    {
        _onClaim       = onClaim;
        _pendingReward = reward;


        if (_rewardSkillCard != null)
        {
            if (reward != null)
            {
                _rewardSkillCard.gameObject.SetActive(true);
                _rewardSkillCard.SetupQuestReward(reward);
            }
            else
            {
                // ทุก skill max แล้ว — ซ่อน card
                _rewardSkillCard.gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(SelectAfterFrame(_firstSelected));
    }

    private void OnClaimClicked()
    {
        if (_pendingReward != null)
        {
            SkillManager.Instance.ApplyReward(_pendingReward);
            _pendingReward = null;
        }

        _onClaim?.Invoke();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private System.Collections.IEnumerator SelectAfterFrame(GameObject target)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }
}