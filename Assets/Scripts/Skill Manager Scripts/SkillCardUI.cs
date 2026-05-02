using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("Display")]
    [SerializeField] private Image      _icon;
    [SerializeField] private TMP_Text   _nameText;
    [SerializeField] private TMP_Text   _descriptionText;
    [SerializeField] private TMP_Text   _levelText;
    [SerializeField] private Button     _chooseButton;

    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Awake()
    {
        _chooseButton.onClick.AddListener(OnChooseClicked);
    }

    // ─────────────────────────────────────────
    //  Public Setup
    // ─────────────────────────────────────────

    // เรียกเมื่อ HasEmptySlot == true → แสดง skill ใหม่ที่ Lv.1
    public void SetupNewSkill(SkillData data, Action onChoose)
    {
        _onChoose = onChoose;

        _icon.sprite        = data.icon;
        _nameText.text      = data.skillName;
        _descriptionText.text = data.briefDescription;
        _levelText.text     = "NEW  Lv.1";
    }

    // เรียกเมื่อ slot เต็ม → แสดง skill ที่มีอยู่พร้อม level ปัจจุบัน
    public void SetupUpgrade(SkillInstance skill, Action onChoose)
    {
        _onChoose = onChoose;

        _icon.sprite        = skill.data.icon;
        _nameText.text      = skill.data.skillName;
        _levelText.text     = skill.IsMaxLevel
                                ? "MAX LEVEL"
                                : $"Lv.{skill.GetDisplayLevel()} → Lv.{skill.GetDisplayLevel() + 1}";

        // แสดง capability ของ level ถัดไป (หรือปัจจุบันถ้า max)
        SkillLevelData nextLevel = skill.IsMaxLevel
                                    ? skill.GetCurrentLevelData()
                                    : skill.data.levels[skill.GetDisplayLevel()];  // GetDisplayLevel = 1-based = index ถัดไป

        _descriptionText.text = nextLevel.capabilityDescription;

        _chooseButton.interactable = !skill.IsMaxLevel;
    }

    // ─────────────────────────────────────────
    //  Private
    // ─────────────────────────────────────────

    private Action _onChoose;

    private void OnChooseClicked()
    {
        _onChoose?.Invoke();
    }
}