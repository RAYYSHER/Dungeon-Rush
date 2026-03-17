using System.Xml.XPath;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int level;
    public int exp;
    public int nextlevelExp;
    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    } 
    void Start()
    {
        level = 1;
        exp = 0;
        nextlevelExp = 10;
    }

    public void GainXP(int xp)
    {
        //exp = exp + xp;
        exp += xp;
        
        if (exp >= nextlevelExp)
        {
            LevelUp();
        }
        Debug.Log(exp + " exp gained");
    }

    void LevelUp()
    {
        level++;
        exp = 0; //cut of excess

        //increase levelUpExp needed
        nextlevelExp *= 2;              //nextLevelExp = nextlevelExp * 2;

        player.IncreaseMainStat();
    } 




}
