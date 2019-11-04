using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        Score,
        LastManStanding
    }

    public GameStates gameState;

    
    public static GameManager instance = null;

    private AudioSource theAudio;

    private List<PlayerController> playerScript = new List<PlayerController>();

    [Header("Managers")]
    public ScoreManager scoreManager;
    public LastManStandingManager LMSManager;
    public CommentaryManager commentaryManager;
    public BaseResultsScreen resultsScreen;

    [Header("Events")]
    private EventsManager eventsManager;
    public float timeSinceLastHazard;

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text player3ScoreText;
    public Text player4ScoreText;

    [Header("GUI Variables")]
    public int killScoreToAdd;
    public int deathScoreToDeduct;

    [Header("Round End Variables")]
    public Image roundEndScreen;
    public bool roundHasEnded;
    public Text firstPlaceScore;
    public Text secondPlaceScore;
    public Text thirdPlaceScore;
    public Text fourthPlaceScore;

    public PauseScreen pauseScreen;
    public bool isPaused;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip bgm;

    [Space]
    public float killCountDownTimer;
    public float deathCountDownTimer;
    


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
        //DontDestroyOnLoad(gameObject);

        //Find References
        eventsManager = FindObjectOfType<EventsManager>();
        resultsScreen = FindObjectOfType<BaseResultsScreen>();
        theAudio = GetComponent<AudioSource>();
        //Sort PlayerList
        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
        {
            //Check if player is playing
            if(pc.IsPlaying())
                playerScript.Add(pc);
            else
            {
                pc.gameObject.SetActive(false);
            }
        }
        playerScript.Sort(delegate (PlayerController p1, PlayerController p2) { return p1.playerNumber.CompareTo(p2.playerNumber); });

    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        // Pause & Resume Game.
        if (Input.GetButtonDown("Start (All Controllers)"))
        {
            // Pause game.
            if (isPaused == false)
            {
                PauseGame();
                isPaused = true;
                return;
            }
            // Resume game.
            else
            {
                ResumeGame();
                isPaused = false;
                return;
            }
        }

     
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Latest Version - Last Man Standing");
        }

    }

    public void PauseGame()
    {
        pauseScreen.AnimatePause();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.AnimateResume();
        isPaused = false;
        Time.timeScale = 1;
    }

    public void RoundEnd(List<PlayerController> playerRanking)
    {
        roundHasEnded = true;

        //Gamemode Score is broken af
        if (gameState == GameStates.Score)
        {
            playerScript.Sort(delegate (PlayerController p1, PlayerController p2) { return p1.currentScore.CompareTo(p2.currentScore); });
            playerScript.Reverse();

            roundEndScreen.gameObject.SetActive(true);

            print(playerScript[0].playerNumber + "PlayerNO" + playerScript[0].currentScore + "CurrentScore");
            print(playerScript[1].playerNumber + "PlayerNO" + playerScript[1].currentScore + "CurrentScore");
            print(playerScript[2].playerNumber + "PlayerNO" + playerScript[2].currentScore + "CurrentScore");

            firstPlaceScore.text = string.Format("Player {0}: {1}", playerScript[0].playerNumber, playerScript[0].currentScore);
            secondPlaceScore.text = string.Format("Player {0}: {1}", playerScript[1].playerNumber, playerScript[1].currentScore);
            thirdPlaceScore.text = string.Format("Player {0}: {1}", playerScript[2].playerNumber, playerScript[2].currentScore);
            //fourthPlaceScore.text = string.Format("Player {0}: {1}", playerScript[3].playerNumber, playerScript[3].currentScore);

            Time.timeScale = 0;
        }
        else if (gameState == GameStates.LastManStanding)
        {
            roundEndScreen.gameObject.SetActive(true);

            PlayerController playerReference = LMSManager.playerRanking[0];
            firstPlaceScore.text = string.Format("First Place: Player {0}", playerReference.playerNumber);
            playerReference = LMSManager.playerRanking[1];
            secondPlaceScore.text = string.Format("Second Place: Player {0}", playerReference.playerNumber);

            if (playerRanking.Count <= 2)
                return;

            playerReference = LMSManager.playerRanking[2];
            thirdPlaceScore.text = string.Format("Third Place: Player {0}", playerReference.playerNumber);

            if (playerRanking.Count <= 3)
                return;

            playerReference = LMSManager.playerRanking[3];
            fourthPlaceScore.text = string.Format("Fourth Place: Player {0}", playerReference.playerNumber);
        }

    }

    //Called from LastManStandingTracker.cs
    public void EndGame(List<PlayerController> ranking)
    {
        roundHasEnded = true;
        resultsScreen.DisplayScreen(ranking);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    
    public void OnPlayerDeath(PlayerController playerDead, PlayerController killer)
    {
        if (gameState == GameStates.Score)
        {
            if (scoreManager == null)
                return;
            scoreManager.ChangeScore(playerDead, killer);
            commentaryManager.CheckPlayerKill(playerDead, killer);
        }
        else if (gameState == GameStates.LastManStanding)
        {
            if (LMSManager == null)
                return;
            LMSManager.ReduceLives(playerDead, killer);
            commentaryManager.CheckPlayerKill(playerDead, killer);
        }
    }

}