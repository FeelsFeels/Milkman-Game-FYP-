using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseCharacter : MonoBehaviour
{
    public enum MoveState
    {
        Moving,
        Still,
        Selected
    }
    public MoveState moveState = MoveState.Still;
    public Transform[] selectPositions = new Transform[4];  //Positions to move to
    public TextMeshProUGUI playerNumberText;
    int currentPositionIndex = 0;   //Current position
    int targetPositionIndex;        //Next position
    float lerpStep;

    public PlayerInputInfo playerInfo;

    private PlayerSelectManager playerSelectManager;

    private void Awake()
    {
        playerSelectManager = FindObjectOfType<PlayerSelectManager>();
    }

    private void Update()
    {
        //Moving the player cursor
        if (moveState == MoveState.Still)
        {
            if (Input.GetAxis(playerInfo.HorizontalInputAxis) >= 0.1f)
            {
                MoveSelectionRight();
            }
            else if (Input.GetAxis(playerInfo.HorizontalInputAxis) <= -0.1f)
            {
                MoveSelectionLeft();
            }

            if (Input.GetButtonDown(playerInfo.AButtonInput))
            {
                ConfirmSelectedCharacter();
            }
        }
        else if (moveState == MoveState.Moving)
        {
            //Moving the indicator
            lerpStep += Time.deltaTime;
            transform.position = Vector3.Lerp(selectPositions[currentPositionIndex].position, selectPositions[targetPositionIndex].position, lerpStep / 0.2f);

            if ((lerpStep / 0.2f) >= 1)
            {
                currentPositionIndex = targetPositionIndex;
                lerpStep = 0;
                moveState = MoveState.Still;
                
                //Sets character 
                //playerSelectManager.ChangeCharacter(playerInfo, currentPositionIndex);
            }
        }
        else if(moveState == MoveState.Selected)
        {
            if (Input.GetButtonDown(playerInfo.BButtonInput))
            {
                UnselectCharacter();
            }
        }
    }
    
    void MoveSelectionRight()
    {        
        int newPositionIndex = currentPositionIndex + 1;
        if (newPositionIndex >= selectPositions.Length)
        {
            newPositionIndex %= selectPositions.Length;
        }
        targetPositionIndex = newPositionIndex;
        moveState = MoveState.Moving;
    }
    void MoveSelectionLeft()
    {
        int newPositionIndex = currentPositionIndex - 1;
        if (newPositionIndex < 0)
        {
            newPositionIndex += selectPositions.Length;
        }
        targetPositionIndex = newPositionIndex;
        moveState = MoveState.Moving;
    }

    void ConfirmSelectedCharacter()
    {
        if (playerSelectManager.CheckIfCharacterTaken(playerInfo, currentPositionIndex))
        {
            playerSelectManager.ChooseCharacter(playerInfo, currentPositionIndex);
            playerNumberText.color = playerInfo.chosenCharacterData.characterColor;
            moveState = MoveState.Selected;
        }
    }

    void UnselectCharacter()
    {
        playerSelectManager.UnchooseCharacter(playerInfo, currentPositionIndex);
        playerNumberText.color = Color.white;
        moveState = MoveState.Still;
    }

}
