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

    void Awake()
    {
        timer = new Timer(timerDurationInMinutes * 60);
        lastMinuteRecorded = timerDurationInMinutes;
    }

    void Start()
    {
        timer.Start();

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
            foreach (var zombie in Zombie.zombieLists)
            {   
                zombie.SetStatToGlobal();
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
        Debug.Log("TeleportPlayerToBoss called");

        Player player = FindFirstObjectByType<Player>();
        Debug.Log($"Player found: {player != null}");
        Debug.Log($"SpawnPoint assigned: {bossRoomSpawnPoint != null}");

        if (player != null && bossRoomSpawnPoint != null)
        {
            var rb = player.GetComponent<Rigidbody>();
            
            Debug.Log($"Rigidbody found: {rb != null}");

            if (rb != null)
            {
                // rb.position = bossRoomSpawnPoint.position;

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
        }
    }
    #endregion

}
