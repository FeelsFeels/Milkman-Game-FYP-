using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkills : SkillSetManager.SkillSet
{
    public GameObject lightningBeamsPrefab;
    public Light directionalLight;
    
    public GameObject lightningSkillsBeamToDestroy;

    Transform skillUser;

    void Awake()
    {
        //directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        //StartCoroutine(DimLighting());
    }

    public override void SkillAttack(SkillSetManager playerSkillManager)
    {
        skillUser = playerSkillManager.GetComponent<Transform>(); // Set the player transform
        //Activate shield
        if (skillUser.Find("InvincibilityShield"))
        {
           Shield shield = skillUser.Find("InvincibilityShield").GetComponent<Shield>();
           shield.shieldRenderer.enabled = true;
           shield.shieldCollider.enabled = true;
        }

        LightningSkillsBeam lightningBeam = Instantiate(lightningBeamsPrefab, playerSkillManager.transform).GetComponent<LightningSkillsBeam>();
        lightningBeam.InitialiseLaser(playerSkillManager);
        lightningSkillsBeamToDestroy = lightningBeam.gameObject;
        StartCoroutine(DimLighting());
        StartCoroutine(SkillDurationTiming(playerSkillManager));
    }
    
    IEnumerator SkillDurationTiming(SkillSetManager manager)
    {
        PlayerController pc = manager.gameObject.GetComponent<PlayerController>();
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

    IEnumerator DimLighting()
    {
        float lerpTime = 0;
        float time = 2f;
        Color32 targetColor = new Color32(135, 135, 135, 255);
        while (lerpTime / time < 3)
        {
            lerpTime += Time.deltaTime;
            directionalLight.color = Color32.Lerp(directionalLight.color, targetColor, lerpTime / time);
            yield return null;
        }
        yield return null;
    }

    IEnumerator RestoreLighting()
    {
        float lerpTime = 0;
        float time = 2f;
        Color32 targetColor = new Color32(255, 253, 232, 255);
        while (lerpTime / time < 3)
        {
            lerpTime += Time.deltaTime;
            directionalLight.color = Color32.Lerp(directionalLight.color, targetColor, lerpTime / time);
            yield return null;
        }
        yield return null;
    }

    public override void EndUltimate(SkillSetManager playerSkillManager)
    {
        base.EndUltimate(playerSkillManager);
        StopAllCoroutines();

        //pc.moveRate = previousMoveRate;
        playerSkillManager.GetComponent<PlayerController>().moveRate = 5;
        playerSkillManager.GetComponent<PlayerController>().playerTurnSmoothing = 10f;

        StartCoroutine(RestoreLighting());
        Destroy(lightningSkillsBeamToDestroy);

        //Deactivate shield
        if (skillUser)
        {
            Shield shield = skillUser.Find("InvincibilityShield").GetComponent<Shield>();
            shield.shieldRenderer.enabled = false;
            shield.shieldCollider.enabled = false;
        }
    }
}
