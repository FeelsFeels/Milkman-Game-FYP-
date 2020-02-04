using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkills : SkillSetManager.SkillSet
{
    public GameObject shockwaveParticles;

    public float timeToNextShockwave = 0f;
    public float knockbackRadius;
    public float knockbackStrength;
    Transform skillUser;
    public bool startSmashing;

    public override void SkillAttack(SkillSetManager manager)
    {
        skillUser = manager.GetComponent<Transform>(); // Set the player
        startSmashing = true;

        // Size up player
        skillUser.localScale *= 3;
        StartCoroutine(EndSkill(manager));
        StartCoroutine(UpdatePlayerBehaviour());

    }

    IEnumerator UpdatePlayerBehaviour()
    {
        while (startSmashing) //Keep updating while this is true
        {
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
            PlayerController player = collider.GetComponent<PlayerController>();
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


    IEnumerator EndSkill(SkillSetManager manager)
    {
        Debug.Log("I've stopped being a rock golem");
        yield return new WaitForSeconds(skillDuration);
 
        EndUltimate(manager);
    }
    public override void EndUltimate(SkillSetManager playerSkillManager)
    {
        base.EndUltimate(playerSkillManager);
        StopAllCoroutines();
        //Stop the smash
        startSmashing = false;
        //Size down player
        playerSkillManager.transform.localScale = Vector3.one;
    }
}
