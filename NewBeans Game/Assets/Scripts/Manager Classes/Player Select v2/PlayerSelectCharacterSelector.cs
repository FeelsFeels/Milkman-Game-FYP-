using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectCharacterSelector : MonoBehaviour
{
    public PlayerSelectBehaviour playerSelectBehaviour;
    public PlayerInputInfo playerInfo;

    bool horizontalHeld;
    bool verticalHeld;

    private void Awake()
    {
        playerSelectBehaviour = FindObjectOfType<PlayerSelectBehaviour>();
    }

    private void Update()
    {
        if (ReferenceEquals(playerInfo, null))
            return;

        float horizontal = Input.GetAxis(playerInfo.HorizontalInputAxis);
        float vertical = Input.GetAxis(playerInfo.VerticalInputAxis);

        //Horizontal movement
        if(horizontalHeld == false)
        {
            if(horizontal >= 0.5f)
            {
                MoveSelectionRight();
            }
            if(horizontal <= 0.5f)
            {
                MoveSelectionLeft();
            }
        }

        //Vertical movement
        if (verticalHeld == false)
        {
            if (vertical >= 0.5f)
            {
                MoveSelectionUp();
            }
            if (vertical <= 0.5f)
            {
                MoveSelectionDown();
            }
        }


        if (horizontal == 0)
        {
            horizontalHeld = false;
        }

        if (vertical == 0)
        {
            verticalHeld = false;
        }
    }

    public void PlayerJoined(PlayerInputInfo player)
    {
        playerInfo = player;
    }

    void MoveSelectionRight()
    {

    }
    void MoveSelectionLeft()
    {

    }
    void MoveSelectionUp()
    {

    }
    void MoveSelectionDown()
    {

    }
}
