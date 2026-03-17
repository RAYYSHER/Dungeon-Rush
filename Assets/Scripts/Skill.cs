using UnityEngine;
using UnityEngine.InputSystem;

public class Skill : MonoBehaviour
{
    private Health health;
    private Timer cooldown;
    private int timeDuration = 15;
    void Awake()
    {
        health = GetComponent<Health>();
        cooldown= new Timer(timeDuration);
    }
    public void Heal(InputAction.CallbackContext context)
    {
        if (cooldown.IsRunning() && cooldown.GetTimeRemaining() > 0)
        {
            return;
        }

        health.IncreaseHealth(health.maxHealth * 30/100);
        cooldown.Start();
        
    }
    void Update()
    {
        cooldown.Update(Time.deltaTime);
        if (cooldown.IsRunning())
        {
            Debug.Log(cooldown.GetTimeRemaining());
        }
    }
}
