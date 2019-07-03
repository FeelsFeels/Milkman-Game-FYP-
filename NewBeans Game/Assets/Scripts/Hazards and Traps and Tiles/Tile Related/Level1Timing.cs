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
    private float timeSinceLastHazard = 0;

    private void Awake()
    {
        eventsManager = FindObjectOfType<EventsManager>();
        timerManager = FindObjectOfType<TimerManager>();
    }


    // Update is called once per frame
    void Update()
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
}
