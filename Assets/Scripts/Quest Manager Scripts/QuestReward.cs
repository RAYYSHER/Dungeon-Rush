/// <summary>
/// Holds the resolved reward granted when a quest is claimed.
/// </summary>
public class QuestReward
{
    public SkillData SkillData   { get; private set; }
    public int       TargetLevel { get; private set; }  // 0-based
    public bool      IsUpgrade   { get; private set; }  // true = upgrading existing slot
    public int       SlotIndex   { get; private set; }  // valid only when IsUpgrade = true

    public static QuestReward ForNewSkill(SkillData data, int targetLevel)
    {
        return new QuestReward
        {
            SkillData   = data,
            TargetLevel = targetLevel,
            IsUpgrade   = false,
            SlotIndex   = -1
        };
    }

    public static QuestReward ForUpgrade(SkillData data, int slotIndex, int targetLevel)
    {
        return new QuestReward
        {
            SkillData   = data,
            TargetLevel = targetLevel,
            IsUpgrade   = true,
            SlotIndex   = slotIndex
        };
    }

    public int    GetDisplayLevel()      => TargetLevel + 1;
    public string GetLevelDescription()  => SkillData.levels[TargetLevel].capabilityDescription;
}