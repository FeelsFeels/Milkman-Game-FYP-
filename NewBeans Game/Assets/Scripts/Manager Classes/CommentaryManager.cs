using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentaryManager : MonoBehaviour
{
    public static CommentaryManager instance;
    public Text commentaryText;


    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameManager GM = FindObjectOfType<GameManager>();
        if (GM)
        {
            GM.playerDeath += CheckPlayerKill;
        }
    }

    public void CheckPlayerKill(PlayerController player, PlayerController killer)
    {

        if (killer == null)
        {
            commentaryText.text = ("Player " + player.playerNumber + " has suicided!");
        }
        else
        {
            commentaryText.text = ("Player " + player.playerNumber + " has been killed by Player " + killer.playerNumber);
        }
    }
}
