using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slot ว่าง  → สุ่ม skill ที่ยังไม่มี + สุ่ม level 1–max
/// Slot เต็ม  → สุ่ม skill ที่มีอยู่ + สุ่ม level ที่สูงกว่า current เสมอ
/// ทุก skill max → return null (QuestCompleteUI จะซ่อน card อัตโนมัติ)
/// </summary>
public static class QuestRewardGenerator
{
    public static QuestReward Generate()
    {
        if (SkillManager.Instance == null) return null;

        return SkillManager.Instance.HasEmptySlot
            ? GenerateNewSkillReward()
            : GenerateUpgradeReward();
    }

    private static QuestReward GenerateNewSkillReward()
    {
        SkillData[] pool = SkillManager.Instance.GetSkillPool();

        List<SkillData> available = new List<SkillData>();
        foreach (SkillData skill in pool)
            if (!SkillManager.Instance.IsSkillOwned(skill))
                available.Add(skill);

        // ถ้า pool หมดแล้ว fallback ไป upgrade
        if (available.Count == 0)
            return GenerateUpgradeReward();

        SkillData chosen      = available[Random.Range(0, available.Count)];
        int       maxLevel    = chosen.levels.Length - 1;
        int       targetLevel = Random.Range(0, maxLevel + 1);

        return QuestReward.ForNewSkill(chosen, targetLevel);
    }

    private static QuestReward GenerateUpgradeReward()
    {
        List<int> upgradeable = new List<int>();
        for (int i = 0; i < 2; i++)
        {
            SkillInstance slot = SkillManager.Instance.GetSlot(i);
            if (slot != null && !slot.IsMaxLevel)
                upgradeable.Add(i);
        }

        // ทุก skill max → null (ไม่มี reward)
        if (upgradeable.Count == 0)
            return null;

        int           chosenIndex = upgradeable[Random.Range(0, upgradeable.Count)];
        SkillInstance chosen      = SkillManager.Instance.GetSlot(chosenIndex);
        int           maxLevel    = chosen.data.levels.Length - 1;
        int           targetLevel = Random.Range(chosen.CurrentLevel + 1, maxLevel + 1);

        return QuestReward.ForUpgrade(chosen.data, chosenIndex, targetLevel);
    }
}