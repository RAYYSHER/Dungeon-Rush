using System;
using UnityEngine;


public class Combat : MonoBehaviour
{
    public int attackDamage;
    public int damageReduction;
    private Timer iFrameTimer;
    public float iFrameDuration = 1;

    private Health health;
    private IDamagable damagableCharacter;


    void Awake()
    {
        health = GetComponent<Health>();
        damagableCharacter = GetComponent<IDamagable>();
        iFrameTimer = new Timer(iFrameDuration);
    }

    void Update()
    {
        iFrameTimer.Update(Time.deltaTime);
    }
    public void UpgradeMaxHealth()
    {
        health.maxHealth = (int) Math.Ceiling(health.maxHealth * 1.5f);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        health.maxHealth = newMaxHealth;
        health.IncreaseHealth(newMaxHealth);
    }

    public void HealMissingHealth(double percent)
    {
        int missingHealth = health.maxHealth - health.healthPoint;

        health.IncreaseHealth((int) (missingHealth * percent));
    }

    public void UpgradeDMGReduction()
    {
        damageReduction = (int) Math.Ceiling(damageReduction + (damageReduction * 1.5f));
        if (damageReduction > 50)
        {
            damageReduction = 50;  //dmg reduction max is 50
        }
    }

    public void UpgradeCombatDMG()
    {
        attackDamage = (int) Math.Ceiling(attackDamage * 1.1f);
    }

    public void GetHit(int damageAmount)
    {
        //calculate damage
        int totaldamage = damageAmount * (1 - (damageReduction / 100)); //current damage after reduction
        if (totaldamage > 0 && health.healthPoint > 0)
        {
            //decrease health
            bool isDead = health.DecreaseHealth(totaldamage);
            if (isDead == true)
            {
                damagableCharacter.Die();
            }  
            else
            {
                iFrameTimer.Start();
            }
        }
    }

    public bool IsIFrameEnable()
    {
        bool isIframeEnabled = iFrameTimer.IsRunning() && iFrameTimer.GetTimeRemaining() > 0;
        return isIframeEnabled;
    }


}
