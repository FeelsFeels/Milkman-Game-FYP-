using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacter : MonoBehaviour
{
    public enum MoveState
    {
        Moving,
        Still
    }
    public MoveState moveState = MoveState.Still;
    int currentPositionIndex = 0;
    int targetPositionIndex;
    float lerpStep;

    public Transform[] selectPositions = new Transform[4];

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
        }
        else if (moveState == MoveState.Moving)
        {
            lerpStep += Time.deltaTime;
            //print(currentPositionIndex);
            //print(targetPositionIndex);
            transform.position = Vector3.Lerp(selectPositions[currentPositionIndex].position, selectPositions[targetPositionIndex].position, lerpStep / 0.2f);

            if ((lerpStep / 0.2f) >= 1)
            {
                currentPositionIndex = targetPositionIndex;
                lerpStep = 0;
                moveState = MoveState.Still;
                //UpdateCharacterSelection();
                playerSelectManager.ChangeCharacter(playerInfo, currentPositionIndex);
            }
        }
    }

    void UpdateCharacterSelection()
    {
        playerInfo.chosenCharacterIndex = currentPositionIndex;
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

}
