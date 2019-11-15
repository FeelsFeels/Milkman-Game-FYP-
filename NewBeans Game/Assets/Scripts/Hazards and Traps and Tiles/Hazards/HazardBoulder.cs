using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBoulder : MonoBehaviour
{
    public Rigidbody rb;
    public bool canStunPlayer;
    public bool rockDropping;

    public float baseForce;

    public GameObject shockwaveParticles;
    public float knockbackStrength = 1800f;


    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void Initialise()
    {
        StartCoroutine(RockDroppingCoroutine());
    }

    public void Shockwave()
    {
        AutoDestroyOverTime particles = Instantiate(shockwaveParticles, transform.position, shockwaveParticles.transform.rotation).GetComponent<AutoDestroyOverTime>();
        particles.DestroyWithTime(0.3f);

        //Gets all players in range of shockwave stomp
        int ignoreLayerMask = ~1 << LayerMask.NameToLayer("Ground");    //Raycasts on everything but ground
        Collider[] inRange = Physics.OverlapSphere(transform.position, 10f, ignoreLayerMask);


        //Disrupts all players in range
        foreach (Collider collider in inRange)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player)
            {
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);

                player.Hit();
                print("Player kena FUCKED by tofu shockwave");
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.2f);
    //    Gizmos.DrawSphere(transform.position, 10f);
    //}

    private void Update()
    {
        float velocity = rb.velocity.magnitude;

        if (velocity > 5f)
            canStunPlayer = true;
        else
            canStunPlayer = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canStunPlayer)
        {
            //Need to check direction it is moving.
            Vector3 directionToPlayer = (collision.transform.position - transform.position).normalized;
            Vector3 moveDirection = rb.velocity.normalized;
            if (Vector3.Dot(moveDirection, directionToPlayer) > 0)
            {
                //Stuns + knocks back other player
                collision.gameObject.GetComponent<PlayerController>().Hit(1f);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection * baseForce * rb.velocity.magnitude / 2);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            gameObject.SetActive(false);

            if (TofuBlockManager.instance != null)
            {
                TofuBlockManager.instance.SpawnTofuWithDelay(Random.Range(5, 10));
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

    IEnumerator RockDroppingCoroutine()
    {
        while(transform.position.y > 0.9f)
        {
            rb.isKinematic = true;
            rb.MovePosition(transform.position + Vector3.down * 2.5f);
            yield return null;
        }
        rb.isKinematic = false;
        transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z);
        rb.velocity = Vector3.zero;

        Shockwave();
    }
}
