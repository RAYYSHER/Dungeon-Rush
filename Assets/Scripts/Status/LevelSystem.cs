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

    void Awake()
    {
        player = GetComponent<Player>();
        positiveEffect = GetComponent<PositiveEffect>();
    } 
    void Start()
    {
        level = 1;
        exp = 0;
        nextlevelExp = 10;
        if (levelBar != null)
        {
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);
        }
    }

    public void GainXP(int xp)
    {
        //exp = exp + xp;
        exp += xp;

        if (levelBar != null)
        {
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);
        }

        if (exp >= nextlevelExp)
        {
            LevelUp();
        }
        // Debug.Log(exp + " exp gained");
    }

    void LevelUp()
    {
        level++;
        exp = 0; //cut of excess

        //increase levelUpExp needed
        nextlevelExp *= 2;              //nextLevelExp = nextlevelExp * 2;

        if (levelBar != null)
        {
            levelBar.UpdateLevelBar(exp, nextlevelExp, level);
        }

        positiveEffect?.TriggerLevelUp();
        player.IncreaseMainStat();
    } 




}
