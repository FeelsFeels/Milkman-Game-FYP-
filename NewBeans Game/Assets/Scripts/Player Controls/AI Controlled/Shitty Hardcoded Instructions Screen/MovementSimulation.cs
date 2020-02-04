using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBeans.InstructionsScreen;

public class MovementSimulation : BaseSimulation
{
    public Transform player1ResetPos;

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
        while (timePassed < 2.0f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.E);
            yield return null;
        }
        while (timePassed < 3.5f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.W);
            yield return null;
        }
        while (timePassed < 4.5f)
        {
            timePassed += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.E);
            player1.Turn(new Vector3(-1f, 0f, 0f).normalized);
            yield return null;
        }
        Vector3 rot = new Vector3(-1, 0, 0);
        float step = 0;
        while (timePassed < 6f)
        {
            print("6sedc");
            timePassed += Time.deltaTime;
            step += Time.deltaTime;
            player1.Move(AIPlayerInputController.Direction.NE);
            player1.Turn(Vector3.Lerp(rot, new Vector3(0, 0, 1), step / 2f));
            yield return null;
        }
        
        while(timePassed < 12f)
        {
            yield return null;
        }
        yield return null;
        StartSimulation();
    }
}
