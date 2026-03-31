using UnityEngine;

public class BossGlobalStat
{
    public static int maxHealth = 500;
    public static int exp = 100;
    public static int attackDamage = 50;

public static void Reset()
{
    maxHealth = 500;
    exp = 100;
    attackDamage = 50;
}
public static void IncreaseStat()
{
    maxHealth += 100;
    exp += 100;
    attackDamage += 25;
}
}
