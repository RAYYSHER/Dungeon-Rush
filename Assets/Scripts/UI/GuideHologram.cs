using UnityEngine;

public class GuideHologram : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private GameObject visual;      // child object ที่มี SpriteRenderer

    [Header("Float Effect")]
    public float floatAmplitude = 0.15f;
    public float floatSpeed = 1.2f;

    private Camera mainCamera;
    private Vector3 visualStartPos;

    void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>();

        if (visual != null)
        {
            visual.SetActive(false);
            visualStartPos = visual.transform.position;
        }
    }

    void Update()
    {
        if (visual == null || !visual.activeSelf) return;

        // Billboard — หันหน้าเข้ากล้องตลอด
        visual.transform.LookAt(mainCamera.transform);
        visual.transform.Rotate(0, 180, 0);

        // ลอยขึ้นลง
        float newY = visualStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        visual.transform.position = new Vector3(visualStartPos.x, newY, visualStartPos.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && visual != null)
            visual.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && visual != null)
            visual.SetActive(false);
    }
}