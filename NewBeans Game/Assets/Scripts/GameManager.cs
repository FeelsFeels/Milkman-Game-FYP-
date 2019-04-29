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
    public int killCount1;
    public int deathCount1;
    public int player1CurrentScore;
    public Text player1ScoreText;

    [Header("Player 2 UI")]
    public int killCount2;
    public int deathCount2;
    public int player2CurrentScore;
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

        player1CurrentScore = 0;
        player2CurrentScore = 0;


    }

    // Update is called once per frame
    void Update()
    {

        if (onePlayerIsKilled == true)
        {
            CheckWhichPlayerDied();

            playerOneDied = false;
            playerTwoDied = false;
            onePlayerIsKilled = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Michelle_LevelDesign But testing");
        }
    }

    // Checks which player died, to add score.
    public void CheckWhichPlayerDied()
    {

        foreach (PlayerController player in playerScript)
        {
            if (player.playerNumber == 1 && player.isDead)
            {
                playerOneDied = true;
                player2CurrentScore += killScoreToAdd;
                instance.player2ScoreText.text = player2CurrentScore.ToString();
                return;
            }

            if (player.playerNumber == 2 && player.isDead)
            {
                playerTwoDied = true;
                player1CurrentScore += killScoreToAdd;
                instance.player1ScoreText.text = player1CurrentScore.ToString();
                return;
            }
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

    public void RoundEnd()
    {
        if (roundHasEnded == true)
        {
            Time.timeScale = 0;
            roundEndScreen.gameObject.SetActive(true);

            if (player1CurrentScore > player2CurrentScore)
            {
                player1WinText.text = ("Player 1 has won with a kill count of " + player1CurrentScore + ".");
                player1WinText.gameObject.SetActive(true);
            }

            else if (player2CurrentScore > player1CurrentScore)
            {
                player2WinText.text = ("Player 2 has won with a kill count of " + player2CurrentScore + ".");
                player2WinText.gameObject.SetActive(true);
            }

            else
            {
                roundEndWithDraw.gameObject.SetActive(true);
            }
        }
    }
}