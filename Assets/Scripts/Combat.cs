using System;
using Unity.Mathematics;
using UnityEngine;


public class Combat : MonoBehaviour
{
    #region Attributes

    public int attackDamage;
    public int damageReduction;
    public int baseDMGReduction;
    private Timer iFrameTimer;
    public float iFrameDuration = 1;

    private Health health;
    private IDamagable damagableCharacter;

    #endregion

    #region build-in function

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

     #endregion

    #region Upgrade stats
    public void UpgradeMaxHealth()
    {
        health.maxHealth = (int) Math.Ceiling(health.maxHealth * 1.05f);
    }
    public void SetMaxHealth(int newMaxHealth)
    {
        health.maxHealth = newMaxHealth;
        health.IncreaseHealth(newMaxHealth);
    }
    public void UpgradeCombatDMG()
    {
        attackDamage = (int) Math.Ceiling(attackDamage * 1.25f);
    }
    public void UpgradeBaseDMGReduction()
    {
        baseDMGReduction = (int) Math.Ceiling(baseDMGReduction * 2.5f);
        if (baseDMGReduction > 60)
        {
            baseDMGReduction = 60;
        }
    }
    #endregion

    public void HealMissingHealth(double percent)
    {
        int missingHealth = health.maxHealth - health.healthPoint;

        health.IncreaseHealth((int) (missingHealth * percent));
    }

    #region Combat

    public void GetHit(int damageAmount)
    {
        //calculate damage
        int totaldamage = (int) (damageAmount * (1f - (damageReduction / 100f))); //current damage after reduction
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

    #endregion
    public void ApplyStresModifier(float stressRatio)
    {
        damageReduction = Mathf.RoundToInt(baseDMGReduction * stressRatio);    
    }

 

    #region I-Frame

    public bool IsIFrameEnable()
    {
        bool isIframeEnabled = iFrameTimer.IsRunning() && iFrameTimer.GetTimeRemaining() > 0;
        return isIframeEnabled;
    }

    #endregion

    public int GetCurrentHealth()
    {
        return health.healthPoint;
    }

    public int GetMaxHealth()
    {
        return health.maxHealth;
    }

}
