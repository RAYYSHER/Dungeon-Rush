using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar healthbar;
    public int healthPoint;
    public int maxHealth;
    public float healthRegenPercent;

    void Awake()
    {
        if (GetComponentInChildren<HealthBar>() != null)
        {
            healthbar = GetComponentInChildren<HealthBar>();    
        }
    }

    void Start()
    {
        healthPoint = maxHealth;
        if (healthbar != null)
        {
            healthbar.UpdateHealthBar(healthPoint, maxHealth);    
        }
        
    }

    public void IncreaseHealth(int amount)
    {
        if (healthPoint + amount <= maxHealth)
        {
            healthPoint += amount;
        }
        else
        {
            healthPoint = maxHealth;
            
        }

        if (healthbar != null)
        {
            healthbar.UpdateHealthBar(healthPoint, maxHealth);    
        }
    }

    public bool DecreaseHealth(int amount)
    {
        
        if (healthPoint - amount > 0)
        {
            healthPoint -= amount;
            if (healthbar != null)
            {
                healthbar.UpdateHealthBar(healthPoint, maxHealth);    
            }
            return false;
        }
        else
        {
            healthPoint = 0;
            if (healthbar != null)
            {
                healthbar.UpdateHealthBar(healthPoint, maxHealth);
                healthbar.gameObject.SetActive(false);    
            }
            return true;
        }
    }
}

