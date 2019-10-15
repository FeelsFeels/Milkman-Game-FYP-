using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBoulder : MonoBehaviour
{
    public Rigidbody rb;
    public bool canStunPlayer;

    public float baseForce;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float velocity = rb.velocity.magnitude;

        if (velocity > 5f)
            canStunPlayer = true;
        else
            canStunPlayer = false;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" && canStunPlayer)
        {
            //Need to check direction it is moving.
            Vector3 directionToPlayer = (collision.transform.position - transform.position).normalized;
            Vector3 moveDirection = rb.velocity.normalized;
            if (Vector3.Dot(moveDirection, directionToPlayer) > 0)
            {
                //Stuns + knocks back other player
                collision.gameObject.GetComponent<PlayerController>().Hit(2f);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection * baseForce * rb.velocity.magnitude / 2);
                print("GET FUKKED");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IAffectedByWeight>() != null)
        {
            other.GetComponent<IAffectedByWeight>().AddWeight(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IAffectedByWeight>() != null)
        {
            other.GetComponent<IAffectedByWeight>().RemoveWeight(1);
        }
    }
}
