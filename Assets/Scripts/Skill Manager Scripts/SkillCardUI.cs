using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private Image      _icon;
    [SerializeField] private TMP_Text   _nameText;
    [SerializeField] private TMP_Text   _descriptionText;
    [SerializeField] private TMP_Text   _levelText;
    [SerializeField] private Button     _chooseButton;
    private Action _onChoose;

    void Awake()
    {
        _chooseButton.onClick.AddListener(OnChooseClicked);
    }

    // ── Level-Up panel ───────────────────────────────────────────────────────

    public void SetupNewSkill(SkillData data, Action onChoose)
    {
        _onChoose             = onChoose;
        _icon.sprite          = data.icon;
        _nameText.text        = data.skillName;
        _descriptionText.text = data.briefDescription;
        _levelText.text       = "NEW  Lv.1";
        _chooseButton.gameObject.SetActive(true);
        _chooseButton.interactable = true;
    }

    public void SetupUpgrade(SkillInstance skill, Action onChoose)
    {
        _onChoose       = onChoose;
        _icon.sprite    = skill.data.icon;
        _nameText.text  = skill.data.skillName;
        _levelText.text = skill.IsMaxLevel
                            ? "MAX LEVEL"
                            : $"Lv.{skill.GetDisplayLevel()} → Lv.{skill.GetDisplayLevel() + 1}";

        SkillLevelData nextLevel = skill.IsMaxLevel
                                    ? skill.GetCurrentLevelData()
                                    : skill.data.levels[skill.GetDisplayLevel()];
        _descriptionText.text = nextLevel.capabilityDescription;

        _chooseButton.gameObject.SetActive(true);
        _chooseButton.interactable = !skill.IsMaxLevel;
    }

    // ★ ใหม่ — Quest Reward panel (ไม่มี Choose button เพราะใช้ Claim แทน)
    public void SetupQuestReward(QuestReward reward)
    {
        _onChoose             = null;
        _icon.sprite          = reward.SkillData.icon;
        _nameText.text        = reward.SkillData.skillName;
        _descriptionText.text = reward.GetLevelDescription();

        if (reward.IsUpgrade)
        {
            SkillInstance existing = SkillManager.Instance.GetSlot(reward.SlotIndex);
            int currentDisplay    = existing != null ? existing.GetDisplayLevel() : 1;
            _levelText.text       = $"Lv.{currentDisplay} → Lv.{reward.GetDisplayLevel()}";
        }
        else
        {
            _levelText.text = reward.GetDisplayLevel() == 1
                ? "NEW  Lv.1"
                : $"NEW  Lv.{reward.GetDisplayLevel()}";
        }
        // ★ เพิ่ม null check
        if (_chooseButton != null)
            _chooseButton.gameObject.SetActive(false);
    }

    private void OnChooseClicked() => _onChoose?.Invoke();
}