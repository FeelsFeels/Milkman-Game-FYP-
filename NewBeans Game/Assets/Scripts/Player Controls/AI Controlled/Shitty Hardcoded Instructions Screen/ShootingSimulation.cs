using NewBeans.InstructionsScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSimulation : BaseSimulation
{ 
    public AIPlayerInputController player3;
    public AIPlayerInputController player4;

    public Transform player1ResetPos;
    public Transform player2ResetPos;
    public Transform player3ResetPos;
    public Transform player4ResetPos;
    [Space]
    public Transform secondShootPos;
    public Transform thirdShootPos;

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
        if (player2 && player2ResetPos)
        {
            player2.transform.position = player2ResetPos.position;
            player2.transform.rotation = player2ResetPos.rotation;
        }
        if (player3 && player3ResetPos)
        {
            player3.transform.position = player3ResetPos.position;
            player3.transform.rotation = player3ResetPos.rotation;
        }
        if (player4 && player4ResetPos)
        {
            player4.transform.position = player4ResetPos.position;
            player4.transform.rotation = player4ResetPos.rotation;
        }
    }

    IEnumerator RulesSimulationRoutine()
    {
        float timepassed = 0;
        while (timepassed < 0.5f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        player1.HoldShootButton();
        while(timepassed < 1.0f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        player1.ReleaseShootButton();
        while(timepassed < 2.0f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        timepassed = 0f;
        while((player1.transform.position - secondShootPos.position).sqrMagnitude > 0.05f)
        {
            Vector3 direction = (secondShootPos.position - player1.transform.position).normalized;
            player1.Move(direction);
            Vector3 turnDirection = (player3.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            yield return null;
        }
        player1.Move(AIPlayerInputController.Direction.Still);
        while (timepassed < 0.9f)
        {
            Vector3 turnDirection = (player3.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            timepassed += Time.deltaTime;
            yield return null;
        }
        player1.HoldShootButton();
        while(timepassed < 2.3f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        player1.ReleaseShootButton();
        while(timepassed < 3.3f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        timepassed = 0f;
        while ((player1.transform.position - thirdShootPos.position).sqrMagnitude > 0.05f)
        {
            Vector3 direction = (thirdShootPos.position - player1.transform.position).normalized;
            player1.Move(direction);
            Vector3 turnDirection = (player4.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            yield return null;
        }
        player1.Move(AIPlayerInputController.Direction.Still);
        while (timepassed < 0.9f)
        {
            Vector3 turnDirection = (player4.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            timepassed += Time.deltaTime;
            yield return null;
        }
        player1.HoldShootButton();
        while (timepassed < 5.5f)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        StartSimulation();
        yield return null;
    }
}
