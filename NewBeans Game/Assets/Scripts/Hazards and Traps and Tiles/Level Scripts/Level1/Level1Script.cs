using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Script : MonoBehaviour
{
    public TilePatternCircular tilePatternCircular;
    public HazardsController hazardsController;
    public TimerManager timerManager;

    float timeLeft;
    float timePassed;
    float timeSinceLastHazard = 0;

    private void Awake()
    {
        tilePatternCircular = FindObjectOfType<TilePatternCircular>();
        hazardsController = FindObjectOfType<HazardsController>();
        timerManager = FindObjectOfType<TimerManager>();
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameStates.Score)
            CheckEvents();
        else if(GameManager.instance.gameState == GameManager.GameStates.LastManStanding)
            CheckEventsLastManStanding();
    }

    void CheckEvents()
    {

        timeLeft = timerManager.timeLeftInSeconds;
        timeSinceLastHazard += Time.deltaTime;

        if (timeLeft <= 30)
        {
            if (timeSinceLastHazard >= 6)
            {
                hazardsController.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if (timeLeft <= 60)
        {
            if (timeSinceLastHazard >= 12)
            {
                hazardsController.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if (timeLeft <= 120)
        {
            if (timeSinceLastHazard >= 15)
            {
                hazardsController.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }

        if (Mathf.Floor(timeLeft) == 130)
        {
            tilePatternCircular.Expand();
        }
        if (Mathf.Floor(timeLeft) == 80)
        {
            tilePatternCircular.Expand();
        }
    }

    void CheckEventsLastManStanding()
    {

        timePassed = timerManager.timeElapsedSinceStart;

        if (Mathf.Floor(timePassed) == 30)
        {
            tilePatternCircular.Expand();
        }

        if (Mathf.Floor(timePassed) == 70)
        {
            tilePatternCircular.Expand();
        }

        if (timePassed >= 70)
        {
            if (timeSinceLastHazard >= 9)
            {
                hazardsController.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
        else if (timePassed >= 30)
        {
            if (timeSinceLastHazard >= 15)
            {
                hazardsController.SpawnHazard();
                timeSinceLastHazard = 0;
            }
        }
    }
}
