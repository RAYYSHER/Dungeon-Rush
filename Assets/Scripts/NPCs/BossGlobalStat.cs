using UnityEngine;

public class BossGlobalStat
{
    public static int maxHealth = 500;
    public static int exp = 100;
    public static int attackDamage = 30;

public static void Reset()
{
    maxHealth = 500;
    exp = 100;
    attackDamage = 30;
}
public static void IncreaseStat()
{
    maxHealth += 200;
    exp += 100;
    attackDamage += 15;
}
}
