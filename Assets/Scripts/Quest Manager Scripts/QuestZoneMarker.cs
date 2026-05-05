using UnityEngine;

public class QuestZoneMarker : MonoBehaviour, IQuestTimer
{
    [Header("Settings")]
    public float requiredDuration = 30f;    // อยู่ครบกี่วินาทีถึงสำเร็จ

    [Header("Visual (Optional)")]
    [SerializeField] private GameObject zoneVisual;   // วงกลมบนพื้น หรือ particle

    private float elapsed      =    0f;
    private bool  playerInside = false;
    private bool  isActive     = false;
    private bool  isComplete   = false;

    void Start()
    {
        zoneVisual?.SetActive(false);
    }

    void Update()
    {
        if (!isActive || isComplete || !playerInside) return;

        elapsed += Time.deltaTime;

        if (elapsed >= requiredDuration)
            Complete();
    }

    public void StartZone()
    {
        isActive  = true;
        elapsed   = 0f;
        isComplete = false;

        zoneVisual?.SetActive(true);
        Debug.Log("[QuestZoneMarker] Started");
    }

    private void Complete()
    {
        isComplete = true;
        zoneVisual?.SetActive(false);

        QuestManager.Instance?.NotifyZoneComplete();
        Debug.Log("[QuestZoneMarker] Complete");
    }

    public float GetTimeRemaining()  => Mathf.Max(requiredDuration - elapsed, 0f);
    public float GetTotalDuration()  => requiredDuration;
    public bool  IsTimerActive()     => isActive && !isComplete;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("[QuestZoneMarker] Player entered — timer resumed");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("[QuestZoneMarker] Player left — timer paused");
        }
    }
}