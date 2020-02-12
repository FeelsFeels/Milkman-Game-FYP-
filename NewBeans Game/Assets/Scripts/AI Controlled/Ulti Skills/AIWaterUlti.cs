using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AIWaterUlti : AIPlayerSkillSetManager.SkillSet
    {

        public GameObject waterTornado;
        public float timeForTornadoGrowth = 0.2f;
        Transform skillUser;
        WaterTornado storm;
        AIPlayerSkillSetManager skillManager;


        public override void SetManagerAndCharacter(AIPlayerSkillSetManager player)
        {
            skillManager = player;
            skillUser = player.transform;
        }

        public override void SkillAttack(AIPlayerSkillSetManager playerSkillManager)
        {
            skillUser = playerSkillManager.GetComponent<Transform>(); // Set the player
            StartCoroutine(EndSkill(playerSkillManager));

            //Set where the tornado shld appear
            // Set the pos to the front of the player
            Vector3 pos = skillUser.position + skillUser.transform.forward * 5f;
            //Debug.DrawLine(pos, pos + Vector3.down * 10, Color.yellow, 5);
            //Instantiate
            WaterTornado tornado = Instantiate(waterTornado, pos, Quaternion.identity).GetComponent<WaterTornado>();
            Debug.Log(tornado);

            storm = tornado;
            tornado.SetSkillUser(skillUser);
            StartCoroutine(GrowTornado());
        }

        IEnumerator GrowTornado()
        {
            yield return new WaitForSeconds(timeForTornadoGrowth); //Wait for tornado to fully grow

            //Ref to tornado, and make tornado move forward
            storm.BrewStorm((skillUser.forward.normalized));
        }

        IEnumerator EndSkill(AIPlayerSkillSetManager manager)
        {
            yield return new WaitForSeconds(skillDuration);
            EndUltimate(manager);
        }
        public override void EndUltimate(AIPlayerSkillSetManager playerSkillManager)
        {
            base.EndUltimate(playerSkillManager);
            StopAllCoroutines();
            if(storm)
                storm.DestroyStorm();
            //StopAllCoroutines();
            //storm.DestroyStorm();
        }

    }
}