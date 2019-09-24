using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBoulder : MonoBehaviour
{
    public float weight;
    public Rigidbody rb;
    public bool canStunPlayer;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float velocity = rb.velocity.magnitude;
        print(velocity);

        if (velocity > 5f)
            canStunPlayer = true;
        else
            canStunPlayer = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && canStunPlayer)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canStunPlayer)
        {
            //Need to check direction it is moving.
            Vector3 directionToPlayer = (other.transform.position - transform.position).normalized;
            Vector3 moveDirection = rb.velocity.normalized;
            if (Vector3.Dot(moveDirection, directionToPlayer) > 0)
            {
                //Stuns other player
            }
        }

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
