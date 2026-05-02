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
        _hurtEffect = GetComponent<HurtEffect>();
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

            float drainRate = maxSts / penaltyDuration;
            DecreaseSTS(drainRate * Time.deltaTime);

            if (_penaltyTimer <= 0f)
            {
                EndPenalty();
            }
            return;
        }

        // ---- Logic ปกติ ----
        IncreaseSTS(timeStressRate * Time.deltaTime);

        // [แก้] เดิมใช้ if/else ตรงๆ ทำให้เมื่อ isUnderStress = false
        // จะเข้า else → DecreaseSTS ทันที → STS ลงจาก 100 นิดนึง
        // → penalty check ด้านล่างไม่ติดเพราะ sts < maxSts ไปแล้ว
        // [แก้] เลยเก็บค่า isUnderStress ไว้ใน wasSprinting ก่อน reset
        // เพื่อให้ penalty check ทำงานได้ก่อนที่ regen จะดึง STS ลง
        bool wasSprinting = isUnderStress;
        
        if (wasSprinting)
        {
            IncreaseSTS(timeStressRate * Time.deltaTime);
            isUnderStress = false;
        }

        // [แก้] ย้าย penalty check มาไว้ตรงนี้ก่อน regen
        // เดิมอยู่หลัง else DecreaseSTS → ทำให้ Dash ที่เติม STS ด้วย IncreaseSTS()
        // โดยตรงไม่เคย trigger penalty ได้เลย เพราะ regen ลดก่อนเสมอ
        if (sts >= maxSts && !_isInPenalty)
        {
            TriggerPenalty();
            return;
        }

        // [แก้] เปลี่ยนจาก else → if (!wasSprinting)
        // ให้ regen ทำงานต่อเนื่องจาก wasSprinting แทน isUnderStress ที่ถูก reset ไปแล้ว
        if (!wasSprinting)
        {
            DecreaseSTS(timeStressRegenRate * Time.deltaTime);
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
