using UnityEngine;

public class QuestSurviveZone : MonoBehaviour, IQuestTimer
{
    [Header("References")]
    [SerializeField] private RoomLock    roomLock;
    [SerializeField] private WorldTimer  worldTimer;
    

    [Header("Survive Settings")]
    public float surviveDuration = 60f;
    public float waveInterval    = 10f;

    private Timer surviveTimer;
    private float waveTimer;
    private bool  isActive = false;

    [Header("Wave Spawners")]
    [SerializeField] private ZombieSpawner     waveSpawner;
    [SerializeField] private EazyZombieSpawner easySpawner;
    [SerializeField] private TankZombieSpawner tankSpawner;
    [SerializeField] private GameObject triggerZoneObject;

    void Awake()
    {
        surviveTimer = new Timer(surviveDuration);
    }

    void Update()
    {
        if (!isActive) return;

        surviveTimer.Update(Time.deltaTime);

        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            waveTimer = waveInterval;

            Debug.Log("[QuestSurviveZone] SpawnWave called");

            waveSpawner?.SpawnWave();
            easySpawner?.SpawnWave();
            tankSpawner?.SpawnWave();
        }

        if (!surviveTimer.IsRunning())
            Complete();
    }

    public void StartSurvive()
    {
        isActive = true;
        roomLock?.Lock();
        worldTimer.SpawningOverridden = true;
        triggerZoneObject?.SetActive(false);  // ปิดแค่ trigger

        surviveTimer.Start();
        waveTimer = 0f;     // spawn wave แรกทันที

        Debug.Log("[QuestSurviveZone] Started");
    }

    private void Complete()
    {
        isActive = false;
        roomLock?.Unlock();
        worldTimer.SpawningOverridden = false;

        // reset cooldown ก่อน activate trigger คืน
        easySpawner?.spawnTimer.Start();
        tankSpawner?.spawnTimer.Start();
        waveSpawner?.spawnTimer.Start();

        QuestManager.Instance?.NotifySurviveComplete();
        triggerZoneObject?.SetActive(true);   // เปิดคืน

        Debug.Log("[QuestSurviveZone] Complete");
    }

    public float GetTimeRemaining()  => surviveTimer.GetTimeRemaining();
    public float GetTotalDuration()  => surviveDuration;
    public bool  IsTimerActive()     => isActive;   
}