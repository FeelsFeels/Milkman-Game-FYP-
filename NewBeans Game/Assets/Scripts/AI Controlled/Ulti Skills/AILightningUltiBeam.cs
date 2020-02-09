using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AILightningUltiBeam : MonoBehaviour
    {
        public Animator anticipationAnimator;
        public Animator leftBeamAnimator;
        public Animator rightBeamAnimator;
        GameObject castingPlayer;
        Collider collider;

        public void InitialiseLaser(AIPlayerSkillSetManager playerSkill)
        {
            castingPlayer = playerSkill.gameObject;
            transform.parent = playerSkill.transform;
            transform.localPosition = Vector3.up;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (other.gameObject == castingPlayer)
                    return;
                else
                {
                    AIPlayerController pc = other.gameObject.GetComponent<AIPlayerController>();
                    //Get direction of player to caster
                    Vector3 forceDir = (pc.transform.position - castingPlayer.transform.position).normalized;
                    pc.Hit(1.5f);
                    pc.rb.AddForce(300f * forceDir);
                }
            }
        }

        /// <summary>
        /// Camera shake fucking sucks and makes me giddy
        /// </summary>    
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
}