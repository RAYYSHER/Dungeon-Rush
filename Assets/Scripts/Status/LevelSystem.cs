using System.Xml.XPath;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int level;
    public int exp;
    public int nextlevelExp;
    private Player player;
    [SerializeField] private LevelBar levelBar;
    private PositiveEffect positiveEffect;
    [SerializeField] private SkillLevelUpUI skillLevelUpUI;

    void Awake()
    {
        player = GetComponent<Player>();
        positiveEffect = GetComponent<PositiveEffect>();
    } 
    void Start()
    {
        level = 1;
        exp = 0;
        nextlevelExp = 15;
        if (levelBar != null)
        {
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);
        }
    }

    public void GainXP(int xp)
    {
        exp += xp;

        // Debug.Log($"[LevelSystem] GainXP: +{xp} | total exp: {exp} | nextlevelExp: {nextlevelExp}");

        if (levelBar != null)
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);

        // ป้องกัน LevelUp ซ้อนกัน — ถ้า panel กำลังแสดงอยู่ให้รอก่อน
        if (exp >= nextlevelExp && !skillLevelUpUI.IsShowing)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        // Debug.Log($"[LevelSystem] LevelUp called → level: {level} | exp: {exp} | nextlevelExp: {nextlevelExp}");

        level++;
        exp = 0;
        nextlevelExp *= 2;

        if (levelBar != null)
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);

        positiveEffect?.TriggerLevelUp();
        player.IncreaseMainStat();
        skillLevelUpUI?.Show();
    } 




}
