using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AIEarthUlti : AIPlayerSkillSetManager.SkillSet
    {
        public GameObject shockwaveParticles;

        public float timeToNextShockwave = 0f;
        public float knockbackRadius;
        public float knockbackStrength;
        AIPlayerSkillSetManager skillManager;
        Transform skillUser;
        public bool startSmashing;
        Rigidbody userRb;




        public override void SetManagerAndCharacter(AIPlayerSkillSetManager player)
        {
            skillManager = player;
            skillUser = player.transform;
        }

        public override void SkillAttack(AIPlayerSkillSetManager manager)
        {
            skillUser = manager.GetComponent<Transform>(); // Set the player
            startSmashing = true;

            // Size up player
            skillUser.localScale *= 3;
            //Add weight
            userRb = skillUser.GetComponent<Rigidbody>();
            userRb.mass = 20;

            StartCoroutine(EndSkill(manager));
            StartCoroutine(UpdatePlayerBehaviour());

        }

        IEnumerator UpdatePlayerBehaviour()
        {
            while (startSmashing) //Keep updating while this is true
            {
                //Disable player getting stunned, let's try let's go
                if (skillUser.GetComponent<AIPlayerController>().playerStunned)
                {
                    AIPlayerController player = skillUser.GetComponent<AIPlayerController>();
                    player.playerStunned = false;
                    player.stunnedTime = 0;
                    player.dizzyStars.SetActive(false);
                }

                timeToNextShockwave += Time.deltaTime;

                if (timeToNextShockwave >= 0.666f)
                {
                    timeToNextShockwave = 0;
                    Shockwave();
                }

                yield return null;
            }

        }

        void Shockwave()
        {
            AutoDestroyOverTime particles = Instantiate(shockwaveParticles, skillUser.position, shockwaveParticles.transform.rotation).GetComponent<AutoDestroyOverTime>();
            particles.DestroyWithTime(0.3f);

            //Gets all players in range of shockwave stomp
            int ignoreLayerMask = ~1 << LayerMask.NameToLayer("Ground");    //Raycasts on everything but ground

            List<Collider> inRange = new List<Collider>();
            inRange.AddRange(Physics.OverlapSphere(skillUser.position, knockbackRadius, ignoreLayerMask));
            if (inRange.Contains(skillUser.GetComponent<Collider>())) //If it includes this player,
            {
                inRange.Remove(skillUser.GetComponent<Collider>()); // remove

            }

            //Disrupts all players in range
            foreach (Collider collider in inRange)
            {
                AIPlayerController player = collider.GetComponent<AIPlayerController>();
                if (player)
                {
                    Vector3 knockbackDirection = (player.transform.position - skillUser.position).normalized;
                    player.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);

                    player.Hit();
                }
                else if (collider.tag == "Rock")
                {
                    Vector3 knockbackDirection = (collider.transform.position - skillUser.position).normalized;
                    collider.GetComponent<HazardBoulder>().rb.AddForce(knockbackStrength * knockbackDirection);
                }
            }
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
            //Stop the smash
            startSmashing = false;

            //Size down player
            playerSkillManager.transform.localScale = Vector3.one;
            //Resume normal mass
            if (userRb)
            {
                userRb.mass = 1;
            }
        }
    }
}