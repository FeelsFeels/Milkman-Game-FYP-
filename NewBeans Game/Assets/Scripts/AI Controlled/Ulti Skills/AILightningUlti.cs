using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AILightningUlti : AIPlayerSkillSetManager.SkillSet
    {
        public GameObject lightningBeamsPrefab;
        public Light directionalLight;

        public GameObject lightningSkillsBeamToDestroy;

        AIPlayerSkillSetManager skillManager;
        Transform skillUser;

        public override void SetManagerAndCharacter(AIPlayerSkillSetManager player)
        {
            skillManager = player;
            skillUser = player.transform;
        }

        public override void SkillAttack(AIPlayerSkillSetManager playerSkillManager)
        {
            skillUser = playerSkillManager.GetComponent<Transform>(); // Set the player transform
                                                                      //Activate shield

            AILightningUltiBeam lightningBeam = Instantiate(lightningBeamsPrefab, playerSkillManager.transform).GetComponent<AILightningUltiBeam>();
            lightningBeam.transform.position += new Vector3(0, 2, 0);
            lightningBeam.InitialiseLaser(playerSkillManager);
            lightningSkillsBeamToDestroy = lightningBeam.gameObject;
            StartCoroutine(SkillDurationTiming(playerSkillManager));
        }

        IEnumerator SkillDurationTiming(AIPlayerSkillSetManager manager)
        {
            AIPlayerController pc = manager.gameObject.GetComponent<AIPlayerController>();
            //Anticipation animation
            pc.moveRate = 0;
            pc.playerTurnSmoothing = 0.0f;
            yield return new WaitForSeconds(2f);    //Anticipation duration 2.0f

            //The laser beam is active and happy.
            pc.moveRate = 0;
            pc.playerTurnSmoothing = 0.4f;
            yield return new WaitForSeconds(skillDuration);
            EndUltimate(manager);
        }
        

        public override void EndUltimate(AIPlayerSkillSetManager playerSkillManager)
        {
            base.EndUltimate(playerSkillManager);
            StopAllCoroutines();

            //pc.moveRate = previousMoveRate;
            if (playerSkillManager)
            {
                playerSkillManager.GetComponent<AIPlayerController>().moveRate = 5;
                playerSkillManager.GetComponent<AIPlayerController>().playerTurnSmoothing = 10f;
            } 
            Destroy(lightningSkillsBeamToDestroy);
        }
    }
}