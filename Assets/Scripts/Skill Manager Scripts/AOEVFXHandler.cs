using System.Collections;
using UnityEngine;

public class AOEVFXHandler : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("AOE Flash")]
    [SerializeField] private GameObject aoeVisual;      // Sphere mesh ครอบ player
    [SerializeField] private float          fadeDuration = 0.5f;

    [Header("Radius Ring")]
    [SerializeField] private LineRenderer radiusRing;   // วงกลมขอบสีขาว
    [SerializeField] private int            ringSegments = 64;

    // ─────────────────────────────────────────
    //  Runtime State
    // ─────────────────────────────────────────

    private Material    _aoeMaterial;
    private Coroutine   _fadeCoroutine;

    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Awake()
    {
        if (aoeVisual != null)
        {
            _aoeMaterial = aoeVisual.GetComponent<Renderer>().material;
            aoeVisual.SetActive(false);
        }

        if (radiusRing != null)
            radiusRing.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────
    //  Public — called by ActiveSkillExecutor
    // ─────────────────────────────────────────

    // เรียกตอนกด AOE skill → flash แล้ว fade
    public void PlayAOEFlash(float radius)
    {
        if (aoeVisual == null) return;

        // scale ตาม radius
        aoeVisual.transform.localScale = Vector3.one * radius * 2f;
        aoeVisual.SetActive(true);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeOut());
    }

    // เรียกตอน skill ถูกเลือก → แสดง ring ตลอด
    public void ShowRadiusRing(float radius)
    {
        if (radiusRing == null) return;

        radiusRing.gameObject.SetActive(true);
        DrawRing(radius);
    }

    // เรียกตอน skill ถูกถอด → ซ่อน ring
    public void HideRadiusRing()
    {
        if (radiusRing == null) return;
        radiusRing.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────
    //  Private
    // ─────────────────────────────────────────

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        // เริ่มที่ alpha = 1
        SetAlpha(1f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        aoeVisual.SetActive(false);
        _fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = _aoeMaterial.GetColor("_BaseColor");
        c.a = alpha;
        _aoeMaterial.SetColor("_BaseColor", c);
    }

    private void DrawRing(float radius)
    {
        radiusRing.positionCount = ringSegments + 1;
        radiusRing.useWorldSpace = false;

        for (int i = 0; i <= ringSegments; i++)
        {
            float angle = i * 2f * Mathf.PI / ringSegments;
            float x     = Mathf.Cos(angle) * radius;
            float z     = Mathf.Sin(angle) * radius;
            radiusRing.SetPosition(i, new Vector3(x, 0.05f, z));
        }
    }
}