using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Animator animator;
    private PlayerController controller;
    private Combat combat;
    private HurtEffect hurtEffect;
    private LevelSystem levelSystem;
    private Stress stressSystem;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        combat = GetComponent<Combat>();
        levelSystem = GetComponent<LevelSystem>();
        animator = GetComponent<Animator>();
        hurtEffect = GetComponent<HurtEffect>();
        stressSystem = GetComponent<Stress>();
    }

    //read joystick's inputActions and return to player(gameObject),
    // making player moves in the desire direction.
    private void Update()
    {
        controller.HandleJoystickInput();
    }

    private void FixedUpdate()
    {
        controller.Control();
    }

    public void IncreaseMainStat()
    {
        
        combat.HealMissingHealth(0.05);             //heal missing health
        combat.UpgradeMaxHealth();                  //increase maxhealth
        combat.UpgradeCombatDMG();                  //increase combatdamage
        combat.UpgradeBaseDMGReduction();           //increase damage reduction       
        //increase health regen
        stressSystem.ApplySTStoDMGReduction();      // applying to combat
        stressSystem.DecreaseSTSPercent(0.20f);     // Decrease flat amount of STS immediately

    }

    public void GetXP(int xp)
    {
        levelSystem.GainXP(xp);
    }

    public void Hurt(int damageAmount)
    {  
       if (combat.IsIFrameEnable() == false)
       {
            animator.SetTrigger("trGetHit");
            hurtEffect.TriggerHurt();  
            Debug.Log("hurt!");
            combat.GetHit(damageAmount);
       } 
    }
    
    public void Die()
    {
        // wait for anination

        // close the player code
        gameObject.SetActive(false);
    }
}
