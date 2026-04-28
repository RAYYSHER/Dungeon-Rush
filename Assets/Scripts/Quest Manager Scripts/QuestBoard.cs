using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] QuestData[] questPool;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject interactPrompt; // "Press [button] to interact" object — optional

    bool playerInRange;
    bool isLocked;
    QuestData selectedQuest;

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
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Entered");
        if (!other.CompareTag("Player")) 
            return;
        playerInRange = true;
        if (!isLocked && interactPrompt) 
            interactPrompt.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player Exit");
        if (!other.CompareTag("Player")) 
            return;
        playerInRange = false;
        if (interactPrompt) 
            interactPrompt.SetActive(false);
    }
}
