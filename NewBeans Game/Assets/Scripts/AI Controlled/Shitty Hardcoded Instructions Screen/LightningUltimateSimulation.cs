using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NewBeans.InstructionsScreen
{
    public class LightningUltimateSimulation : BaseSimulation
    {
        public Transform player1ResetPos;
        public Transform player2ResetPos;

        public Tween rotTween;

        private void Awake()
        {
            rotTween = player1.transform.DORotate(Vector3.zero, 0);
        }

        public override void ResetSimulation()
        {
            StopAllCoroutines();
            player1.ResetStates();
            player2.ResetStates();
            rotTween.Complete();
            if (player1 && player1ResetPos)
            {
                player1.transform.position = player1ResetPos.position;
                player1.transform.rotation = player1ResetPos.rotation;
                player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (player2 && player2ResetPos)
            {
                player2.transform.position = player2ResetPos.position;
                player2.transform.rotation = player2ResetPos.rotation;
                player2.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public override void StartSimulation()
        {
            ResetSimulation();
            print("dqwdwq");
            StartCoroutine("RulesSimulationRoutine1");
        }

        IEnumerator RulesSimulationRoutine1()
        {
            yield return new WaitForSeconds(0.5f);
            player1.GetComponent<AIPlayerSkillSetManager>().currentCharge = 13195f;
            player1.GetComponent<AIPlayerSkillSetManager>().ReleaseSpecialSkill();
            yield return new WaitForSeconds(2.3f);
            print("rotate 1");
            rotTween = player1.transform.DOLocalRotate(new Vector3(0, 89, 0), 2);
            yield return new WaitForSeconds(2.5f);
            print("rotate 2");
            rotTween = player1.transform.DOLocalRotate(Vector3.zero, 2);
            yield return new WaitForSeconds(4f);
            StartSimulation();
        }
    }
}
