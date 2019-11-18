using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkills : SkillSetManager.SkillSet
{
    public GameObject lightningBeamsPrefab;
    public override void SkillAttack(SkillSetManager playerSkillManager)
    {
        LightningSkillsBeam lightningBeam = Instantiate(lightningBeamsPrefab, playerSkillManager.transform).GetComponent<LightningSkillsBeam>();
        lightningBeam.InitialiseLaser(playerSkillManager);
        StartCoroutine(SkillDurationTiming(playerSkillManager, lightningBeam.gameObject));
    }
    
    IEnumerator SkillDurationTiming(SkillSetManager manager, GameObject go)
    {
        PlayerController pc = manager.gameObject.GetComponent<PlayerController>();
        //float previousMoveRate = pc.moveRate;
        pc.moveRate = 0;
        pc.playerTurnSmoothing = 0.2f;

        yield return new WaitForSeconds(skillDuration);

        //pc.moveRate = previousMoveRate;
        pc.moveRate = 10;
        pc.playerTurnSmoothing = 10f;

        Destroy(go);
        EndUltimate(manager);
    }

}
