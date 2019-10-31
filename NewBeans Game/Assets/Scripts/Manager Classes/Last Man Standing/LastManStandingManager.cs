using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastManStandingManager : MonoBehaviour
{
    GameManager gameManager;

    public Text player1LivesText;
    public Text player2LivesText;
    public Text player3LivesText;
    public Text player4LivesText;

    public int startingLives;

    public int[] playerLives = new int[4];
    public bool[] playerLost = new bool[4];
    
    public List<PlayerController> alivePlayers = new List<PlayerController>();
    public List<PlayerController> playerRanking = new List<PlayerController>();
    public Dictionary<PlayerController, int> playerLivesInfo = new Dictionary<PlayerController, int>();


    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        //Sorts the players by their playerNumbers when game starts
        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
        {

            alivePlayers.Add(pc);
            playerLivesInfo.Add(pc, startingLives);

            UpdateScoreUI(pc);
        }
    }

    private void Start()
    {

    }

    public void ReduceLives(PlayerController deadPlayer, PlayerController killerPlayer)
    {

        int currentLives = --playerLivesInfo[deadPlayer];
        print(deadPlayer + " has " + currentLives + " lives left!");

        if (currentLives <= 0)
        {
            alivePlayers.Remove(deadPlayer);
            deadPlayer.shouldRespawn = false;   //Kills player, no more respawning
            deadPlayer.gameObject.SetActive(false); //player setactive false to stop camera tracking

            playerRanking.Insert(0, deadPlayer);
        }

        UpdateScoreUI(deadPlayer);
        CheckPlayersLeft();
    }


    public void UpdateScoreUI(PlayerController deadPlayer)
    {
        int deadPlayerNumber = deadPlayer.playerNumber;
        int deadPlayerLivesLeft = playerLivesInfo[deadPlayer];

        switch (deadPlayerNumber)
        {
            case 1:
                player1LivesText.text = deadPlayerLivesLeft.ToString();
                break;
            case 2:
                player2LivesText.text = deadPlayerLivesLeft.ToString();
                break;
            case 3:
                player3LivesText.text = deadPlayerLivesLeft.ToString();
                break;
            case 4:
                player4LivesText.text = deadPlayerLivesLeft.ToString();
                break;                
        }
    }

    void CheckPlayersLeft()
    {
        //One player is left.
        if (playerLivesInfo.Count - playerRanking.Count <= 1)
        {
            playerRanking.Insert(0, alivePlayers[0]);
            gameManager.RoundEnd(playerRanking);
        }
    }
}
