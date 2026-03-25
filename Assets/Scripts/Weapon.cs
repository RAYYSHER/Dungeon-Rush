using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attackDamage;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    private Player player;
    public Animator animator;
    private Transform enemyRoot;
    public Timer attackTimer;
    private Combat combat;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        attackTimer = new Timer(attackCooldown);

        var combat = GetComponentInParent<Combat>();
        if (combat != null)
        {
            enemyRoot = combat.transform;
            Debug.Log($"[Weapon] Combat found on: {enemyRoot.name}");
        }

        else
        {
            Debug.LogError("[Weapon] ไม่เจอ Combat เลย!");
            enemyRoot = transform;
        }

        
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();

    }
    void Update()
    {
        attackTimer.Update(Time.deltaTime);

        float distance = Vector3.Distance(enemyRoot.position , player.transform.position);
        
         

        if (distance <= attackRange && !attackTimer.IsRunning())
        {
            attackTimer.Start();

            if (animator != null)
            {
                animator.SetTrigger("trAttack");    
            }

            int totalDamage = attackDamage + (combat != null ? combat.attackDamage : 0 );

            IDamagable enemy = player.GetComponent<IDamagable>();
            enemy.Hurt(totalDamage);
            Debug.Log($"Hit for {totalDamage} (weapon: {attackDamage} + combat: {(combat != null ? combat.attackDamage : 0)})");
        }
    }
}
