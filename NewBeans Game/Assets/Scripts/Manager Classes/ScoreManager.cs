using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text player3ScoreText;
    public Text player4ScoreText;

    private List<PlayerController> playerList = new List<PlayerController>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //Sorts the players by their playerNumbers when game starts
        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
            playerList.Add(pc);
        playerList.Sort(delegate (PlayerController p1, PlayerController p2) { return p1.playerNumber.CompareTo(p2.playerNumber); });

    }

    public void ChangeScore(PlayerController deadPlayer, PlayerController killerPlayer)
    {
        killerPlayer.killCount += 1;
        killerPlayer.currentScore += GameManager.instance.killScoreToAdd;
        killerPlayer.killCountTimer = GameManager.instance.killCountDownTimer;
        UpdateScore();

    }

    public void ChangeScore(PlayerController deadPlayer, int scoreToChange)
    {
        deadPlayer.deathCount += 1;
        deadPlayer.currentScore += scoreToChange;
        UpdateScore();
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        player1ScoreText.text = playerList[0].currentScore.ToString();
        player2ScoreText.text = playerList[1].currentScore.ToString();
        player3ScoreText.text = playerList[2].currentScore.ToString();
        //player2ScoreText.text = playerScript[3].currentScore.ToString();
    }
}
