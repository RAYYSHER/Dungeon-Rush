using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] QuestData[] questPool;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject interactPrompt; // "Press [button] to interact" object — optional

    [Header("Quest Sign (Billboard Sprite)")]
    [SerializeField] private GameObject questSign;

    [Header("Float Effect")]
    public float floatAmplitude = 0.15f;
    public float floatSpeed     = 1.2f;

    private Camera   mainCamera;
    private Vector3  signStartPos;

    bool playerInRange;
    bool isLocked;
    QuestData selectedQuest;

    void Start()
    {
        mainCamera    = FindFirstObjectByType<Camera>();

        selectedQuest = questPool[Random.Range(0, questPool.Length)];

        if (questSign != null)
        {
            questSign.SetActive(false);
            signStartPos = questSign.transform.position;
        }

        if (interactPrompt) 
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (questSign == null || !questSign.activeSelf) return;

        // Billboard
        questSign.transform.LookAt(mainCamera.transform);
        questSign.transform.Rotate(0, 180, 0);

        // Float
        float newY = signStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        questSign.transform.position = new Vector3(signStartPos.x, newY, signStartPos.z);
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
