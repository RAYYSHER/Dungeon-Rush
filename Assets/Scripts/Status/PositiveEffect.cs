using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PositiveEffect : MonoBehaviour
{
    [Header("Level up Effect")]
    public Color levelUpColor = new Color(1f, 0f, 1f, 1f);
    public int levelUpBlinkCount = 5;

    [Header("Heal Effect")]
    public Color healColor = new Color(0f, 1f, 0f, 1f);
    public int healBlinkCount = 3;

    [Header("Shared Setting")]
    public float blinkDuration = 1f;
    public float blinkSpeed = 10f;

    private List<Material> materials = new List<Material>();
    private List<Color> originalColors = new List<Color>();
    private Coroutine effectCoroutine;

    void Awake()
    {
        
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

    public void TriggerLevelUp()
    {
        TriggerEffect(levelUpColor, levelUpBlinkCount);
    }

    public void TriggerHeal()
    {
        TriggerEffect(healColor, healBlinkCount);
    }

    private void TriggerEffect(Color color, int blinkCount)
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }

        effectCoroutine = StartCoroutine(Blink(color, blinkCount));
    }

    private IEnumerator Blink(Color color, int blinkCount)
    {
        float blinkInterval = blinkDuration / (blinkCount * 2);

        for (int i = 0; i < blinkCount; i++)
        {
            SetColors(color);
            yield return new WaitForSeconds(blinkInterval);

            RestoreColors();
            yield return new WaitForSeconds(blinkInterval);
        }

        RestoreColors();
        effectCoroutine = null;
    }

    private void SetColors(Color color)
    {
        
        foreach (var mat in materials)
        {
            mat.SetColor("_BaseColor", color);
        }
    }

    private void RestoreColors()
    {
        
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetColor("_BaseColor", originalColors[i]);
        }

    }
}
