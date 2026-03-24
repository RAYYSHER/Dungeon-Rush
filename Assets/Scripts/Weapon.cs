using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attackDamage;
    public float attackRange = 1f;
    private Player player;
    public Animator animator;
    private Transform enemyRoot;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();

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
        float distance = Vector3.Distance(enemyRoot.position , player.transform.position);
        
        Debug.Log($"[{enemyRoot.name}] enemyRoot.pos={enemyRoot.position} | player.pos={player.transform.position} | dist={distance}");

        if (distance <= attackRange)
        {
            if (animator != null)
            {
                animator.SetTrigger("trAttack");    
            }

            IDamagable enemy = player.GetComponent<IDamagable>();
            enemy.Hurt(attackDamage);
            Debug.Log("Hit");
        }
    }
}
