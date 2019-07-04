using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level1Timing : MonoBehaviour
{

    private EventsManager eventsManager;
    private GameManager gameManager;
    private TimerManager timerManager;

    private float timeLeft;
    private float timePassed;
    private float timeSinceLastHazard = 0;

    public bool useTimeElapsed;

    private void Awake()
    {
        eventsManager = FindObjectOfType<EventsManager>();
        timerManager = FindObjectOfType<TimerManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!useTimeElapsed)
        {
            TimeLeftInSecondsEvents();
        }
        else
        {
            TimeElapsedEvents();
        }
    }

    void TimeLeftInSecondsEvents()
    {

        timeLeft = timerManager.timeLeftInSeconds;
        timeSinceLastHazard += Time.deltaTime;

        if (timeLeft <= 30)
        {
            if (timeSinceLastHazard >= 4)
            {
                eventsManager.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if (timeLeft <= 60)
        {
            if (timeSinceLastHazard >= 12)
            {
                eventsManager.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if (timeLeft <= 120)
        {
            if (timeSinceLastHazard >= 15)
            {
                eventsManager.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }

        if (Mathf.Floor(timeLeft) == 130)
        {
            eventsManager.NewPhase();
        }
        if (Mathf.Floor(timeLeft) == 80)
        {
            eventsManager.NewPhase();
        }
    }

    void TimeElapsedEvents()
    {
        timePassed = timerManager.timeElapsedSinceStart;

        if(Mathf.Floor(timePassed) == 30)
        {
            eventsManager.NewPhase();
        }
        
        if(Mathf.Floor(timePassed) == 70)
        {
            eventsManager.NewPhase();
        }

        if(timePassed >= 70)
        {
            if (timeSinceLastHazard >= 9)
            {
                eventsManager.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if(timePassed >= 30)
        {
            if (timeSinceLastHazard >= 15)
            {
                eventsManager.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
    }
}
