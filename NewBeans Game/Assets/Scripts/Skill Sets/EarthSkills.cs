using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkills : SkillSetManager.SkillSet
{
    public GameObject testing;

    public override void SkillAttack(SkillSetManager manager)
    {
        GameObject test = Instantiate(testing, manager.transform.position, Quaternion.identity);
        test.GetComponent<Rigidbody>().AddForce(transform.forward * 10);

        StartCoroutine(OhHeck(manager, test));
    }
    IEnumerator OhHeck(SkillSetManager manager, GameObject go)
    {
        Destroy(go, skillDuration);
        yield return new WaitForSeconds(skillDuration);
        EndUltimate(manager);
    }
}
