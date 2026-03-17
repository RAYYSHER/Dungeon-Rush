using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attackDamage;
    private Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();

    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position , player.transform.position);
        if (distance <= 1)
        {
            IDamagable enemy = player.GetComponent<IDamagable>();
            enemy.Hurt(attackDamage);
        }
    }
}
