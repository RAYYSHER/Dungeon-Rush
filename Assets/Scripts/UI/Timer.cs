using Unity.VisualScripting;
using UnityEngine;

public class Timer
{
    private float duration;
    private float timeRemaining;
    private bool isRunning;

    public Timer(float duration)
    {
        this.duration = duration;
        timeRemaining = duration;
    }
    public void Start()
    {
        timeRemaining = duration;
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }
    
    public void Update(float deltatime)
    {
        if (isRunning == false)
        {
            return; //end
        }

        timeRemaining -= deltatime;
        if (timeRemaining <= 0f)
        {
            isRunning = false;
        }
    }
    public bool IsTimeUp()
    {
        return timeRemaining <= 0f;
    }

    public float GetTimeRemaining()
    {
        return Mathf.Max(timeRemaining, 0f);
    }

    public bool IsRunning()
    {
        return isRunning;
    }

}