using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastManStandingManager : MonoBehaviour
{
    public Text player1LivesText;
    public Text player2LivesText;
    public Text player3LivesText;
    public Text player4LivesText;

    public int startingLives;

    public int[] playerLives = new int[4];
    public bool[] playerLost = new bool[4];

    private List<PlayerController> playerList = new List<PlayerController>();
    public Stack<PlayerController> playerRankOrder = new Stack<PlayerController>();
    

    void Awake()
    {
        //Sorts the players by their playerNumbers when game starts
        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
            playerList.Add(pc);
        playerList.Sort(delegate (PlayerController p1, PlayerController p2) { return p1.playerNumber.CompareTo(p2.playerNumber); });
    }

    private void Start()
    {
        GameManager GM = FindObjectOfType<GameManager>();

        //Set initial values
        for(int i = 0; i < playerLives.Length; i++)
        {
            //Checking if player exists
            if (i >= playerList.Count)
            {
                playerLives[i] = 0;
                playerLost[i] = true;
                continue;
            }

            playerLives[i] = startingLives;
            playerLost[i] = false;

        }
        UpdateScore();
    }

    public void ReduceLives(PlayerController deadPlayer, PlayerController killerPlayer)
    {
        playerLives[deadPlayer.playerNumber - 1] -= 1;

        if (killerPlayer != null)
        {
            killerPlayer.killCount++;
        }

        //Player has no more lives
        if(playerLives[deadPlayer.playerNumber - 1] <= 0)
        {
            playerLost[deadPlayer.playerNumber - 1] = true;
            deadPlayer.shouldRespawn = false;
            playerRankOrder.Push(deadPlayer);

            //TEMPORARY FIX, player setactive false to stop camera tracking
            deadPlayer.gameObject.SetActive(false);
        }

        UpdateScore();
    }

    public void UpdateScore()
    {
        player1LivesText.text = playerLives[0].ToString();
        player2LivesText.text = playerLives[1].ToString();
        player3LivesText.text = playerLives[2].ToString();
        player4LivesText.text = playerLives[3].ToString();

        int numberPlayersLeft = 0;

        for (int i = 0; i < playerLost.Length; i++)
        {
            if(playerLost[i] == false)
            {
                numberPlayersLeft++;
            }
        }
        if(numberPlayersLeft == 1)
        {
            print(GetWinnerPlayerReference().name);
            playerRankOrder.Push(GetWinnerPlayerReference());
            GameManager.instance.roundHasEnded = true;
            GameManager.instance.RoundEnd();
        }
    }

    PlayerController GetWinnerPlayerReference()
    {
        for (int i = 0; i < playerLost.Length; i++)
        {
            if (playerLost[i] == true)
                continue;
            else
                return playerList[i];
        }
        return null;
    }
}
