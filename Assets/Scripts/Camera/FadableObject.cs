using System.Collections.Generic;
using UnityEngine;

public class FadableObject : MonoBehaviour
{
    public float fadeAlpha = 0.7f;
    public float fadeSpeed = 10f;
    private List<Material> materials = new List<Material>();
    public float targetAlpha = 1f;

    void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
        {
            // per renderer
            foreach (var mat in r.materials)
            {
                materials.Add(mat);
            }
        }
    }

    void Start()
    {
        // Force alpha to 1 on start, overriding whatever the material has
        
        // Debug.Log($"[FadableObject] Found {materials.Count} materials");
        foreach (var mat in materials)
        {
            Color c = mat.GetColor("_BaseColor");
            // Debug.Log($"[FadableObject] Mat: {mat.name} | Alpha before: {c.a}");

            c.a = 1f;


            mat.SetColor("_BaseColor", c);
            // Debug.Log($"[FadableObject] Mat: {mat.name} | Alpha after: {c.a}");

        }
    }

    void Update()
    {
        foreach (var mat in materials)
        {
            Color c = mat.GetColor("_BaseColor");
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            mat.SetColor("_BaseColor", c);
        }
    }

    public void FadeIn()
    {
        targetAlpha = 1f;
        Debug.Log("Fading IN");
    }

    public void FadeOut()
    {
        targetAlpha = fadeAlpha;
        Debug.Log("fading out");
    }    
}
