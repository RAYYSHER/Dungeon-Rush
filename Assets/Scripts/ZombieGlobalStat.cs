using UnityEngine;

public class ZombieGlobalStat
{
    public static int maxHealth = 100;
    public static int exp = 5;
    public static int attackDamage = 5;

public static void IncreaseStat()
{
    maxHealth += 50;
    exp += 10;
    attackDamage += 5;
}

}
