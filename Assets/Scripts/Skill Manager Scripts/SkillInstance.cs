[System.Serializable]
public class SkillInstance
{
    // ─────────────────────────────────────────
    //  References
    // ─────────────────────────────────────────

    public SkillData data;

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

    public int CurrentLevel { get; private set; }   // 0-based  (0 = Lv.1, 4 = Lv.5)
    public bool IsMaxLevel  => CurrentLevel >= data.levels.Length - 1;

    // ─────────────────────────────────────────
    //  Constructor
    // ─────────────────────────────────────────

    public SkillInstance(SkillData data)
    {
        this.data     = data;
        CurrentLevel  = 0;      // เริ่มที่ Lv.1 เสมอ
    }

    // ─────────────────────────────────────────
    //  Public Methods
    // ─────────────────────────────────────────

    public void Upgrade()
    {
        if (IsMaxLevel)
        {
            return;
        }

        CurrentLevel++;
    }

    public SkillLevelData GetCurrentLevelData()
    {
        return data.levels[CurrentLevel];
    }

    // Display level เป็น 1-based สำหรับแสดงบน UI
    public int GetDisplayLevel()
    {
        return CurrentLevel + 1;
    }
}