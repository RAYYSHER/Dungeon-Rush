using System.Collections;
using UnityEngine;

public class ActiveSkillExecutor : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  References
    // ─────────────────────────────────────────

    private Stress  stressSystem;
    private Combat  combat;
    private Health  health;
    private AOEVFXHandler _aoeVFX;

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

    private Timer[] _cooldownTimers = new Timer[2];
    private Timer[] _durationTimers  = new Timer[2];
    private bool    _dmgBuffActive  = false;

    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Awake()
    {
        stressSystem      = GetComponent<Stress>();
        combat            = GetComponent<Combat>();
        health            = GetComponent<Health>();
        _aoeVFX           = GetComponent<AOEVFXHandler>();

        _cooldownTimers[0] = new Timer(0f);
        _cooldownTimers[1] = new Timer(0f);
        _durationTimers[0] = new Timer(0f);
        _durationTimers[1] = new Timer(0f);
    }

    void OnEnable()
    {
        SkillManager.Instance.OnSkillsChanged += RefreshTimers;
    }

    void OnDisable()
    {
        SkillManager.Instance.OnSkillsChanged -= RefreshTimers;
    }

    void Update()
    {
        _cooldownTimers[0].Update(Time.deltaTime);
        _cooldownTimers[1].Update(Time.deltaTime);
        _durationTimers[0].Update(Time.deltaTime);
        _durationTimers[1].Update(Time.deltaTime);
    }

    // ─────────────────────────────────────────
    //  Public — called by PlayerController
    // ─────────────────────────────────────────

    public void UseSkill(int slotIndex)
    {
         SkillInstance skill = SkillManager.Instance.GetSlot(slotIndex);

        if (skill == null)                          return;
        if (skill.data.skillType != SkillType.Active) return;
        if (_cooldownTimers[slotIndex].IsRunning())  return;
        if (stressSystem.IsInPenalty)               return;   // penalty — active skill ใช้ไม่ได้

        // เพิ่ม STS cost — ถ้าเกินหลอด Stress.IncreaseSTS จะ Clamp ไว้ที่ maxSts
        // แล้ว Stress.Update() จะ trigger penalty เองในเฟรมถัดไป
        stressSystem.IncreaseSTS(skill.data.stsCost);

        ExecuteEffect(skill);

        _cooldownTimers[slotIndex] = new Timer(skill.data.cooldown);
        _cooldownTimers[slotIndex].Start();

        // start duration timer ถ้า skill มี duration  ← เพิ่มทั้งบล็อกนี้
        if (skill.data.duration > 0f)
        {
            _durationTimers[slotIndex] = new Timer(skill.data.duration);
            _durationTimers[slotIndex].Start();
        }
    }

    // UI can call this to show remaining cooldown
    public float GetCooldownRemaining(int slotIndex)
    {
        return _cooldownTimers[slotIndex].GetTimeRemaining();
    }

    public bool IsOnCooldown(int slotIndex)
    {
        return _cooldownTimers[slotIndex].IsRunning();
    }

    // ─────────────────────────────────────────
    //  Effect Execution
    // ─────────────────────────────────────────

    private void ExecuteEffect(SkillInstance skill)
    {
        SkillLevelData levelData = skill.GetCurrentLevelData();

        switch (skill.data.effectType)
        {
            case SkillEffectType.Heal:
                ExecuteHeal(levelData);
                break;

            case SkillEffectType.STSReduce:
                ExecuteSTSReduce(levelData);
                break;

            case SkillEffectType.Shield:
                ExecuteShield(levelData, skill.data.duration);
                break;

            case SkillEffectType.AOEDamage:
                ExecuteAOEDamage(levelData, skill.data.area);
                break;

            case SkillEffectType.DMGBuff:
                ExecuteDMGBuff(levelData, skill.data.duration);
                break;
        }
    }

    // ─────────────────────────────────────────
    //  Individual Effects
    // ─────────────────────────────────────────

    private void ExecuteHeal(SkillLevelData levelData)
    {
        int healAmount = Mathf.RoundToInt(health.maxHealth * levelData.primaryValue);
        health.IncreaseHealth(healAmount);

        Debug.Log($"[ActiveSkillExecutor] Heal → {healAmount} HP ({levelData.primaryValue * 100f}% of max)");
    }

    private void ExecuteSTSReduce(SkillLevelData levelData)
    {
        stressSystem.DecreaseSTSPercent(levelData.primaryValue);

        Debug.Log($"[ActiveSkillExecutor] STS Reduce → {levelData.primaryValue * 100f}%");
    }

    private void ExecuteShield(SkillLevelData levelData, float duration)
    {
        // Shield ต้องการ component แยกต่างหาก — เตรียม hook ไว้ก่อน
        ShieldHandler shieldHandler = GetComponent<ShieldHandler>();
        if (shieldHandler == null)
        {
            Debug.LogWarning("[ActiveSkillExecutor] ShieldHandler not found on Player.");
            return;
        }

        int shieldHP = Mathf.RoundToInt(health.maxHealth * levelData.primaryValue);
        shieldHandler.ActivateShield(shieldHP, duration);

        Debug.Log($"[ActiveSkillExecutor] Shield → {shieldHP} HP | {duration}s");

    }

    private void ExecuteAOEDamage(SkillLevelData levelData, float baseArea)
    {
        float   radius  = levelData.primaryValue;
        int     damage  = Mathf.RoundToInt(combat.attackDamage * 1.75f);

        _aoeVFX?.PlayAOEFlash(radius);     // ← เพิ่ม

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target == null)             continue;
            if (hit.gameObject == gameObject) continue;     // ไม่โจมตีตัวเอง

            target.Hurt(damage);
        }

        Debug.Log($"[ActiveSkillExecutor] AOE Damage → {damage} dmg | radius: {radius}m");
    }

    private void ExecuteDMGBuff(SkillLevelData levelData, float duration)
    {
        if (_dmgBuffActive) return;
        StartCoroutine(DMGBuffCoroutine(levelData.primaryValue, duration));
    }

    private IEnumerator DMGBuffCoroutine(float multiplier, float duration)
    {
        _dmgBuffActive = true;

        int originalDamage  = combat.attackDamage;
        combat.attackDamage = Mathf.RoundToInt(originalDamage * multiplier);

        Debug.Log($"[ActiveSkillExecutor] DMG Buff START → {originalDamage} × {multiplier} = {combat.attackDamage}");

        yield return new WaitForSeconds(duration);

        combat.attackDamage = originalDamage;
        _dmgBuffActive      = false;

        Debug.Log($"[ActiveSkillExecutor] DMG Buff END → restored to {originalDamage}");
    }

    // ─────────────────────────────────────────
    //  Private Helpers
    // ─────────────────────────────────────────

    public bool WillTriggerPenalty(float stsCost)
    {
        return stressSystem.sts + stsCost >= stressSystem.maxSts;
    }

    // สร้าง Timer ใหม่ตาม cooldown ของ skill ปัจจุบัน
    private void RefreshTimers()
    {
        for (int i = 0; i < 2; i++)
        {
            SkillInstance skill = SkillManager.Instance.GetSlot(i);
            if (skill == null) continue;

            // สร้าง Timer ใหม่เฉพาะเมื่อยังไม่เคย assign หรือ cooldown เปลี่ยน
            bool timerNotSet = _cooldownTimers[i] == null;
            if (timerNotSet)
                _cooldownTimers[i] = new Timer(skill.data.cooldown);
        }
    }

    // UI เรียกเพื่อรู้ว่า duration กำลัง active อยู่มั้ย
    public bool IsDurationActive(int slotIndex)
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(slotIndex);
        if (skill == null) return false;

        // Shield — ดูจาก ShieldHandler โดยตรง
        if (skill.data.effectType == SkillEffectType.Shield)
        {
            ShieldHandler shield = GetComponent<ShieldHandler>();
            return shield != null && shield.IsActive;
        }

        // DMG Buff และ skill อื่นที่มี duration → ดูจาก timer
        return _durationTimers[slotIndex].IsRunning();
    }

    // UI เรียกเพื่อรู้เวลาที่เหลือของ duration
    public float GetDurationRemaining(int slotIndex)
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(slotIndex);
        if (skill == null) return 0f;

        return _durationTimers[slotIndex].GetTimeRemaining();
    }

    // UI เรียกเพื่อรู้ duration ทั้งหมด (สำหรับคำนวณ fill amount)
    public float GetDurationTotal(int slotIndex)
    {
        SkillInstance skill = SkillManager.Instance.GetSlot(slotIndex);
        if (skill == null) return 1f;

        return skill.data.duration;
    }
}