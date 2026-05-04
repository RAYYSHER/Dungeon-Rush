using System.Collections;
using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private HealthBar  shieldBar;

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

    private int     _shieldHP       = 0;
    private int     _maxShieldHP        = 0; 
    private bool    _isActive       = false;
    private Coroutine _durationCoroutine;

    // ─────────────────────────────────────────
    //  Public — called by ActiveSkillExecutor
    // ─────────────────────────────────────────

    public bool IsActive => _isActive;

    public void ActivateShield(int shieldHP, float duration)
    {
        if (_durationCoroutine != null)
        StopCoroutine(_durationCoroutine);

    _shieldHP       = shieldHP;
    _maxShieldHP    = shieldHP;         

    _isActive       = true;

    if (shieldVisual != null)
        shieldVisual.SetActive(true);

    if (shieldBar != null)
    {
        shieldBar.gameObject.SetActive(true);               
        shieldBar.UpdateHealthBar(_shieldHP, _maxShieldHP); 
    }

    _durationCoroutine = StartCoroutine(ExpireAfter(duration));
    }

    // Player.Hurt() เรียกตรงนี้ก่อนหัก HP จริง
    // คืนค่า damage ที่เหลือหลังหัก shield (0 = shield รับหมด)
    public int AbsorbDamage(int damage)
    {
        if (!_isActive) return damage;

        if (damage <= _shieldHP)
        {
            _shieldHP -= damage;

            if (shieldBar != null)
                shieldBar.UpdateHealthBar(_shieldHP, _maxShieldHP); 

            if (_shieldHP <= 0)
                DeactivateShield();

            return 0;
        }
        else
        {
            int remaining = damage - _shieldHP;
            DeactivateShield();
            return remaining;
        }
    }

    // ─────────────────────────────────────────
    //  Private
    // ─────────────────────────────────────────

    private void DeactivateShield()
    {
        _shieldHP   = 0;
        _isActive   = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        if (shieldBar != null)
            shieldBar.gameObject.SetActive(false);              

        if (_durationCoroutine != null)
        {
            StopCoroutine(_durationCoroutine);
            _durationCoroutine = null;
        }
    }

    private IEnumerator ExpireAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateShield();
        Debug.Log("[ShieldHandler] Shield expired");
    }
}