using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBeans.InstructionsScreen;

public class MovementSimulation : BaseSimulation
{
    public Transform player1ResetPos;
    public Transform player2ResetPos;

    public override void StartSimulation()
    {
        ResetSimulation();
        StartCoroutine("RulesSimulationRoutine");
    }
    public override void ResetSimulation()
    {
        StopAllCoroutines();

        if (player1 && player1ResetPos)
        {
            player1.transform.position = player1ResetPos.position;
            player1.transform.rotation = player1ResetPos.rotation;
        }
        if(player2 && player2ResetPos)
        {
            player2.transform.position = player2ResetPos.position;
            player2.transform.rotation = player2ResetPos.rotation;
        }
    }

    IEnumerator RulesSimulationRoutine()
    {
        player1.Move(AIPlayerInputController.Direction.Still);
        float timePassed = 0f;

        while (timePassed < 0.5f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        while (timePassed < 2.5f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.S);
            yield return null;
        }
        while (timePassed < 4.5f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.N);
            yield return null;
        }
        while (timePassed < 5.3f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.S);
            Vector3 turnDirection = player2.transform.position - player1.transform.position;
            player1.Turn(turnDirection.normalized);            
            yield return null;
        }
        player1.HoldShootButton();
        while (timePassed < 6.7f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.S);
            Vector3 turnDirection = (player2.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            //player1.Turn(turnDirection.normalized);
            yield return null;
        }
        player1.ReleaseShootButton();
        while (timePassed < 7.5f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.NE);
            yield return null;
        }
        player1.Move(AIPlayerInputController.Direction.Still);
        while (timePassed < 8.2f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        StartSimulation();
    }
}

