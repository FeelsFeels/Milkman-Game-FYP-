using NewBeans.InstructionsScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NewBeans.InstructionsScreen
{
    public class UltimateSimulation : BaseSimulation
    {
        public VideoPlayer videoPlayer;
        public AIControlsScreenUI ultiUI;

        public override void ResetSimulation()
        {

        }

        public override void StartSimulation()
        {
            ResetSimulation();
            StartCoroutine("RulesSimulationRoutine");
        }

        IEnumerator RulesSimulationRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            player1.GetComponent<AIPlayerSkillSetManager>().currentCharge = 13195f;
            player1.GetComponent<AIPlayerSkillSetManager>().ReleaseSpecialSkill();
            yield return new WaitForSeconds(0.5f);
        }
    }
}