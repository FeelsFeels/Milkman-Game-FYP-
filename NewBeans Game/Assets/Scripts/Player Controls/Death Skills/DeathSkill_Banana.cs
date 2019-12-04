using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathSkill_Banana : BaseDeathSkill
{
    public GameObject bananaPrefab;
    public GameObject aimingCanvas;
    public Image aimingImage;
    public Image chargingImage;
    public float timeCharged;
    bool skillUsed;

    public GeneralMovement generalMovement;

    private void Update()
    {
        if (assignedPlayer == null || assignedController == null)
            return;

        if (!skillUsed)
        {
            //timeCharged += Time.deltaTime;
            //chargingImage.fillAmount = timeCharged / 10f;
            //if(timeCharged >= 10f)
            //{
            //    OnUse();
            //}
        }

        

        if (Input.GetButtonDown(AButtonInput))
        {
            OnUse();
        }

        float moveVerticalAxis = Input.GetAxisRaw(VerticalInputAxis);
        float moveHorizontalAxis = Input.GetAxisRaw(HorizontalInputAxis);
        Vector3 input = new Vector3(moveHorizontalAxis, 0, -moveVerticalAxis);
        Vector3 direction = input.normalized;

        if (moveHorizontalAxis != 0 || moveVerticalAxis != 0)
        {
            generalMovement.Move(direction);
        }
    }
    

    public override void OnReady()
    {
        aimingCanvas.SetActive(true);
        aimingImage.color = assignedPlayer.inputInfo.chosenCharacterData.characterColor;
    }

    public override void OnUse()
    {
        if (!skillUsed)
        {
            skillUsed = true;
            chargingImage.enabled = false;
            StartCoroutine(ThrowBananaRoutine());
        }
    }

    public override void OnFinishSkill()
    {
        assignedController.FinishUsingSkill();
        Destroy(gameObject);
    }

    IEnumerator ThrowBananaRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject banana = Instantiate(bananaPrefab, aimingCanvas.transform.position + Vector3.up * 100f, Quaternion.identity);
            banana.GetComponent<Rigidbody>().velocity = Vector3.down * 80f;
            yield return new WaitForSeconds(0.25f);
        }
        OnFinishSkill();
    }

}
