using NewBeans.InstructionsScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingSimulation : BaseSimulation
{
    public Transform player1ResetPos;
    public Transform player2ResetPos;
    public Transform rockResetPos;

    [Space]
    public GameObject rock;
    public Transform secondShootPos;
    public Transform thirdShootPos;

    public override void ResetSimulation()
    {
        StopAllCoroutines();
        player1.ResetStates();
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
        if(rock && rockResetPos)
        {
            rock.transform.position = rockResetPos.position;
            rock.transform.rotation = rockResetPos.rotation;
        }
    }

    public override void StartSimulation()
    {
        ResetSimulation();
        StartCoroutine("RulesSimulationRoutine");
    }

    IEnumerator RulesSimulationRoutine()
    {
        float timepassed = 0f;
        yield return new WaitForSeconds(0.5f);
        while(timepassed < 0.5f)
        {
            timepassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.E);
            yield return null;
        }
        player1.Move(AIPlayerInputController.Direction.Still);
        yield return new WaitForSeconds(0.5f);
        player1.HoldPullButton();
        yield return new WaitForSeconds(0.3f);
        player1.ReleasePullButton();
        yield return new WaitForSeconds(1.2f);
        timepassed = 0f;
        while (timepassed < 0.5f)
        {
            timepassed += Time.deltaTime;
            //player2.Move(AIPlayerInputController.Direction.E);
            Vector3 turnDirection = (player2.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            yield return null;
        }
        player1.HoldPullButton();
        while (timepassed < 0.8f)
        {
            timepassed += Time.deltaTime;
            //player2.Move(AIPlayerInputController.Direction.E);
            Vector3 turnDirection = (player2.transform.position - player1.transform.position).normalized;
            player1.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
            yield return null;
        }
        player1.ReleasePullButton();
        yield return new WaitForSeconds(1.5f);

        StartSimulation();
    }
}
