using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkillsBeam : MonoBehaviour
{
    public Animator anticipationAnimator;
    public Animator leftBeamAnimator;
    public Animator rightBeamAnimator;
    GameObject castingPlayer;
    Collider collider;

    public void InitialiseLaser(SkillSetManager playerSkill)
    {
        castingPlayer = playerSkill.gameObject;
        transform.parent = playerSkill.transform;
        transform.position = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject == castingPlayer)
                return;
            else
            {
                PlayerController pc = other.gameObject.GetComponent<PlayerController>();
                //Get direction of player to caster
                Vector3 forceDir = (pc.transform.position - castingPlayer.transform.position).normalized;
                pc.Hit(1.5f);
                pc.rb.AddForce(300f * forceDir);
            }
        }
    }


    //Light shaking during charge
    public void ShakeCamera1()
    {
        //FindObjectOfType<CameraControls>().ShakeCamera(1.9f, 0f);
    }

    //Heavy shaking during actual beam
    public void ShakeCamera2()
    {
        //FindObjectOfType<CameraControls>().ShakeCamera(6f, 0f);
    }

}
