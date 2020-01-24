using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPushVFXController : MonoBehaviour
{
    PlayerController playerController;  //Needed to access the inputInfo inside
    public Transform objectPosition;
    public GameObject pushHandModel;
    public ParticleSystem vfxToPlay;
    Vector3 originalScaling = new Vector3(0.08f, 0.08f, 0.08f);
    Vector3 maxScaling = new Vector3(0.16f, 0.16f, 0.16f);

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        GameObject go = playerController.inputInfo.chosenCharacterData.chargingPushModel;
        pushHandModel = Instantiate(go, objectPosition.position, go.transform.rotation);
        go = playerController.inputInfo.chosenCharacterData.chargingPushVFX;
        vfxToPlay = Instantiate(go, objectPosition.position, go.transform.rotation).GetComponentInChildren<ParticleSystem>();

        pushHandModel.transform.parent = objectPosition.transform;
        vfxToPlay.gameObject.transform.parent = objectPosition.transform;
    }

    private void Start()
    {
        pushHandModel.SetActive(false);
    }

    public void StartVFX()
    {
        pushHandModel.transform.localScale = originalScaling;
        pushHandModel.SetActive(true);
        vfxToPlay.Play();
        StartCoroutine("ChangeScalingOverTime");
    }

    public void StopVFX()
    {
        pushHandModel.transform.localScale = originalScaling;
        pushHandModel.SetActive(false);
        vfxToPlay.Stop();
        StopCoroutine("ChangeScalingOverTime");
    }

    IEnumerator ChangeScalingOverTime()
    {
        float timeCharged = 0f;
        while (timeCharged < 1.5f)
        {
            timeCharged += Time.deltaTime;
            pushHandModel.transform.localScale = Vector3.Lerp(originalScaling, maxScaling, timeCharged / 1.2f);
            yield return null;
        }
        pushHandModel.transform.localScale = originalScaling;
    }
}
