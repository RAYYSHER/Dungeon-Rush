using TMPro;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    private Timer timer;
    public int timerDurationInMinutes = 5;
    public TMP_Text timerText;
    private int lastMinuteRecorded;
    private bool timerEnded = false;
    public Transform bossRoomSpawnPoint;
    public bool SpawningOverridden { get; set; } = false;

    [Header("Zombie Spawning")]
    
    public float spawnInterval = 30f;
    private float spawnTimer = 0f;

    [Header("Boss Room Lock")]
    [SerializeField] private RoomLock bossRoomLock; 

    void Awake()
    {
        timer = new Timer(timerDurationInMinutes * 60);
        lastMinuteRecorded = timerDurationInMinutes;
    }

    void Start()
    {
        timer.Start();
        spawnTimer = spawnInterval;

    }

    void Update()
    {
        
        timer.Update(Time.deltaTime);
        float remaining = timer.GetTimeRemaining(); //float -> 299.433517 etc

        //Teleport Player with timeout
        if (remaining <= 0f && !timerEnded)
        {
            timerEnded = true;
            TeleportPlayerToBoss();
        }

        

        //check is a full minute has passed
        int currentMinute = Mathf.CeilToInt(remaining / 60f);
        if (currentMinute < lastMinuteRecorded)
        {
            lastMinuteRecorded = currentMinute;

            //update status every minute passed
            ZombieGlobalStat.IncreaseStat();
            foreach (var spawner in Zombie.zombieSpawners)
            {   
                foreach (var zombie in spawner.zombieLists)
                {
                    zombie.SetStatToGlobal();
                }
            } 
            BossGlobalStat.IncreaseStat();
            foreach (var boss in FloorBoss.bossLists)
            {
                boss.SetStatToGlobal();
            }
        }
        

        string timeText = FormatTime(timer.GetTimeRemaining());
        timerText.text = timeText;
    }

    string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);

        return $"{minutes:00} : {secs:00}";
    }

    #region Teleport Player to Boss Room when timeout

    void TeleportPlayerToBoss()
    {
        Player player = FindFirstObjectByType<Player>();

        if (player != null && bossRoomSpawnPoint != null)
        {
            var rb = player.GetComponent<Rigidbody>();
            
            if (rb != null)
            {

                rb.isKinematic = true;
                player.transform.position = bossRoomSpawnPoint.position;
                rb.isKinematic = false;

                rb.linearVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

            }
            else
            {
                player.transform.position = bossRoomSpawnPoint.position;
            }

            if (bossRoomLock != null)
            {
                bossRoomLock.Lock();
            }

        }
    }
    #endregion

    public void AddTime(float seconds)
    {
        timer.AddTime(seconds);
        Debug.Log($"[WorldTimer] Extended +{seconds}s");
    }

}
