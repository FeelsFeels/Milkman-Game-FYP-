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

    GolemStates golemState = GolemStates.Idle;

    Rigidbody rb;
    Animator animator;

    //Movement stuffs
    float timeIdling;
    public float patrolSpeed;
    float chaseSpeed;
    Vector3 targetPosition; //The place the golem wants to move to eventually
    bool patrolling;

    //Attacking variables
    float timeToNextShockwave = 0f;
    public float knockbackStrength;

    //Visuals
    bool showInitialiseParticles;
    public GameObject shockwaveParticles;
    public GameObject crashingParticles;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        PatrolArea();
    }

    public void Initialise()
    {
        showInitialiseParticles = true;
        rb = GetComponent<Rigidbody>(); //Needed as method is called during instantiation of golem

        rb.velocity = Vector3.down * 100;

        timeIdling = 0;
        golemState = GolemStates.Idle;

    }

    void PatrolArea()
    {
        if(golemState == GolemStates.Idle)
        {
            timeIdling += Time.deltaTime;
            if(timeIdling > 5f)
            {
                timeIdling = 0;
                golemState = GolemStates.FindingNewPosition;
            }
        }

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
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);

            //Move Golem towards position
            //rb.velocity = direction * patrolSpeed * Time.deltaTime;
            //rb.velocity = transform.forward * patrolSpeed * Time.deltaTime;
            Vector3 movement = transform.forward * patrolSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
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

        //Setting golem's animation state
        if (golemState == GolemStates.Patrolling)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
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

                player.Hit();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (showInitialiseParticles)
        {
            //Spawn crashing particles thrice
            for (int i = 0; i < 50; i++)
            {
                AutoDestroyOverTime newParticles = Instantiate(crashingParticles, new Vector3(transform.position.x + Random.Range(-5f, 5f), 0, transform.position.z + Random.Range(-5f, 5f)), Quaternion.identity).GetComponent<AutoDestroyOverTime>();
                newParticles.DestroyWithTime(2);
                showInitialiseParticles = false;
            }
        }
    }
}
