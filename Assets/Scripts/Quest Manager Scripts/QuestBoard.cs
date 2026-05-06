using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] QuestData[] questPool;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject interactPrompt; // "Press [button] to interact" object — optional

    [Header("Quest Sign")]
    [SerializeField] private QuestSign questSign;

    bool playerInRange;
    bool isLocked;
    QuestData selectedQuest;

    [SerializeField] QuestSurviveZone surviveZone;   // optional — ลากใส่ถ้าเป็น survive quest
    [SerializeField] QuestZoneMarker zoneMarker;

    void Start()
    {
        selectedQuest = questPool[Random.Range(0, questPool.Length)]; 

        if (interactPrompt) 
            interactPrompt.SetActive(false);
    }

    void OnEnable()
    {
        interactAction.action.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Quest Entered");
        if (!playerInRange || isLocked) 
            return;
   
        QuestManager.Instance?.ShowQuestOffer(selectedQuest, this);
        Debug.Log("Quest Selected");
    }


    // Called by QuestManager when quest is accepted.
    public void Lock()
    {
        isLocked = true;
        if (interactPrompt)
            interactPrompt.SetActive(false);

        switch (selectedQuest.questType)
        {
            case QuestType.Survive:
                surviveZone?.StartSurvive();
                break;
            case QuestType.StayInZone:
                zoneMarker?.StartZone();
                break;
            case QuestType.KillCount:
                // ไม่ต้องทำอะไรเพิ่ม
                break;
        }
    }

public IQuestTimer GetQuestTimer()
{
    switch (selectedQuest.questType)
    {
        case QuestType.Survive:    return surviveZone;
        case QuestType.StayInZone: return zoneMarker;
        default:                   return null;
    }
}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Entered");
        if (!other.CompareTag("Player")) 
            return;
        playerInRange = true;
        if (!isLocked)
        {
            questSign?.Show();
            if (interactPrompt) 
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player Exit");
        if (!other.CompareTag("Player")) 
            return;
        playerInRange = false;
        questSign?.Hide();
        if (interactPrompt) 
            interactPrompt.SetActive(false);
    }
}
