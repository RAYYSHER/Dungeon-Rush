using UnityEngine;

public enum SkillType   { Active, Passive }
public enum TargetType  { Self, Enemy, Area }
public enum SkillEffectType
{
    Heal,
    STSReduce,
    Shield,
    AOEDamage,
    DMGBuff,
    IncreaseEXP
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Identity")]
    public string       skillName;
    [TextArea(1, 3)]
    public string       briefDescription;
    public Sprite       icon;

    [Header("Type")]
    public SkillType    skillType;
    public TargetType   targetType;
    public SkillEffectType effectType; 

    [Header("Stats")]
    public float        stsCost;
    public float        cooldown;
    public float        duration;           // 0 = instant
    public float        castingTime;        // 0 = instant
    public float        distance;           // 0 = self
    public float        area;               // 0 = no area

    [Header("Level Capabilities (Max 5 Levels)")]
    public SkillLevelData[] levels = new SkillLevelData[5];
}