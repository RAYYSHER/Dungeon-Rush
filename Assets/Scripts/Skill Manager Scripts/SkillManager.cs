using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [Header("Main Skill Pool")]
    [SerializeField] private SkillData[] mainSkillPool;

    private SkillInstance[] _slots      = new SkillInstance[2];
    private int             _skillCount = 0;

    public event Action OnSkillsChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Queries ──────────────────────────────────────────────────────────────

    public bool HasEmptySlot            => _skillCount < 2;
    public int  SkillCount              => _skillCount;
    public SkillInstance GetSlot(int i) => (i >= 0 && i < 2) ? _slots[i] : null;

    // ★ ใหม่ — ให้ QuestRewardGenerator อ่าน pool ได้
    public SkillData[] GetSkillPool() => mainSkillPool;

    // ★ ใหม่ — เช็คว่า player มี skill นี้อยู่แล้วหรือเปล่า
    public bool IsSkillOwned(SkillData data)
    {
        for (int i = 0; i < _skillCount; i++)
            if (_slots[i].data == data) return true;
        return false;
    }

    // ── Level-Up Offers ──────────────────────────────────────────────────────

    public SkillData[] GetNewSkillOffers()
    {
        List<SkillData> available = new List<SkillData>();
        foreach (SkillData skill in mainSkillPool)
            if (!IsSkillOwned(skill))
                available.Add(skill);

        ShuffleList(available);
        int count = Mathf.Min(2, available.Count);
        SkillData[] offers = new SkillData[count];
        for (int i = 0; i < count; i++) offers[i] = available[i];
        return offers;
    }

    public SkillInstance[] GetUpgradeOffers() => new SkillInstance[] { _slots[0], _slots[1] };

    // ── Actions — Level-Up panel ─────────────────────────────────────────────

    public void AddSkill(SkillData data)
    {
        if (!HasEmptySlot) return;
        _slots[_skillCount] = new SkillInstance(data);
        _skillCount++;
        OnSkillsChanged?.Invoke();
        UpdateAOERing();
    }

    public void UpgradeSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _skillCount) return;
        _slots[slotIndex].Upgrade();
        OnSkillsChanged?.Invoke();
        UpdateAOERing();
    }

    // ★ ใหม่ — apply quest reward (new skill หรือ upgrade) ที่ target level ทันที
    public void ApplyReward(QuestReward reward)
    {
        if (reward == null) return;

        if (!reward.IsUpgrade)
        {
            if (!HasEmptySlot) return;
            _slots[_skillCount] = new SkillInstance(reward.SkillData);
            while (_slots[_skillCount].CurrentLevel < reward.TargetLevel)
                _slots[_skillCount].Upgrade();
            _skillCount++;
        }
        else
        {
            int idx = reward.SlotIndex;
            if (idx < 0 || idx >= _skillCount) return;
            while (_slots[idx].CurrentLevel < reward.TargetLevel)
                _slots[idx].Upgrade();
        }

        Debug.Log($"[SkillManager] Quest reward applied: {reward.SkillData.skillName} Lv.{reward.GetDisplayLevel()}");
        OnSkillsChanged?.Invoke();
        UpdateAOERing();
    }

    // ── Misc ─────────────────────────────────────────────────────────────────

    public void ResetAllSkills()
    {
        _slots[0] = null; _slots[1] = null; _skillCount = 0;
        OnSkillsChanged?.Invoke();
    }

    private void UpdateAOERing()
    {
        AOEVFXHandler aoeVFX = FindFirstObjectByType<AOEVFXHandler>();
        if (aoeVFX == null) return;
        for (int i = 0; i < 2; i++)
        {
            SkillInstance slot = _slots[i];
            if (slot == null || slot.data.effectType != SkillEffectType.AOEDamage) continue;
            aoeVFX.ShowRadiusRing(slot.GetCurrentLevelData().primaryValue);
            return;
        }
        aoeVFX.HideRadiusRing();
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