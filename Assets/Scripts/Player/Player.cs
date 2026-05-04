using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Animator animator;
    private PlayerController controller;
    private Movement movement;
    private Combat combat;
    private bool isAttacking = false;
    private HurtEffect hurtEffect;
    private LevelSystem levelSystem;
    private Stress stressSystem;
    private ShieldHandler shieldHandler;
    

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        levelSystem = GetComponent<LevelSystem>();
        animator = GetComponent<Animator>();
        hurtEffect = GetComponent<HurtEffect>();
        stressSystem = GetComponent<Stress>();
        shieldHandler = GetComponent<ShieldHandler>();
    }

    private void FixedUpdate()
    {
        controller.HandleJoystickInput();
        controller.Control();
    }

    public void IncreaseMainStat()
    {
        combat.HealMissingHealth(0.1);
        combat.UpgradeMaxHealth();
        combat.UpgradeCombatDMG();
        combat.UpgradeBaseDMGReduction();
        stressSystem.ApplySTStoDMGReduction();
        stressSystem.DecreaseSTSPercent(0.20f);
    }

    public void SetAttacking(bool value)
    {
        isAttacking = value;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    public void GetXP(int xp)
    {
        levelSystem.GainXP(xp);
    }

    public void Hurt(int damageAmount)
    {
        if (combat.IsIFrameEnable() || movement.IsDashing)
        return;

    // เช็ค shield ก่อน — รับ damage ที่เหลือหลัง shield หัก
    int remainingDamage = shieldHandler != null
        ? shieldHandler.AbsorbDamage(damageAmount)
        : damageAmount;

    if (remainingDamage <= 0) return;   // shield รับหมด player ไม่โดน

    if (!isAttacking)
        animator.SetTrigger("trGetHit");

    hurtEffect.TriggerHurt();
    combat.GetHit(remainingDamage);
    }
    
    public void Die()
    {
        FindFirstObjectByType<GameResultUI>().ShowResult(false);
        gameObject.SetActive(false);
    }
}