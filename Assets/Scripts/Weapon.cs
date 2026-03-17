using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attackDamage;
    private Player player;
    public Animator animator;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();

    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position , player.transform.position);
        if (distance <= 1)
        {
            animator.SetTrigger("trAttack");
            IDamagable enemy = player.GetComponent<IDamagable>();
            enemy.Hurt(attackDamage);
        }
    }
}
