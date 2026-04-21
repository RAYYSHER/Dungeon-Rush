using UnityEngine;

public class ZombieGlobalStat
{
    public static int maxHealth = 100;
    public static int exp = 5;
    public static int attackDamage = 5;

public static void Reset()
{
    maxHealth = 100;
    exp = 5;
    attackDamage = 5;
}
public static void IncreaseStat()
{
    maxHealth += 75;
    exp += 10;
    attackDamage += 15;
}

}
