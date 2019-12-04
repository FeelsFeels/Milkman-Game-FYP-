using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBanana : MonoBehaviour
{
    PlayerController affectedPlayer;
    MeshCollider meshCollider;
    bool steppedOn;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {

        if(other.gameObject.tag == "Player" && !steppedOn)
        {
            //Make them slip and fall
            steppedOn = true;
            affectedPlayer = other.gameObject.GetComponent<PlayerController>();
            StartCoroutine(MakePlayerSlip());
        }
        if(other.gameObject.tag == "Hole")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator MakePlayerSlip()
    {
        float spinTime = 0;
        affectedPlayer.Hit(2.5f);

        //Make banana fly
        meshCollider.enabled = false;
        Vector3 flyDirection = (affectedPlayer.transform.forward + Vector3.up * 1.3f).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(flyDirection * 10000f);

        while(spinTime < 1)
        {
            spinTime += Time.deltaTime;
            affectedPlayer.transform.Rotate(0.0f, 10f, 0.0f);
            yield return null;
        }
    }
}
