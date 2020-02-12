using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUltiSkill : SkillSetManager.SkillSet
{
    Transform skillUser;

    public bool beyblading;
    public bool showVFX;

    public float knockbackRadius = 10f;
    public float knockbackStrength;
    

    public GameObject ultiPrefab;
    public GameObject fireCharacter;
    public GameObject thisUlti;

    //private GrapplingHook hook;

    public float skillLifetime;
    private SkillSetManager skillManager;

    void Start()
    {
        SkillSetManager[] managers = FindObjectsOfType<SkillSetManager>();
        for (int i = 0; i < managers.Length; i++)
        {
            if(managers[i].playerAvatar == SkillSetManager.characterChosen.Fire)
            {
                skillManager = managers[i];
                fireCharacter = managers[i].gameObject;
                break;
            }
        }
    }


    void Update()
    {
        if (thisUlti != null)
        {
            thisUlti.transform.Rotate(0, 10, 0, Space.Self);
            thisUlti.transform.position = new Vector3(fireCharacter.transform.position.x, fireCharacter.transform.position.y + 3.5f, fireCharacter.transform.position.z);
        }

        if (fireCharacter != null)
        {
            if (fireCharacter.GetComponent<PlayerController>().isDead)
            {
                Destroy(thisUlti);
            }
        }
    }

    public override void SkillAttack(SkillSetManager manager)
    {

        skillUser = manager.GetComponent<Transform>(); // Set the player
        beyblading = true;
        showVFX = true;

        StartCoroutine(EndSkill(manager));
        StartCoroutine(UpdatePlayerBehaviour());

    }

    public void Beyblade()
    {
        if (showVFX == true)
        {
            thisUlti = Instantiate(ultiPrefab, fireCharacter.transform.position, Quaternion.identity);
            showVFX = false;
            return;
        }

        int ignoreLayerMask = ~1 << LayerMask.NameToLayer("Ground");    //Raycasts on everything but ground


        List<Collider> inRange = new List<Collider>();
        inRange.AddRange(Physics.OverlapSphere(skillUser.position, knockbackRadius));
        if (inRange.Contains(skillUser.GetComponent<Collider>())) //If it includes this player,
        {
            inRange.Remove(skillUser.GetComponent<Collider>()); // remove

        }

        //Disrupts all players in range
        foreach (Collider collider in inRange)
        {
            GrapplingHook hook = GetComponent<GrapplingHook>();
            if (hook)
            {
                hook.latchedObject = null;
                hook.StartTakeBack();
            }

            PlayerController player = collider.GetComponent<PlayerController>();
            if (player)
            {
                Vector3 knockbackDirection = (player.transform.position - skillUser.position).normalized;
                player.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);
            }

            PushProjectile projectile = GetComponent<PushProjectile>();
            if (collider.gameObject.tag == "PushProjectile") 
            {
                //Vector3 knockbackDirection = (collider.transform.position - skillUser.position).normalized;
                //collider.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);
                Destroy(collider.gameObject);
            }
        }
    }

    IEnumerator UpdatePlayerBehaviour()
    {
        while (beyblading) //Keep updating while this is true
        {
            fireCharacter.GetComponent<PlayerController>().moveRate = 12f;
            Beyblade();
            
            yield return null;
        }

    }
    IEnumerator EndSkill(SkillSetManager manager)
    {
        yield return new WaitForSeconds(skillDuration);
        EndUltimate(manager);
        // Stops the beyblade
    }
    public override void EndUltimate(SkillSetManager playerSkillManager)
    {
        base.EndUltimate(playerSkillManager);
        StopAllCoroutines();
        fireCharacter.GetComponent<PlayerController>().moveRate = 5f;
        beyblading = false;
    }
}
