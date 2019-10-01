using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolem : MonoBehaviour
{

    enum GolemStates
    {
        Idle,
        Patrolling,
        FindingNewPosition,
        Chasing,
        Hit,
        BeingLitAFLikeASuperDuperDopeAFMasterIMeanLikeThatSkirtComplimentsYourBodySoooooWellGurlIdKillForABodyLikeYoursMode
    }

    GolemStates golemState = GolemStates.FindingNewPosition;

    Rigidbody rb;
    
    //Movement stuffs
    public float patrolSpeed;
    float chaseSpeed;
    Vector3 targetPosition; //The place the golem wants to move to eventually
    bool patrolling;

    //Attacking variables
    float timeToNextShockwave = 0f;
    public float knockbackStrength;
    public GameObject shockwaveParticles;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        PatrolArea();
    }

    void PatrolArea()
    {
        if (golemState == GolemStates.FindingNewPosition)
        {
            //Initialises finding position
            FindNewTargetPosition();
        }

        if (golemState == GolemStates.Patrolling)
        {
            //Get direction to target position
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            //Set rotation
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.75f * Time.deltaTime);

            //Move Golem towards position
            //rb.velocity = direction * patrolSpeed * Time.deltaTime;
            rb.velocity = transform.forward * patrolSpeed * Time.deltaTime;

            //Check if Position is reached
            float distance = (targetPosition - transform.position).sqrMagnitude;
            if(distance < 2f)
            {
                //Nearby target position
                FindNewTargetPosition();
            }
            //Visualising position
            Debug.DrawRay(targetPosition, Vector3.up * 100f, Color.red);

            //Stomping behaviour
            timeToNextShockwave += Time.deltaTime;
            if(timeToNextShockwave >= 0.666f)
            {
                timeToNextShockwave = 0;
                Shockwave();
            }
        }
    }

    void FindNewTargetPosition()
    {
        //Finds a position to go to that is at 5 units away (Radius)
        float angle = Random.Range(-360f, 360f);
        float newXPos = transform.position.x + (15 * Mathf.Cos(angle));
        float newZPos = transform.position.z + (15 * Mathf.Sin(angle));
        Vector3 newPosition = new Vector3(newXPos, transform.position.y, newZPos);

        //Raycast downwards to make sure the new Position is not over a hole
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(newXPos, 10, newZPos), Vector3.down, out hit, Mathf.Infinity))
        {
            print("Rock Golem is going to: " + hit.collider.gameObject.name);
            if(hit.collider.tag != "Hole")
            {   
                targetPosition = newPosition;
                golemState = GolemStates.Patrolling;
            }
        }
    }

    void Shockwave()
    {
        AutoDestroyOverTime particles = Instantiate(shockwaveParticles, transform.position, shockwaveParticles.transform.rotation).GetComponent<AutoDestroyOverTime>();
        particles.DestroyWithTime(0.3f);

        //Gets all players in range of shockwave stomp
        int ignoreLayerMask =~ 1 << LayerMask.NameToLayer("Ground");    //Raycasts on everything but ground
        Collider[] inRange = Physics.OverlapSphere(transform.position, 8f, ignoreLayerMask);

        //Disrupts all players in range
        foreach(Collider collider in inRange)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player)
            {
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);
            }
        }        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Knocks player back
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(knockbackStrength * 2 * knockbackDirection);
        }
    }
}
