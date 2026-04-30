using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private Health health;
    private Timer cooldown;
    private int timeDuration = 15;
    private PositiveEffect positiveEffect;

    [Header("Cooldown UI")]
    [SerializeField] private GameObject cooldownOverlay;  // Panel บัง skill icon (Image สีดำ alpha ~0.6)
    [SerializeField] private Image cooldownFill;      // Direction: Top To Bottom

    void Start()
    {
        SetOverlayActive(false);
    }

    void Awake()
    {
        health = GetComponent<Health>();
        cooldown= new Timer(timeDuration);
        positiveEffect = GetComponent<PositiveEffect>();
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (cooldown.IsRunning() && cooldown.GetTimeRemaining() > 0)
        {
            return;
        }


        health.IncreaseHealth(health.maxHealth * 30/100);
        positiveEffect?.TriggerHeal();
        cooldown.Start();

        SetOverlayActive(true);
        if (cooldownFill != null)
            cooldownFill.fillAmount = 1f;
        
    }
    void Update()
    {
        cooldown.Update(Time.deltaTime);

        if (cooldownOverlay != null && cooldownOverlay.activeSelf)
        {
            Debug.Log(cooldown.GetTimeRemaining());

            float remaining = cooldown.GetTimeRemaining();

            if (cooldownFill != null)
                cooldownFill.fillAmount = remaining / timeDuration;

            if (remaining <= 0f)
                SetOverlayActive(false);
        }
    }

    private void SetOverlayActive(bool active)
    {
        if (cooldownOverlay != null)
            cooldownOverlay.SetActive(active);
    }
}
