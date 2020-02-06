using NewBeans.InstructionsScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NewBeans.InstructionsScreen
{
    public class UltimateSimulation : BaseSimulation
    {
        public Transform player1ResetPos;
        public Transform player2ResetPos;

        public AIControlsScreenUI ultiUI;

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
        }

        public override void StartSimulation()
        {
            ResetSimulation();
            StartCoroutine("RulesSimulationRoutine");
        }

        IEnumerator RulesSimulationRoutine()
        {
            player1.GetComponent<AIPlayerSkillSetManager>().currentCharge = 13195f;
            ultiUI.FullCharge();
            yield return new WaitForSeconds(1.0f);
            player1.GetComponent<AIPlayerSkillSetManager>().ReleaseSpecialSkill();
            ultiUI.UseUlti();
            yield return new WaitForSeconds(1.0f);
            float timepassed = 0f;
            while (timepassed < 3.0f)
            {
                timepassed += Time.deltaTime;
                player1.Move(AIPlayerInputController.Direction.E);
                yield return null;
            }
            player1.Move(AIPlayerInputController.Direction.Still);
            yield return new WaitForSeconds(2.0f);
            StartSimulation();
        }
    }
}