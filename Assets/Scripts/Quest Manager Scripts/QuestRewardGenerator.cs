using UnityEngine;

[System.Serializable]
public class AlternativeRewardConfig
{
    public Sprite    icon;
    public string    label;
    [TextArea] 
    public string    description;
    public float     value;
}

public class QuestRewardGenerator : MonoBehaviour
{
    public static QuestRewardGenerator Instance { get; private set; }

    [Header("Alternative Rewards")]
    [SerializeField] private AlternativeRewardConfig restoreHP;
    [SerializeField] private AlternativeRewardConfig reduceStress;
    [SerializeField] private AlternativeRewardConfig extendTime;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Public ───────────────────────────────────────────────────────────────

    

    public QuestReward Generate()
    {
        if (SkillManager.Instance == null) return null;

        return SkillManager.Instance.HasEmptySlot
            ? GenerateNewSkillReward()
            : GenerateUpgradeReward();
    }

    public QuestReward GenerateAlternative()
    {
        int roll = Random.Range(0, 3);
        AlternativeRewardConfig config = roll switch
        {
            0 => restoreHP,
            1 => reduceStress,
            _ => extendTime
        };

        RewardType type = roll switch
        {
            0 => RewardType.RestoreHP,
            1 => RewardType.ReduceStress,
            _ => RewardType.ExtendTime
        };

        return QuestReward.ForAlternative(type, config.value, config.label, config.description, config.icon);
    }

    // ── Private ──────────────────────────────────────────────────────────────

    private QuestReward GenerateNewSkillReward()
    {
        var pool = SkillManager.Instance.GetSkillPool();
        var available = new System.Collections.Generic.List<SkillData>();

        foreach (SkillData skill in pool)
            if (!SkillManager.Instance.IsSkillOwned(skill))
                available.Add(skill);

        if (available.Count == 0)
            return GenerateUpgradeReward();

        SkillData chosen      = available[Random.Range(0, available.Count)];
        int       maxLevel    = chosen.levels.Length - 1;
        int       targetLevel = Random.Range(0, maxLevel + 1);

        return QuestReward.ForNewSkill(chosen, targetLevel);
    }

    private QuestReward GenerateUpgradeReward()
    {
        var upgradeable = new System.Collections.Generic.List<int>();
        for (int i = 0; i < 2; i++)
        {
            SkillInstance slot = SkillManager.Instance.GetSlot(i);
            if (slot != null && !slot.IsMaxLevel)
                upgradeable.Add(i);
        }

        if (upgradeable.Count == 0)
            return GenerateAlternative();

        int           chosenIndex = upgradeable[Random.Range(0, upgradeable.Count)];
        SkillInstance chosen      = SkillManager.Instance.GetSlot(chosenIndex);
        int           maxLevel    = chosen.data.levels.Length - 1;
        int           targetLevel = Random.Range(chosen.CurrentLevel + 1, maxLevel + 1);

        return QuestReward.ForUpgrade(chosen.data, chosenIndex, targetLevel);
    }
}