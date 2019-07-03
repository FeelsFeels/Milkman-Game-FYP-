using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    [Header("Timer")]
    public float startingTime;
    public float timeLeftInSeconds;
    public float timeElapsedSinceStart;
    public Text timerText;  //Leave null if not having a counting text


    public void Start()
    {
        StartTimerCount();
    }

    // Starts the count down of round time.
    public void StartTimerCount()
    {
        timeLeftInSeconds = startingTime;
        timerText.text = ("Time Left: 0:00");
        InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);

    }

    // Updates the timer every millisecond.
    public void UpdateTimer()
    {
        string minutes, seconds;
        if (GameManager.instance.roundHasEnded != true)
        {
            timeElapsedSinceStart += Time.deltaTime;

            if (timerText != null)
            {
                if (timeLeftInSeconds > 0)
                {
                    timeLeftInSeconds -= Time.deltaTime;
                    minutes = Mathf.Floor(timeLeftInSeconds / 60).ToString("0");
                    seconds = (timeLeftInSeconds % 60).ToString("00");
                    timerText.text = "Time Left: " + minutes + ":" + seconds;
                }
                else
                {
                    GameManager.instance.roundHasEnded = true;

                    minutes = "00";
                    seconds = "00";
                    timerText.text = "Time Left: " + minutes + ":" + seconds;

                    GameManager.instance.RoundEnd();
                }
            }
        }
    }


}
