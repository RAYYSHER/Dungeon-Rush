using TMPro;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    private Timer timer;
    private int timerDurationInMinutes = 5;
    public TMP_Text timerText;
    private int lastMinuteRecorded;

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

        //check is a full minute has passed
        int currentMinute = Mathf.CeilToInt(remaining / 60f);
        if (currentMinute < lastMinuteRecorded)
        {
            lastMinuteRecorded = currentMinute;

            //update every minute passed
            ZombieGlobalStat.IncreaseStat();
            foreach (var zombie in Zombie.zombieLists)
            {   
                zombie.SetStatToGlobal();
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
}
