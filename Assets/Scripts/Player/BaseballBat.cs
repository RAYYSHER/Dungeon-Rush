using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    public int attackDamage;
    private Combat combat;
    void Awake()
    {
        combat = GetComponentInParent<Combat>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Comparetag to check it's the right gameobject or not.
        if(other.CompareTag("Enemy") == true)
        {
            IDamagable enemy = other.GetComponent<IDamagable>();

            enemy.Hurt(attackDamage + combat.attackDamage);
            Debug.Log("attacked");
        }
    }
}
