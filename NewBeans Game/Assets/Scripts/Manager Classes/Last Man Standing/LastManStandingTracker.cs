using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LastManStandingTracker : MonoBehaviour
{
    public GameManager gameManager;
    DeathInfluenceManager deathInfluenceManager;

    public List<PlayerController> alivePlayers = new List<PlayerController>();
    public List<PlayerController> playerRanking = new List<PlayerController>();
    public LastManStandingUI[] lastManStandingUIs;

    public Dictionary<PlayerController, int> playerLivesInfo = new Dictionary<PlayerController, int>();

    public int startingLives;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        deathInfluenceManager = FindObjectOfType<DeathInfluenceManager>();
        lastManStandingUIs = FindObjectsOfType<LastManStandingUI>();

        //foreach(LastManStandingUI ui in lastManStandingUIs)
        //{
        //    ui.livesText.text = startingLives.ToString();
        //}

        PlayerController[] tempPCList = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pc in tempPCList)
        {
            //Check if player is playing
            if (pc.IsPlaying())
            {
                alivePlayers.Add(pc);
                pc.OnPlayerDeath.AddListener(UpdateLivesLeft);
                playerLivesInfo.Add(pc, startingLives);

                //Skills
                pc.gameObject.GetComponent<SkillSetManager>().OnChargeUltimate.AddListener(UpdateUltiCharge);

            }
            else
                continue;
        }
    }


    public void UpdateLivesLeft(PlayerController deadPlayer, PlayerController killer)
    { 
        int currentLives = --playerLivesInfo[deadPlayer];
        print(deadPlayer + " has " + currentLives + " lives left!");

        if (killer)
        { 
            killer.killCount++;
        }

        if (currentLives <= 0)
        {
            alivePlayers.Remove(deadPlayer);
            deadPlayer.shouldRespawn = false;   //Kills player, no more respawning
            deadPlayer.gameObject.SetActive(false); //player setactive false to stop camera tracking

            playerRanking.Insert(0, deadPlayer);

            if(deathInfluenceManager)
                deathInfluenceManager.AddNewDeadPlayer(deadPlayer);
        }

        
        UpdateLivesUI(deadPlayer.inputInfo, playerLivesInfo[deadPlayer]);
        CheckLastManStanding();
    }


    /// *********************************
    /// Update Ulti Charge
    /// *********************************
    public void UpdateUltiCharge(SkillSetManager manager)
    {
        float percent = manager.chargePercentage;
        PlayerController thePlayer = manager.GetComponent<PlayerController>();
        PlayerInputInfo playerInfo = thePlayer.inputInfo;

        foreach (LastManStandingUI playerUI in lastManStandingUIs)
        {
            playerUI.UpdateUltiUI(playerInfo, percent);
        }
    }




    public void UpdateLivesUI(PlayerInputInfo playerToUpdate, int livesLeft)
    {
        foreach(LastManStandingUI ui in lastManStandingUIs)
        {
            ui.UpdateUI(playerToUpdate, livesLeft);
        }
    }

    void CheckLastManStanding()
    {
        //One player is left.
        if(playerLivesInfo.Count - playerRanking.Count <= 1)
        {
            playerRanking.Insert(0, alivePlayers[0]);
            gameManager.EndGame(playerRanking);
        }
    }

}
