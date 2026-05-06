using UnityEngine;

public enum RewardType
{
    Skill,          // skill ใหม่หรือ upgrade
    RestoreHP,
    ReduceStress,
    ExtendTime
}

public class QuestReward
{
    public RewardType RewardType  { get; private set; }

    // Skill reward
    public SkillData  SkillData   { get; private set; }
    public int        TargetLevel { get; private set; }  // 0-based
    public bool       IsUpgrade   { get; private set; }
    public int        SlotIndex   { get; private set; }

    // Alternative reward
    public float      Value       { get; private set; }  // HP / STS / seconds
    public string     Label       { get; private set; }  // ชื่อที่แสดงใน UI
    public string  Description    { get; private set; }
    public Sprite     Icon        { get; private set; }

    // ── Skill ────────────────────────────────────────────────────────────────
    public static QuestReward ForAlternative(RewardType type, float value, string label, string description, Sprite icon)
    {
        return new QuestReward
        {
            RewardType  = type,
            Value       = value,
            Label       = label,
            Description = description,
            Icon        = icon
        };
    }

    public static QuestReward ForUpgrade(SkillData data, int slotIndex, int targetLevel)
    {
        return new QuestReward
        {
            RewardType  = RewardType.Skill,
            SkillData   = data,
            TargetLevel = targetLevel,
            IsUpgrade   = true,
            SlotIndex   = slotIndex
        };
    }

        public static QuestReward ForNewSkill(SkillData data, int targetLevel)
    {
        return new QuestReward
        {
            RewardType  = RewardType.Skill,
            SkillData   = data,
            TargetLevel = targetLevel,
            IsUpgrade   = false,
            SlotIndex   = -1
        };
    }


    public static QuestReward ForAlternative(RewardType type, float value, string label, string description)
    {
        return new QuestReward
        {
            RewardType  = type,
            Value       = value,
            Label       = label,
            Description = description
        };
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    public int    GetDisplayLevel()     => TargetLevel + 1;
    public string GetLevelDescription() => SkillData.levels[TargetLevel].capabilityDescription;
}