using UnityEngine;

[System.Serializable]
public class SkillLevelData
{
    [TextArea(1, 2)]
    public string capabilityDescription;   // "Heal 6% of max HP"

    public float primaryValue;             // ค่าหลักที่ใช้คำนวณ เช่น 0.06 = 6%
    public float secondaryValue;           // ค่าสำรอง (ถ้า skill ต้องการ) ไม่ใช้ก็ปล่อย 0
}