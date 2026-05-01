using UnityEngine;

public class QuestSign : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private GameObject visual;

    [Header("Float Effect")]
    public float floatAmplitude = 0.15f;
    public float floatSpeed = 1.2f;

    private Camera mainCamera;
    private Vector3 startPos;

    void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>();
        startPos = transform.position;

        if (visual != null)
            visual.SetActive(false);
    }

    void Update()
    {
        // Float — ทำงานตลอดไม่ว่า visual จะโชว์หรือไม่
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Billboard — ทำงานเฉพาะตอน visual โชว์
        if (visual != null && visual.activeSelf)
        {
            visual.transform.LookAt(mainCamera.transform);
            visual.transform.Rotate(0, 180, 0);
        }
    }

    public void Show()
    {
        if (visual != null) visual.SetActive(true);
    }

    public void Hide()
    {
        if (visual != null) visual.SetActive(false);
    }
}