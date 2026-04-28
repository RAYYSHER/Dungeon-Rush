using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FloorBoss : MonoBehaviour, IDamagable
{
    #region Attributes

    [SerializeField] private BossHealthBar bossHealthBar;
    private Animator animator;
    private Player player;
    private NavMeshAgent agent;
    private AnimatorController ac;
    private Combat combat;
    private Rigidbody rb;
    public static List<FloorBoss> bossLists = new List<FloorBoss>();
    public int exp;
    private HurtEffect hurtEffect;

    #endregion

    #region Build-in Function

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindAnyObjectByType<Player>();
        agent = GetComponent<NavMeshAgent>();
        ac = GetComponent<AnimatorController>();
        combat = GetComponent<Combat>();
        rb = GetComponent<Rigidbody>();
        hurtEffect = GetComponent<HurtEffect>();
    }

    void Start()
    {
        agent.speed = 1.5f;
        agent.angularSpeed = 60;

        if (bossHealthBar != null)
        {
            bossHealthBar.SetBoss(this);
        }
    }

    void Update()
    {
        if (Vector3.Distance(agent.transform.position , player.transform.position) <= 15)
        {
            agent.destination = player.transform.position;    
        }
        else
        {
            agent.ResetPath();
        }
        
        if (agent.velocity.magnitude != 0)
        {
            ac.SetWalk();
        }
        else
        {
            ac.SetIdle();
        }
    }

    #endregion

    #region Method

    public void SetStatToGlobal()
    {
        combat.SetMaxHealth(BossGlobalStat.maxHealth);
        combat.attackDamage = BossGlobalStat.attackDamage;
        exp = BossGlobalStat.exp;

        if (bossHealthBar != null)
        {
            bossHealthBar.UpdateBossHealthBar(combat.GetCurrentHealth(), BossGlobalStat.maxHealth);
        }
    }

    public void Hurt(int damageAmount)
    {
        combat.GetHit(damageAmount);
        hurtEffect.TriggerHurt();
        
        if (bossHealthBar != null)
        {
            bossHealthBar.UpdateBossHealthBar(combat.GetCurrentHealth(), combat.GetMaxHealth());   
        }

    }

    public void Die()
    {
        GameStatTracker.Instance?.StopTimer();
     GameStatTracker.Instance?.AddElimination(); 
        animator.SetTrigger("Dead");
        enabled = false;
        GetComponent<Weapon>().enabled = false;

        rb.constraints |= RigidbodyConstraints.FreezeRotationY 
                        | RigidbodyConstraints.FreezePositionX 
                        | RigidbodyConstraints.FreezePositionZ;

        player.GetXP(exp);

        //Result panel showed
        FindFirstObjectByType<GameResultUI>().ShowResult(true);
    }

    void OnEnable()
    {
        bossLists.Add(this);
    }

    void OnDisable()
    {
        bossLists.Remove(this);
    }

    #endregion
}
