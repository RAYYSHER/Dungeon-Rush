using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    [Header("Hurt Effect Setting")]
    public Color hurtColor = new Color(1f, 0f, 0f, 1f);             // Red Tint
    public float blinkSpeed = 10f;                                  // How fast it blinks
    private float hurtDuration;                                     // Total hurt effect duration
    public int   blinkCount = 3;     
    
    [Header("Stress Penalty Effect")]
    public Color stressPenaltyColor = new Color(0.6f, 0f, 1f, 1f);  // Purple

    private Combat combat;                                          // Blink amounts

    private List<Material> materials = new List<Material>();
    private List<Color> originalColors = new List<Color>();

    private Coroutine hurtCoroutine;
    private Coroutine penaltyCoroutine;
    

    void Awake()
    {

        combat = GetComponent<Combat>();
        hurtDuration = combat.iFrameDuration;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
        {
            // per renderer
            foreach (var mat in r.materials)
            {
                materials.Add(mat);
                originalColors.Add(mat.GetColor("_BaseColor"));
            }
        }
    }

    public void TriggerHurt()
    {
        if(hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine = StartCoroutine(HurtBlink());
    }

    // ---- Stress Penalty (Purple) ----
 
    /// <summary>
    /// กะพริบสีม่วง 1 ครั้งต่อวินาที ตลอด duration ของ Penalty
    /// เรียกจาก Stress.TriggerPenalty()
    /// </summary>
    public void TriggerStressPenalty(float duration)
    {
        if (penaltyCoroutine != null)
        {
            StopCoroutine(penaltyCoroutine);
        }
        penaltyCoroutine = StartCoroutine(StressPenaltyBlink(duration));
    }

    private IEnumerator StressPenaltyBlink(float duration)
    {
        float elapsed = 0f;
        float halfSecond = 0.5f; // 1 blink/sec = 0.5s on + 0.5s off
 
        while (elapsed < duration)
        {
            // หยุด hurt blink ชั่วคราวเพื่อไม่ให้สีชนกัน
            SetMaterialColors(stressPenaltyColor);
            yield return new WaitForSeconds(halfSecond);
 
            RestoreOriginalColors();
            yield return new WaitForSeconds(halfSecond);
 
            elapsed += 1f;
        }
 
        RestoreOriginalColors();
        penaltyCoroutine = null;
    }

    private IEnumerator HurtBlink()
    {
        float blinkInterval = hurtDuration / (blinkCount *2);

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


    private void SetMaterialColors(Color color)
    {
        foreach (var mat in materials)
        {
            mat.SetColor("_BaseColor", color);
        }
    }

   
    private void RestoreOriginalColors()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetColor("_BaseColor", originalColors[i]);
        }
    }    
}

