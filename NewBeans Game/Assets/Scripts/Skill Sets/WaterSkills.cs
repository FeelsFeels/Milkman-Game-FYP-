using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkills : SkillSetManager.SkillSet
{
    public GameObject waterTornado;
    public float timeForTornadoGrowth = 0.2f;
    Transform skillUser;
    WaterTornado storm;

    public override void SkillAttack(SkillSetManager manager)
    {
        skillUser = manager.GetComponent<Transform>(); // Set the player
        StartCoroutine(EndSkill(manager));

        //Set where the tornado shld appear
        // Set the pos to the front of the player
        Vector3 pos = skillUser.position + skillUser.TransformDirection(skillUser.forward) * 5f + skillUser.TransformDirection(skillUser.up) * 0.1f;
        Debug.DrawLine(pos, pos + Vector3.down * 10, Color.yellow, 5);
        //Instantiate
        WaterTornado tornado = Instantiate(waterTornado, pos, Quaternion.identity).GetComponent<WaterTornado>();
        Debug.Log(tornado);

        storm = tornado;
        tornado.SetSkillUser(skillUser);
        StartCoroutine(GrowTornado());

    }
    

    IEnumerator GrowTornado()
    {
        yield return new WaitForSeconds(timeForTornadoGrowth);

        //Ref to tornado, and make tornado move forward
        storm.BrewStorm((skillUser.forward.normalized));
    }

    IEnumerator EndSkill(SkillSetManager manager)
    {
        Debug.Log("Tornado has stopped");
        yield return new WaitForSeconds(skillDuration);

        storm.DestroyStorm();
        EndUltimate(manager);
    }
    //private void Update()
    //{
    //    if(skillUser != null)
    //    Debug.Log(skillUser.position);
    //}
}
