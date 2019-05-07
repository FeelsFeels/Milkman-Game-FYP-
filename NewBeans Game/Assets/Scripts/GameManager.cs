using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    private PlayerController[] playerScript;

    [Header("Player 1 UI")]
    public Text player1ScoreText;

    [Header("Player 2 UI")]
    public Text player2ScoreText;

    [Header("Check For Player Deaths")]
    public static bool onePlayerIsKilled;
    public bool playerOneDied;
    public bool playerTwoDied;

    [Header("GUI Variables")]
    public int killScoreToAdd;
    public int deathScoreToDeduct;

    [Header("Round End Variables")]
    public Image roundEndScreen;
    public bool roundHasEnded;
    public Text player1WinText;
    public Text player2WinText;
    public Text roundEndWithDraw;

    [Header("Timer")]
    public float timeLeftInSeconds;
    public Text timerText;

    public Image pauseScreen;

    // Awake is always called before any Start functions
    void Awake()
    {
        // Check if instance already exists;
        if (instance == null)
        {
            // If not, set instance to this.
            instance = this;
        }

        // If instance already exists and is not this;
        else if (instance != this)
        {
            // Then destroy this. (There can only be one instance of a GameManager).
            Destroy(gameObject);
        }

        // Don't destroy this object otherwise.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        player1WinText.gameObject.SetActive(false);
        player2WinText.gameObject.SetActive(false);
        roundEndWithDraw.gameObject.SetActive(false);


        StartTimerCount();

        playerScript = FindObjectsOfType<PlayerController>();



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PauseGame();
        }

        if (onePlayerIsKilled == true)
        {

            playerOneDied = false;
            playerTwoDied = false;
            onePlayerIsKilled = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Demo Game (Ortho Cam)");
            StartTimerCount();

        }
    }

    // Starts the count down of round time.
    public void StartTimerCount()
    {
        timerText.text = ("Time Left: :00:000");
        InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);

    }

    // Updates the timer every millisecond.
    public void UpdateTimer()
    {
        string minutes, seconds, fraction;

        if (timeLeftInSeconds > 0)
        {
            timeLeftInSeconds -= Time.deltaTime;
            minutes = Mathf.Floor(timeLeftInSeconds / 60).ToString("00");
            seconds = (timeLeftInSeconds % 60).ToString("00");
            fraction = ((timeLeftInSeconds * 100) % 100).ToString("000");
            timerText.text = "Time Left: " + minutes + ":" + seconds + ":" + fraction;
        }
        else
        {
            roundHasEnded = true;

            minutes = "00";
            seconds = "00";
            fraction = "000";
            timerText.text = "Time Left: " + minutes + ":" + seconds + ":" + fraction;

            RoundEnd();
        }
    }

    public void PauseGame()
    {
        pauseScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.gameObject.SetActive(false);
    }

    public void RoundEnd()
    {
        if (roundHasEnded == true)
        {
            Time.timeScale = 0;
            roundEndScreen.gameObject.SetActive(true);
        }
    }
}