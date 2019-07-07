using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text player3ScoreText;
    public Text player4ScoreText;

    public List<PlayerController> playerList = new List<PlayerController>();

    // Start is called before the first frame update
    void Awake()
    {
        //Sorts the players by their playerNumbers when game starts
        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
            playerList.Add(pc);
        playerList.Sort(delegate (PlayerController p1, PlayerController p2) { return p1.playerNumber.CompareTo(p2.playerNumber); });

    }

    public void ChangeScore(PlayerController deadPlayer, PlayerController killerPlayer)
    {
        if (killerPlayer != null)
        {
            killerPlayer.currentScore += GameManager.instance.killScoreToAdd;
            killerPlayer.killCountTimer = GameManager.instance.killCountDownTimer;
        }
        UpdateScore();
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        player1ScoreText.text = playerList[0].currentScore.ToString();
        player2ScoreText.text = playerList[1].currentScore.ToString();
        player3ScoreText.text = playerList[2].currentScore.ToString();
        //player4ScoreText.text = playerScore[3].ToString();
    }
}
