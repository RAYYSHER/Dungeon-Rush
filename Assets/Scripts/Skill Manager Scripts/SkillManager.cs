using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("Main Skill Pool")]
    [SerializeField] private SkillData[] mainSkillPool;     // ลาก ScriptableObject ทั้ง 6 ใส่ใน Inspector

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

    private SkillInstance[] _slots      = new SkillInstance[2];
    private int             _skillCount = 0;

    // ─────────────────────────────────────────
    //  Events
    // ─────────────────────────────────────────

    public event Action OnSkillsChanged;    // UI ฟังตรงนี้เพื่ออัปเดต HUD

    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ─────────────────────────────────────────
    //  Queries
    // ─────────────────────────────────────────

    public bool HasEmptySlot            => _skillCount < 2;
    public int  SkillCount              => _skillCount;
    public SkillInstance GetSlot(int i) => (i >= 0 && i < 2) ? _slots[i] : null;

    // ─────────────────────────────────────────
    //  Level Up Offers
    // ─────────────────────────────────────────

    // เรียกเมื่อ slot ว่างอยู่ → ส่ง SkillData ใหม่ 2 อันที่ยังไม่ได้เลือก
    public SkillData[] GetNewSkillOffers()
    {
        List<SkillData> available = new List<SkillData>();

        foreach (SkillData skill in mainSkillPool)
        {
            if (!IsOwned(skill))
                available.Add(skill);
        }

        ShuffleList(available);

        int count = Mathf.Min(2, available.Count);
        SkillData[] offers = new SkillData[count];
        for (int i = 0; i < count; i++)
            offers[i] = available[i];

        return offers;
    }

    // เรียกเมื่อ slot เต็ม → ส่ง skill ที่มีอยู่ทั้ง 2 มาให้เลือกอัปเกรด
    public SkillInstance[] GetUpgradeOffers()
    {
        return new SkillInstance[] { _slots[0], _slots[1] };
    }

    // ─────────────────────────────────────────
    //  Actions
    // ─────────────────────────────────────────

    public void AddSkill(SkillData data)
    {
        if (!HasEmptySlot) return;

        _slots[_skillCount] = new SkillInstance(data);
        _skillCount++;
        OnSkillsChanged?.Invoke();
    }

    public void UpgradeSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _skillCount) return;

        _slots[slotIndex].Upgrade();
        OnSkillsChanged?.Invoke();
    }

    public void ResetAllSkills()
    {
        _slots[0]    = null;
        _slots[1]    = null;
        _skillCount  = 0;
        OnSkillsChanged?.Invoke();
    }

    // ─────────────────────────────────────────
    //  Private Helpers
    // ─────────────────────────────────────────

    private bool IsOwned(SkillData data)
    {
        for (int i = 0; i < _skillCount; i++)
        {
            if (_slots[i].data == data) return true;
        }
        return false;
    }

    private void ShuffleList(List<SkillData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}