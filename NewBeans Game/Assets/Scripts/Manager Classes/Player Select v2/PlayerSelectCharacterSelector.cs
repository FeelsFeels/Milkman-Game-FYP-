using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectCharacterSelector : MonoBehaviour
{
    public PlayerSelectBehaviour playerSelectBehaviour;
    public PlayerInputInfo playerInfo;

    public bool activated;
    public int charHoverOverIndex;
    public bool chosenCharacter;

    bool horizontalHeld;
    bool verticalHeld;

    private void Awake()
    {
        playerSelectBehaviour = FindObjectOfType<PlayerSelectBehaviour>();
        charHoverOverIndex = 0;
    }
    
    private void Update()
    {
        if (!activated)
            return;
        

        float horizontal = Input.GetAxis(playerInfo.HorizontalInputAxis);
        float vertical = Input.GetAxis(playerInfo.VerticalInputAxis);

        //Horizontal movement
        if(horizontalHeld == false)
        {
            if (playerInfo.chosenCharacterData == null)
            {
                if (horizontal >= 0.5f)
                {
                    horizontalHeld = true;
                    MoveSelectionRight();
                }
                if (horizontal <= -0.5f)
                {
                    horizontalHeld = true;
                    MoveSelectionLeft();
                }
            }
        }

        //Vertical movement
        if (verticalHeld == false)
        {
            if (playerInfo.chosenCharacterData == null)
            {
                if (vertical >= 0.5f)
                {
                    verticalHeld = true;
                    MoveSelectionDown();
                }
                if (vertical <= -0.5f)
                {
                    verticalHeld = true;
                    MoveSelectionUp();
                }
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

        if(!horizontalHeld && !verticalHeld)
        {
            if (Input.GetButtonDown(playerInfo.AButtonInput))
            {
                SelectCharacter();
            }
            if (Input.GetButtonDown(playerInfo.BButtonInput))
            {
                DeselectCharacter();
            }
        }
    }

    public void PlayerJoined(PlayerInputInfo player)
    {
        StartCoroutine("ActivateInHalfSec");
    }

    void MoveSelectionRight()
    {
        if (charHoverOverIndex != 1 && charHoverOverIndex != 3)
        {
            //print("RIGHT");
            charHoverOverIndex++;
            playerSelectBehaviour.UpdateUI();
        }
    }
    void MoveSelectionLeft()
    {
        if (charHoverOverIndex != 0 && charHoverOverIndex != 2)
        {
            //print("LEFT");
            charHoverOverIndex--;
            playerSelectBehaviour.UpdateUI();
        }

    }
    void MoveSelectionUp()
    {
        if (charHoverOverIndex != 0 && charHoverOverIndex != 1)
        {
            //print("UP");
            charHoverOverIndex -= 2;
            playerSelectBehaviour.UpdateUI();
        }

    }
    void MoveSelectionDown()
    {
        if (charHoverOverIndex != 2 && charHoverOverIndex != 3)
        {
            //print("DOWN");
            charHoverOverIndex += 2;
            playerSelectBehaviour.UpdateUI();
        }
    }

    void SelectCharacter()
    {
        if (playerInfo.chosenCharacterData != null)
            return;

        //print("Is character taken? Answer: " + playerSelectBehaviour.CheckIfCharacterTaken(playerInfo, charHoverOverIndex));
        if (!playerSelectBehaviour.CheckIfCharacterTaken(playerInfo, charHoverOverIndex))
        {
            //print("selection");
            playerSelectBehaviour.ChooseCharacter(playerInfo, charHoverOverIndex);
        }
    }
    void DeselectCharacter()
    {
        if (playerInfo.chosenCharacterData != null)
        {
            //print("deselection");
            playerSelectBehaviour.UnchooseCharacter(playerInfo, charHoverOverIndex);
        }
    }

    IEnumerator ActivateInHalfSec()
    {
        yield return new WaitForSeconds(0.2f);
        activated = true;
    }
}
