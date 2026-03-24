using UnityEngine;

public class BossGlobalStat
{
    public static int maxHealth = 1000;
    public static int exp = 100;
    public static int attackDamage = 50;

public static void IncreaseStat()
{
    maxHealth += 50;
    exp += 100;
    attackDamage += 50;
}
}
