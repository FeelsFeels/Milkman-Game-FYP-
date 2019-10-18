using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolem2 : MonoBehaviour
{

    enum GolemStates
    {
        Idling,
        Inactive,
        Activating,
        Chasing,
        Deactivating
    }

    GolemStates golemState = GolemStates.Inactive;

    Rigidbody rb;
    Animator animator;


    // //State timers // //
    float timeSpentChasing;

    float timeToActivate = 3.0f;
    float timeSpentActivating;

    float timeToIdle;
    float timeSpentIdling;

    float timeToDeactivate = 3.0f;
    float timeSpentDeactivating;
    // // // // // // // //

    //Movement variables
    public float chaseSpeed;
    Vector3 targetPosition; //The place the golem wants to move to eventually
    PlayerController playerToChase;

    //Attacking variables
    float timeToNextShockwave = 0f;
    public float knockbackStrength;

    //Scaling during states
    public Vector3 unactivatedLocalscale;
    public Vector3 activatedLocalscale;

    //Particle Effects
    public GameObject shockwaveParticles;

    //Range Representation
    public GameObject shockwaveRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        shockwaveRange.SetActive(false);
        transform.localScale = unactivatedLocalscale;
    }

    // Update is called once per frame
    private void Update()
    {
        GolemBehaviour();
    }

    void GolemBehaviour()
    {
        ////////****////////////
        ///Prepare4Death PT 2///
        ////////****////////////
        if(golemState == GolemStates.Activating)
        {
            timeSpentActivating += Time.deltaTime;

            float lerpStep = timeSpentActivating / timeToActivate;

            transform.localScale = Vector3.Lerp(unactivatedLocalscale, activatedLocalscale, lerpStep);
            
            if(timeSpentActivating >= timeToActivate)
            {
                shockwaveRange.SetActive(true);
                golemState = GolemStates.Chasing;
                timeSpentActivating = 0;
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
                golemState = GolemStates.Idling;
            }
        }

        if(golemState == GolemStates.Idling)
        {
            timeSpentIdling += Time.deltaTime;
            if(timeSpentIdling >= timeToIdle)
            {
                timeSpentIdling = 0;
                golemState = GolemStates.Deactivating;
                shockwaveRange.SetActive(false);
                //animator.SetTrigger("Deactivate");
            }
        }

        if(golemState == GolemStates.Deactivating)
        {
            timeSpentDeactivating += Time.deltaTime;

            float lerpStep = timeSpentDeactivating / timeToDeactivate;

            transform.localScale = Vector3.Lerp(activatedLocalscale, unactivatedLocalscale, lerpStep);

            if (timeSpentDeactivating >= timeToDeactivate)
            {
                golemState = GolemStates.Inactive;
                timeSpentDeactivating = 0;
            }
        }


        //Setting golem's animation state
        if (golemState == GolemStates.Chasing)
        {
            animator.SetFloat("Speed", 1);
        }
        else if (golemState == GolemStates.Idling || golemState == GolemStates.Deactivating || golemState == GolemStates.Inactive)
        {
            animator.SetFloat("Speed", 0);
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


    private void OnCollisionEnter(Collision collision)
    {
        //Check for collisions that would activate the golem
        if (golemState == GolemStates.Inactive || golemState == GolemStates.Idling)
        {
            if (collision.gameObject.tag == "Player")
            { 
                golemState = GolemStates.Activating;
                animator.SetTrigger("Activate");
                playerToChase = collision.gameObject.GetComponent<PlayerController>();
            }
            else if (collision.gameObject.tag == "PushProjectile")
            {
                golemState = GolemStates.Activating;
                animator.SetTrigger("Activate");
                playerToChase = collision.gameObject.GetComponent<PushProjectile>().ownerPlayer.GetComponent<PlayerController>();
            }
        }
    }
}
