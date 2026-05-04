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
    public float penaltyDuration = 5f;
    private bool _isInPenalty = false;
    private float _penaltyTimer = 0f;
    private HurtEffect _hurtEffect;

    // Property ให้ script อื่นอ่านได้ง่าย
    public bool IsInPenalty => _isInPenalty;

    #endregion

    #region Build-in function

    void Awake()
    {
        combat      = GetComponent<Combat>();
        _hurtEffect = GetComponent<HurtEffect>();
    }

    void Start()
    {
        sts = 0;
        if (stressBar != null)
            stressBar.UpdateStressBar(sts, maxSts);
    }

    void Update()
    {
        // ---- ระหว่างโดน Penalty ----
        if (_isInPenalty)
        {
            _penaltyTimer -= Time.deltaTime;

            float drainRate = maxSts / penaltyDuration;
            DecreaseSTS(drainRate * Time.deltaTime);    // drain ลงเรื่อยๆ

            if (_penaltyTimer <= 0f)
                EndPenalty();

            return;     // ไม่รัน logic ปกติระหว่าง penalty
        }

        // ---- Logic ปกติ ----
        IncreaseSTS(timeStressRate * Time.deltaTime);

        bool wasSprinting = isUnderStress;
        if (wasSprinting)
        {
            IncreaseSTS(timeStressRate * Time.deltaTime);
            isUnderStress = false;
        }

        // เช็ค penalty ก่อน regen — ป้องกัน regen ลด STS จนไม่ trigger
        if (sts >= maxSts && !_isInPenalty)
        {
            TriggerPenalty();
            return;
        }

        if (!wasSprinting)
            DecreaseSTS(timeStressRegenRate * Time.deltaTime);
    }

    #endregion

    #region Stress Stat

    public void IncreaseSTS(float amount)
    {
        if (_isInPenalty) return;   // penalty กำลัง drain อยู่ — ห้ามเพิ่ม STS จากภายนอก

        sts = Mathf.Clamp(sts + amount, 0, maxSts);
        ApplySTStoDMGReduction();

        if (stressBar != null)
            stressBar.UpdateStressBar(sts, maxSts);
    }

    public void DecreaseSTS(float amount)
    {
        sts = Mathf.Clamp(sts - amount, 0, maxSts);
        ApplySTStoDMGReduction();

        if (stressBar != null)
            stressBar.UpdateStressBar(sts, maxSts);
    }

    public void SetUnderStress()
    {
        if (_isInPenalty) return;   // penalty อยู่ — ไม่รับ sprint stress
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
        _isInPenalty  = true;
        _penaltyTimer = penaltyDuration;
        GameStatTracker.Instance?.AddPenalty(); 
        _hurtEffect?.TriggerStressPenalty(penaltyDuration);   // แสงม่วงกะพริบตลอด penalty
        Debug.Log("[Stress] Penalty triggered!");
    }

    void EndPenalty()
    {
        _isInPenalty = false;
        sts          = 0f;

        _hurtEffect?.StopStressPenalty();                     // หยุดแสงม่วง

        if (stressBar != null)
            stressBar.UpdateStressBar(sts, maxSts);

        ApplySTStoDMGReduction();
        Debug.Log("[Stress] Penalty ended.");
    }

    #endregion
}