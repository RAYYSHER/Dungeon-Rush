using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    [Header("Hurt Effect Setting")]
    public Color hurtColor = new Color(1f, 0f, 0f, 1f);         //Red Tint
    public float blinkSpeed = 10f;                              // How fast it blinks
    private float hurtDuration;                           // Total hurt effect duration
    public int   blinkCount = 3;     
    
    private Combat combat;                           // How many blinks

    private List<Material> materials = new List<Material>();
    private List<Color> originalColors = new List<Color>();
    private Coroutine hurtCoroutine;

    

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

