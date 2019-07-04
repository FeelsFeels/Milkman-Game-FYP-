using System;
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

    public int[] playerScore = new int[4];

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

    private void Start()
    {
        GameManager GM = FindObjectOfType<GameManager>();
        if (GM)
        {
            GM.playerDeath += ChangeScore;
        }

        Array.Clear(playerScore, 0, playerScore.Length);
    }

    public void ChangeScore(PlayerController deadPlayer, PlayerController killerPlayer)
    {
        playerScore[killerPlayer.playerNumber - 1] += 1;
        killerPlayer.killCountTimer = GameManager.instance.killCountDownTimer;
        UpdateScore();
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        player1ScoreText.text = playerScore[0].ToString();
        player2ScoreText.text = playerScore[1].ToString();
        player3ScoreText.text = playerScore[2].ToString();
        //player4ScoreText.text = playerScore[3].ToString();
    }
}
