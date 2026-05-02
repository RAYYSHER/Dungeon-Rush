using UnityEngine;

public class PassiveSkillApplier : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Public Methods — called by other systems
    // ─────────────────────────────────────────

    // Player.GetXP() เรียกตรงนี้ก่อน pass ไปให้ LevelSystem
    public int ModifyEXPGain(int baseXP)
    {
        SkillInstance skill = FindPassiveSkill(SkillEffectType.IncreaseEXP);
        if (skill == null) return baseXP;

        float bonus     = skill.GetCurrentLevelData().primaryValue; // 0.10, 0.15 ...
        int   finalXP   = Mathf.RoundToInt(baseXP * (1f + bonus));

        Debug.Log($"[PassiveSkillApplier] EXP {baseXP} → {finalXP} (+{bonus * 100f}%)");
        return finalXP;
    }

    // ─────────────────────────────────────────
    //  Private Helpers
    // ─────────────────────────────────────────

    private SkillInstance FindPassiveSkill(SkillEffectType effectType)
    {
        for (int i = 0; i < 2; i++)
        {
            SkillInstance slot = SkillManager.Instance.GetSlot(i);

            if (slot == null)                                   continue;
            if (slot.data.skillType != SkillType.Passive)       continue;
            if (slot.data.effectType != effectType)             continue;

            return slot;
        }
        return null;
    }
}