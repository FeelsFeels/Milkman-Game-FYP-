using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (Input.GetAxis(playerInfo.HorizontalInputAxis) >= 0.7f)
            {
                MoveSelectionRight();
            }
            else if (Input.GetAxis(playerInfo.HorizontalInputAxis) <= -0.7f)
            {
                MoveSelectionLeft();
            }

            //if (Input.GetButtonDown(playerInfo.AButtonInput))
            //{
            //    ConfirmSelectedCharacter();
            //}
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
                playerSelectManager.ChangeCharacter(playerInfo, currentPositionIndex);
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
        //playerSelectManager.ChangeCharacter(playerInfo, currentPositionIndex);
    }

}
