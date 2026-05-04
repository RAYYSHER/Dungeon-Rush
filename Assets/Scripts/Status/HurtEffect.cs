using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    [Header("Hurt Effect")]
    public Color hurtColor  = new Color(1f, 0f, 0f, 1f);
    public int   blinkCount = 3;

    [Header("Stress Penalty Effect")]
    public Color penaltyColor = new Color(0.6f, 0f, 1f, 1f);   // สีม่วง — ปรับได้ใน Inspector

    private float hurtDuration;
    private Combat combat;

    private List<Material> materials      = new List<Material>();
    private List<Color>    originalColors = new List<Color>();

    private Coroutine hurtCoroutine;
    private Coroutine penaltyCoroutine;   // แยก coroutine ออกจาก hurt

    // ─────────────────────────────────────────

    void Awake()
    {
        combat       = GetComponent<Combat>();
        hurtDuration = combat.iFrameDuration;

        foreach (var r in GetComponentsInChildren<Renderer>())
            foreach (var mat in r.materials)
            {
                materials.Add(mat);
                originalColors.Add(mat.GetColor("_BaseColor"));
            }
    }

    // ─────────────────────────────────────────
    //  Hurt (red blink — ถูกโจมตี)
    // ─────────────────────────────────────────

    public void TriggerHurt()
    {
        if (hurtCoroutine != null)
            StopCoroutine(hurtCoroutine);

        hurtCoroutine = StartCoroutine(HurtBlink());
    }

    private IEnumerator HurtBlink()
    {
        float blinkInterval = hurtDuration / (blinkCount * 2);

        for (int i = 0; i < blinkCount; i++)
        {
            SetMaterialColors(hurtColor);
            yield return new WaitForSeconds(blinkInterval);
            RestoreOriginalColors();
            yield return new WaitForSeconds(blinkInterval);
        }

        RestoreOriginalColors();
        hurtCoroutine = null;
    }

    // ─────────────────────────────────────────
    //  Stress Penalty (purple blink — 1 ครั้ง/วินาที ตลอด penalty duration)
    // ─────────────────────────────────────────

    public void TriggerStressPenalty(float duration)
    {
        if (penaltyCoroutine != null)
            StopCoroutine(penaltyCoroutine);

        penaltyCoroutine = StartCoroutine(PenaltyBlink(duration));
    }

    public void StopStressPenalty()
    {
        if (penaltyCoroutine != null)
        {
            StopCoroutine(penaltyCoroutine);
            penaltyCoroutine = null;
        }

        RestoreOriginalColors();
    }

    private IEnumerator PenaltyBlink(float duration)
    {
        float elapsed      = 0f;
        float blinkInterval = 0.5f;   // on 0.5s, off 0.5s = 1 blink/sec

        while (elapsed < duration)
        {
            SetMaterialColors(penaltyColor);
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;

            RestoreOriginalColors();
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        RestoreOriginalColors();
        penaltyCoroutine = null;
    }

    // ─────────────────────────────────────────
    //  Helpers
    // ─────────────────────────────────────────

    private void SetMaterialColors(Color color)
    {
        foreach (var mat in materials)
            mat.SetColor("_BaseColor", color);
    }

    private void RestoreOriginalColors()
    {
        for (int i = 0; i < materials.Count; i++)
            materials[i].SetColor("_BaseColor", originalColors[i]);
    }
}