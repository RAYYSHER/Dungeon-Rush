using UnityEngine;
using UnityEngine.UI;

public class DurationBarUI : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("Slot Setting")]
    [SerializeField] private int        _slotIndex;         // 0 หรือ 1 — ตั้งใน Inspector

    [Header("Display")]
    [SerializeField] private GameObject _barRoot;           // root ที่ toggle on/off
    [SerializeField] private Image      _durationOverlay;   // Image type = Filled, Radial360
    [SerializeField] private Image      _skillIcon;         // bg icon ของ skill

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

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
        _barRoot.SetActive(false);
        Refresh();
    }

    void Update()
    {
        UpdateDurationBar();
    }

    // ─────────────────────────────────────────
    //  Private Methods
    // ─────────────────────────────────────────

    private void Refresh()
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(_slotIndex);

        // แสดง bar เฉพาะ skill ที่มี duration
        bool hasDuration = skill != null && skill.data.duration > 0f;

        if (!hasDuration)
        {
            _barRoot.SetActive(false);
            return;
        }

        // อัปเดต icon ตาม skill ที่ slot นั้น
        if (_skillIcon != null)
            _skillIcon.sprite = skill.data.icon;
    }

    private void UpdateDurationBar()
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(_slotIndex);

        if (skill == null || skill.data.duration <= 0f)
        {
            _barRoot.SetActive(false);
            return;
        }

        bool isActive = _executor.IsDurationActive(_slotIndex);
        _barRoot.SetActive(isActive);

        if (!isActive) return;

        float remaining = _executor.GetDurationRemaining(_slotIndex);
        float total     = _executor.GetDurationTotal(_slotIndex);

         Debug.Log($"[DurationBarUI] remaining: {remaining} | total: {total} | fill: {remaining / total}");  // ← เพิ่ม

        _durationOverlay.fillAmount = remaining / total;
    }
}