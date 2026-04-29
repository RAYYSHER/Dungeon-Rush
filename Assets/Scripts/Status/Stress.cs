using System;
using UnityEngine;

public class Stress : MonoBehaviour
{
    #region Attributes
    public float sts;
    public float maxSts = 100f;
    public float timeStressRate = 1f;
    public float timeStressRegenRate = 2f;
    private bool isUnderStress = false;

    [SerializeField] private StressBar stressBar;

    private Combat combat;


    #endregion

    #region Penalty
    
    [Header("Stress Penalty")]
    public float penaltyDuration = 5f;          // แก้ได้ใน Inspector
    private bool _isInPenalty = false;
    private float _penaltyTimer = 0f;
    private HurtEffect _hurtEffect;

    #endregion

    #region Build-in function
    void Awake()
    {
        combat = GetComponent<Combat>();
    }

    void Start()
    {
        sts = 0;


        if (stressBar != null)
        {
            stressBar.UpdateStressBar(sts, maxSts);
        }
    }

    void Update()
    {
         // ---- ระหว่างโดน Penalty ----
        if (_isInPenalty)
        {
            _penaltyTimer -= Time.deltaTime;
 
            // Drain STS จาก maxSts → 0 ภายใน penaltyDuration วินาที
            float drainRate = maxSts / penaltyDuration;
            DecreaseSTS(drainRate * Time.deltaTime);
 
            if (_penaltyTimer <= 0f)
            {
                EndPenalty();
            }
            return; // ข้าม logic ปกติระหว่าง penalty
        }
 
        // ---- Logic ปกติ ----
        IncreaseSTS(timeStressRate * Time.deltaTime);
 
        if (isUnderStress)
        {
            IncreaseSTS(timeStressRate * Time.deltaTime);
            isUnderStress = false;  // reset each frame, Movement.Walk() sets it true
        }
        else
        {
            DecreaseSTS(timeStressRegenRate * Time.deltaTime);
        }
 
        // ตรวจว่า STS เต็มหรือยัง
        if (sts >= maxSts && !_isInPenalty)
        {
            TriggerPenalty();
        }

    }

    #endregion

    #region Stress Stat

    public void IncreaseSTS(float amount)
    {
        sts = Mathf.Clamp(sts + amount, 0, maxSts);
        ApplySTStoDMGReduction();

        if (stressBar != null)
        {
            stressBar.UpdateStressBar(sts, maxSts);
        }
    }

    public void DecreaseSTS(float amount)
    {
        sts = Mathf.Clamp(sts - amount, 0, maxSts);
        ApplySTStoDMGReduction();

        if (stressBar != null)
        {
            stressBar.UpdateStressBar(sts, maxSts);
        }
    }
    
    public void SetUnderStress()
    {
        isUnderStress = true;
    }

    public void DecreaseSTSPercent(float percent)
    {
        float amount = sts * percent;
        DecreaseSTS(amount);
    }


    public void ApplySTStoDMGReduction()
    {
        float stressRatio = 1f - (sts / maxSts);
        combat.ApplyStresModifier(stressRatio);
    }

    #endregion

     #region Penalty
 
    void TriggerPenalty()
    {
        _isInPenalty = true;
        _penaltyTimer = penaltyDuration;
        _hurtEffect?.TriggerStressPenalty(penaltyDuration);
        Debug.Log("[Stress] Penalty triggered!");
    }
 
    void EndPenalty()
    {
        _isInPenalty = false;
        sts = 0f;                           // force STS → 0 เมื่อหมด penalty
 
        if (stressBar != null)
            stressBar.UpdateStressBar(sts, maxSts);
 
        ApplySTStoDMGReduction();
        Debug.Log("[Stress] Penalty ended.");
    }
 
    /// <summary>ให้ Movement / Controller ถามว่ากำลังโดน penalty อยู่ไหม</summary>
    public bool IsInPenalty() => _isInPenalty;
 
    #endregion
}
