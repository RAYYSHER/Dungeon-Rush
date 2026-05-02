using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("Slot Setting")]
    [SerializeField] private int            _slotIndex;         // 0 หรือ 1 — ตั้งใน Inspector

    [Header("Display")]
    [SerializeField] private Image          _icon;
    [SerializeField] private Image          _cooldownOverlay;   // Image type = Filled, fill method = Radial360
    [SerializeField] private TMP_Text       _levelText;
    [SerializeField] private GameObject     _emptyState;        // แสดงเมื่อยังไม่มี skill

    private ActiveSkillExecutor _executor;

    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Awake()
    {
        _executor = FindFirstObjectByType<ActiveSkillExecutor>();
    }

    void OnEnable()
    {
        if (SkillManager.Instance == null) return;
        SkillManager.Instance.OnSkillsChanged += Refresh;
    }

    void OnDisable()
    {
        if (SkillManager.Instance == null) return;
        SkillManager.Instance.OnSkillsChanged -= Refresh;
    }

    void Start()
    {
        SkillManager.Instance.OnSkillsChanged += Refresh;
        Refresh();
    }

    void Update()
    {
        UpdateCooldownOverlay();
    }

    // ─────────────────────────────────────────
    //  Private Methods
    // ─────────────────────────────────────────

    private void Refresh()
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(_slotIndex);

        bool hasSkill = skill != null;
        _emptyState.SetActive(!hasSkill);
        _icon.gameObject.SetActive(hasSkill);
        _levelText.gameObject.SetActive(hasSkill);
        _cooldownOverlay.gameObject.SetActive(hasSkill);

        if (!hasSkill) return;

        _icon.sprite    = skill.data.icon;
        _levelText.text = $"Lv.{skill.GetDisplayLevel()}";
    }

    private void UpdateCooldownOverlay()
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(_slotIndex);

        if (skill == null || skill.data.skillType != SkillType.Active)
        {
            _cooldownOverlay.fillAmount = 0f;
            return;
        }

        float remaining = _executor.GetCooldownRemaining(_slotIndex);
        _cooldownOverlay.fillAmount = remaining / skill.data.cooldown;
    }
}