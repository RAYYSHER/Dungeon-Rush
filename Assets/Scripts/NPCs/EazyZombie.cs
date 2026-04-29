using UnityEngine;
using UnityEngine.AI;

public class EazyZombie : MonoBehaviour, IDamagable
{
    #region Attributes
    private Animator animator;
    private Player player;
    private NavMeshAgent agent;
    private AnimatorController ac;
    private Combat combat;
    private Rigidbody rb;
    public int exp = 5;
    private HurtEffect hurtEffect;
    public static EazyZombieSpawner[] eazyZombieSpawners;
    private EnemyDespawnBody despawnBody;

    [SerializeField] private AudioClip damageSoundClip;
    
    
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
        despawnBody = GetComponent<EnemyDespawnBody>();

        eazyZombieSpawners = FindObjectsByType<EazyZombieSpawner>(FindObjectsSortMode.None);
    }

    void Start()
    {
        agent.speed = 3.5f;
        agent.angularSpeed = 100;
        
        // SetStatToGlobal();
        // Debug.Log($"[Zombie] SetStatToGlobal → combat ID: {combat.GetInstanceID()}, attackDamage: {combat.attackDamage}");
    }
    void Update()
    {
        // Vector3 lookDirection = player.transform.position - transform.position;
        // FacingToPlayer(new Vector2(lookDirection.x, lookDirection.z)); 
        
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

    // public void SetStatToGlobal()
    // {
    //     combat.SetMaxHealth(ZombieGlobalStat.maxHealth);
    //     combat.attackDamage = ZombieGlobalStat.attackDamage;
    //     exp = ZombieGlobalStat.exp;
    // }

    public void Hurt(int damageAmount)
    {
        combat.GetHit(damageAmount);
        hurtEffect.TriggerHurt();  

        // play sound FX
        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);  
    }
    public void Die()
    {
        GameStatTracker.Instance?.AddElimination();

        animator.SetTrigger("Dead");
        GetComponent<Weapon>().enabled = false;

        rb.constraints  |= RigidbodyConstraints.FreezeRotationY 
                        |  RigidbodyConstraints.FreezePositionX 
                        |  RigidbodyConstraints.FreezePositionZ;
        player.GetXP(exp);

        despawnBody.Despawn();
        enabled = false;
    }
    void OnDisable()
    {
        foreach (EazyZombieSpawner eazyZombieSpawner in eazyZombieSpawners)
        {
            if (eazyZombieSpawner.zombieLists.Contains(this))
            {
                eazyZombieSpawner.zombieLists.Remove(this);
                break;
            }
        }
    }

    #endregion

}
