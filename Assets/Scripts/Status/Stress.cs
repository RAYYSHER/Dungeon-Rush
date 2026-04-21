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

    public Combat combat;


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
        IncreaseSTS(timeStressRate * Time.deltaTime);

        if (isUnderStress)
        {
            IncreaseSTS(timeStressRate * Time.deltaTime);
            isUnderStress = false;                              // reset each frame, Movement.Walk() sets it true
        }
        else
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
}
