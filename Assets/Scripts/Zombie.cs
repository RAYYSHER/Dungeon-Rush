using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamagable
{
    #region Attributes
    private Animator animator;
    private Player player;
    private NavMeshAgent agent;
    private AnimatorController ac;
    private Combat combat;
    private Rigidbody rb;
    public int exp;
    public static List<Zombie> zombieLists = new List<Zombie>(); 
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
        agent.speed = 2.5f;
        agent.angularSpeed = 100;
        agent.stoppingDistance = 1.2f; //ระยะหยุดเดินของ Zombie
    }
    void Update()
    {
        // Vector3 lookDirection = player.transform.position - transform.position;
        // FacingToPlayer(new Vector2(lookDirection.x, lookDirection.z)); 
        
        if (Vector3.Distance(agent.transform.position , player.transform.position) <= 10)
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
        combat.SetMaxHealth(ZombieGlobalStat.maxHealth);
        combat.attackDamage = ZombieGlobalStat.attackDamage;
        exp = ZombieGlobalStat.exp;
    }

    public void Hurt(int damageAmount)
    {
        combat.GetHit(damageAmount);
        hurtEffect.TriggerHurt();  
    }
    public void Die()
    {
        animator.SetTrigger("Dead");
        enabled = false;
        GetComponent<Weapon>().enabled = false;

        rb.constraints  |= RigidbodyConstraints.FreezeRotationY 
                        |  RigidbodyConstraints.FreezePositionX 
                        |  RigidbodyConstraints.FreezePositionZ;

        player.GetXP(exp);
    }

    void OnEnable()
    {
        zombieLists.Add(this);
        Debug.Log(zombieLists);    
    }

    void OnDisable()
    {
        zombieLists.Remove(this);
    }

    #endregion

}
