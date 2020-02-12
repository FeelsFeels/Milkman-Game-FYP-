using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class EarthUltimateSimulation : BaseSimulation
    {
        public Transform player1ResetPos;
        public Transform player2ResetPos;

        public override void ResetSimulation()
        {
            StopAllCoroutines();
            player1.ResetStates();
            player2.ResetStates();
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
        }

        public override void StartSimulation()
        {
            ResetSimulation();
            StartCoroutine("RulesSimulationRoutine1");
            StartCoroutine("RulesSimulationRoutine2");
            StartCoroutine("RulesSimulationRoutine3");
        }

        IEnumerator RulesSimulationRoutine1()
        {
            yield return new WaitForSeconds(0.5f);
            player1.GetComponent<AIPlayerSkillSetManager>().currentCharge = 13195f;
            player1.GetComponent<AIPlayerSkillSetManager>().ReleaseSpecialSkill();
            yield return new WaitForSeconds(1.0f);
            float timepassed = 0f;
            while (timepassed < 5.0f)
            {
                timepassed += Time.deltaTime;
                player1.Move(AIPlayerInputController.Direction.E);
                yield return null;
            }
            player1.Move(AIPlayerInputController.Direction.Still);
            yield return new WaitForSeconds(1.0f);
            StartSimulation();
        }
        IEnumerator RulesSimulationRoutine2()
        {
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i <= 5; i++)
            {
                player2.HoldShootButton();
                yield return new WaitForSeconds(0.2f);
                player2.ReleaseShootButton();
                yield return new WaitForSeconds(0.7f);
            }
        }
        IEnumerator RulesSimulationRoutine3()
        {
            yield return new WaitForSeconds(0.5f);
            float timepassed = 0f;
            while (timepassed < 5.0f)
            {
                Vector3 turnDirection = (player1.transform.position - player2.transform.position).normalized;
                player2.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), 10);
                yield return null;
            }
        }

    }
}
