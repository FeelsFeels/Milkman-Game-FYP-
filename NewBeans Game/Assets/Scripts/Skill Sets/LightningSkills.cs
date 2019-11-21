using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkills : SkillSetManager.SkillSet
{
    public GameObject lightningBeamsPrefab;
    public Light directionalLight;

    void Awake()
    {
        //directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        //StartCoroutine(DimLighting());
    }

    public override void SkillAttack(SkillSetManager playerSkillManager)
    {
        LightningSkillsBeam lightningBeam = Instantiate(lightningBeamsPrefab, playerSkillManager.transform).GetComponent<LightningSkillsBeam>();
        lightningBeam.InitialiseLaser(playerSkillManager);
        StartCoroutine(DimLighting());
        StartCoroutine(SkillDurationTiming(playerSkillManager, lightningBeam.gameObject));
    }
    
    IEnumerator SkillDurationTiming(SkillSetManager manager, GameObject go)
    {
        PlayerController pc = manager.gameObject.GetComponent<PlayerController>();

        //Anticipation animation
        pc.moveRate = 0;
        pc.playerTurnSmoothing = 0.0f;
        yield return new WaitForSeconds(2f);    //Anticipation duration 2.0f

        //The laser beam is active and happy.
        pc.moveRate = 0;
        pc.playerTurnSmoothing = 0.2f;
        yield return new WaitForSeconds(skillDuration);

        //pc.moveRate = previousMoveRate;
        pc.moveRate = 10;
        pc.playerTurnSmoothing = 10f;

        StartCoroutine(RestoreLighting());

        Destroy(go);
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

}
