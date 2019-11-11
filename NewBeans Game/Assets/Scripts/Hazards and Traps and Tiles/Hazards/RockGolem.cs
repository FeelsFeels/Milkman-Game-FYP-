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
        FindingPlayerToChase
    }

    GolemStates golemState = GolemStates.Idle;

    Rigidbody rb;
    Animator animator;

    //Movement stuffs
    float timeIdling;
    float timeFindingPlayer;
    float timeSpentChasing;
    public float patrolSpeed;
    public float chaseSpeed;
    Vector3 targetPosition; //The place the golem wants to move to eventually
    bool patrolling;
    PlayerController playerToChase;

    //Attacking variables
    float timeToNextShockwave = 0f;
    public float knockbackStrength;
    public float knockbackRadius;

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
        GolemBehaviour();
    }

    public void Initialise()
    {
        showInitialiseParticles = true;
        rb = GetComponent<Rigidbody>(); //Needed as method is called during instantiation of golem

        rb.velocity = Vector3.down * 100;

        timeIdling = 0;
        golemState = GolemStates.Idle;

    }

    void GolemBehaviour()
    {
        ////////****////////////
        ///Prepare4Death PT 2///
        ////////****////////////

        if (golemState == GolemStates.Idle)
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
            Vector3 movement = transform.forward * patrolSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            //Check if Position is reached
            float distance = (targetPosition - transform.position).sqrMagnitude;
            if(distance < 2f)
            {
                //Nearby target position
                FindNewTargetPosition();
                golemState = GolemStates.Idle;
            }
            //Visualising position
            Debug.DrawRay(targetPosition, Vector3.up * 100f, Color.red);

            ////Stomping behaviour
            //timeToNextShockwave += Time.deltaTime;
            //if(timeToNextShockwave >= 0.666f)
            //{
            //    timeToNextShockwave = 0;
            //    Shockwave();
            //}
        }

        if (golemState == GolemStates.FindingPlayerToChase)
        {
            timeFindingPlayer += Time.deltaTime;
            Vector3 facePos = new Vector3(playerToChase.transform.position.x, transform.position.y, playerToChase.transform.position.z);
            Vector3 direction = facePos - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);

            if(timeFindingPlayer >= 2.5f)
            {
                golemState = GolemStates.Chasing;
                timeFindingPlayer = 0;
            }
        }

        if(golemState == GolemStates.Chasing)
        {
            //Get direction to target position
            Vector3 direction = playerToChase.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            //Set rotation
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 4.0f * Time.deltaTime);

            //Move Golem towards position
            Vector3 movement = transform.forward * chaseSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            //Stomping behaviour
            timeToNextShockwave += Time.deltaTime;
            if (timeToNextShockwave >= 0.666f)
            {
                timeToNextShockwave = 0;
                Shockwave();
            }

            timeSpentChasing += Time.deltaTime;
            if (playerToChase.isDead || timeSpentChasing > 10f)
            {
                timeSpentChasing = 0; 
                golemState = GolemStates.Idle;
            }
        }

        //Setting golem's animation state
        if (golemState == GolemStates.Patrolling || golemState == GolemStates.Chasing)
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
        Collider[] inRange = Physics.OverlapSphere(transform.position, knockbackRadius, ignoreLayerMask);

        //Disrupts all players in range
        foreach(Collider collider in inRange)
        {
            print(collider.name);
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player)
            {
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody>().AddForce(knockbackStrength * knockbackDirection);

                player.Hit();
            }
            else if(collider.tag == "Rock")
            {
                print("neeba");
                Vector3 knockbackDirection = (collider.transform.position - transform.position).normalized;
                collider.GetComponent<HazardBoulder>().rb.AddForce(knockbackStrength * knockbackDirection);
            }
        }
    }
    
    void ChangeState(GolemStates newState)
    {
        switch (newState)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (golemState == GolemStates.FindingPlayerToChase || golemState == GolemStates.Chasing)
                return;

            playerToChase = collision.gameObject.GetComponent<PlayerController>();
            golemState = GolemStates.FindingPlayerToChase;
            timeFindingPlayer = 0;

        }
        else if(collision.gameObject.tag == "PushProjectile")
        {
            if (golemState == GolemStates.FindingPlayerToChase || golemState == GolemStates.Chasing)
                return;

            animator.SetTrigger("Hit");
            playerToChase = collision.gameObject.GetComponent<PushProjectile>().ownerPlayer.GetComponent<PlayerController>();
            golemState = GolemStates.FindingPlayerToChase;
        }

        if (showInitialiseParticles)
        {
            //Spawn crashing particles thrice
            for (int i = 0; i < 50; i++)
            {
                AutoDestroyOverTime newParticles = Instantiate(crashingParticles, new Vector3(transform.position.x + Random.Range(-5f, 5f), 0, transform.position.z + Random.Range(-5f, 5f)), Quaternion.identity).GetComponent<AutoDestroyOverTime>();
                newParticles.DestroyWithTime(2);
            }
            showInitialiseParticles = false;
            Shockwave();
            StartCoroutine(SetVelocityZeroNextFrame());
        }
    }

    IEnumerator SetVelocityZeroNextFrame()
    {
        yield return null;
        rb.velocity = Vector3.zero;
    }
}
