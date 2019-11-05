using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    
    [Header("Player Score")]
    public int killCount;
    public int deathCount;
    public int currentScore;

    [Header("Player Status Effects")]
    public bool playerStunned;  //Are you stunned
    public float stunnedTime;   //Time passed while being stunned
    private float stunDuration = 0.25f; //Time to spend stunned
    public bool shootingHook;   //Cannot move while shooting hook

    [Header("Visual Effects")]
    public GameObject playerDieEffect;
    public GameObject playerPushedEffect;
    public GameObject playerPulledEffect;

    //Player Input 
    [Header ("Player Input and Initialisation")]
    public PlayerInputInfo inputInfo;
    public int ControllerNumber;
    public GameObject playerModel;
    [HideInInspector] public string HorizontalInputAxis;
    [HideInInspector] public string VerticalInputAxis;
    [HideInInspector] public string AButtonInput;
    [HideInInspector] public string BButtonInput;
    [HideInInspector] public string RightHorizontalAxis;
    [HideInInspector] public string RightVerticalAxis;
    [HideInInspector] public string RightBumper;

    [Header("Player Movement")]
    public float playerTurnSmoothing = 10f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float moveRate = 10;  // units moved per second holding down move input

    [Header("Player Death Variables")]
    public bool isDead = false;
    public bool shouldRespawn = true;
    public float waitToRespawn = 3f;
    public Transform stageCenterPos;
    public Transform respawnPosition;
    public float respawnDelay;
    public GameObject lastHitBy;

    [Header("Object Components and References")]
    // Object's Components
    public Animator animator;
    CapsuleCollider capsuleCollider;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Rigidbody rb;
    Shield invincibilityShield;
    public GameObject dizzyStars;

    public GameObject cameraRigObj;
    float cameraRigRot =0f;


    public bool rightAnalogTargeting = false;

    //DeathEvent
    //[HideInInspector]
    public UnityEvent<PlayerController> OnPlayerDeath = new PlayerDeathEvent();

    void Reset()
    {
        //playController = GetComponent<CharacterController>();
        
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }


    private void Awake()
    {
        if (inputInfo != null)
        {
            playerNumber = inputInfo.playerNumber;

            HorizontalInputAxis = inputInfo.HorizontalInputAxis;
            VerticalInputAxis = inputInfo.VerticalInputAxis;
            AButtonInput = inputInfo.AButtonInput;
            BButtonInput = inputInfo.BButtonInput;

            if (inputInfo.RightHorizontalAxis != null)
            RightHorizontalAxis = inputInfo.RightHorizontalAxis;
            if (inputInfo.RightVerticalAxis != null)
                RightVerticalAxis = inputInfo.RightVerticalAxis;
            if (inputInfo.RightBumper != null)
                RightBumper = inputInfo.RightBumper;

            //No player assigned to character
            //if (inputInfo.chosenCharacterData == null)
            //    gameObject.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Components
        animator = transform.Find("Character Model").GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
        invincibilityShield = GetComponentInChildren<Shield>();
        cameraRigObj = FindObjectOfType<CameraControls>().gameObject;

        //Set the camera rig rotation at the start. This will be the 'correction angle'
        if (cameraRigObj != null)
            cameraRigRot = cameraRigObj.transform.rotation.eulerAngles.y;
    }

    //Deprecated 
    //public void SetControllerNumber (int controllerNo)
    //{
    //    ControllerNumber = controllerNo; //get the controller number that will control this player (to which this script is attached to)
    //    HorizontalInputAxis = "Horizontal (Controller " + controllerNo + ")";
    //    VerticalInputAxis = "Vertical (Controller " + controllerNo + ")";
    //    AButtonInput = "AButton (Controller " + controllerNo + ")";
    //    BButtonInput = "BButton (Controller " + controllerNo + ")";
    //}

    private void Update()
    {
        //Stunned timing
        if (playerStunned)
        {
            stunnedTime += Time.deltaTime;
            dizzyStars.SetActive(true);

            if (stunnedTime >= stunDuration)
            {
                animator.ResetTrigger("Hit");
                playerStunned = false;
                stunnedTime = 0;
                dizzyStars.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        rb.AddForce(Physics.gravity * 2);

        /// ***********
        /// Move controls
        /// ***********

        float moveVerticalAxis = Input.GetAxisRaw(VerticalInputAxis);
        float moveHorizontalAxis = Input.GetAxisRaw(HorizontalInputAxis);
        Vector3 input = new Vector3(moveHorizontalAxis, 0, -moveVerticalAxis);
        Vector3 direction = input.normalized;

        if (moveHorizontalAxis != 0 || moveVerticalAxis != 0)
        {
            Move(direction);
        }

        /// Right analog
        float aimVerticalAxis = Input.GetAxisRaw(RightVerticalAxis);
        float aimHorizontalAxis = Input.GetAxisRaw(RightHorizontalAxis);
        Vector3 rightAxisInput = new Vector3(aimHorizontalAxis, 0, -aimVerticalAxis);
        Vector3 dir = rightAxisInput.normalized;

        if (aimHorizontalAxis != 0 || aimVerticalAxis != 0)
        {
            rightAnalogTargeting = true;
            Turn(dir);
        }
        else { rightAnalogTargeting = false; }


        OrientPlayerWithGround();

        //If Not moving, can set the animator values
        if (Input.GetAxis(HorizontalInputAxis) == 0 && Input.GetAxis(VerticalInputAxis) == 0)
        {
            if (animator != null)
            animator.SetFloat("Speed", 0);
        }
    }


    /// ***********
    /// Moving character methods
    /// ***********
    private void Move(Vector3 direction)
    {
        if (playerStunned || shootingHook)
            return;

        // Movement based on camera's rotation at the start.
        Vector3 velocity = Quaternion.AngleAxis(cameraRigRot , Vector3.up) * (direction * moveRate); //Multiply the direction by the camera rotation
        Vector3 movement = velocity * Time.deltaTime;
        transform.Translate(movement, Space.World);

        //Rotate player if NOT DEAD
        if (!isDead) {
            if (!rightAnalogTargeting) { //If not using right analog for turning
            Quaternion correctedQ;
            correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * direction); //Correct the rotation quaternion 
            transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
            }
        }

        //If Moving, can set the animator values
        if (animator != null)
        animator.SetFloat("Speed", 1);

    }


    void Turn (Vector3 dir)
    {
        if (!isDead) // If player is still alive,
        {
            Quaternion correctedQ;
            correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * dir); //Correct the rotation quaternion 
            transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
        }
        
    }


    public void Hit()
    {
        animator.SetTrigger("Hit");
        playerStunned = true;
        stunnedTime = 0;
        stunDuration = 0.25f;
    }

    public void Hit(float timeToStun)
    {
        animator.SetTrigger("Hit");
        playerStunned = true;
        stunnedTime = 0;
        stunDuration = timeToStun;
    }

    /// *********************************
    /// Player Die
    /// *********************************

    // This hides the player when dead, and makes it reappear when alive.
    public void HidePlayerWhenDead()
    {
        if (isDead == true)
        {
            skinnedMeshRenderer.enabled = false;
            capsuleCollider.enabled = false;
            this.transform.Find("Canvas").gameObject.SetActive(false);
            this.transform.Find("Projector").gameObject.SetActive(true);
        }

        if (isDead == false)
        {
            skinnedMeshRenderer.enabled = true;
            capsuleCollider.enabled = true;
            invincibilityShield.ActivateShield();
            this.transform.Find("Canvas").gameObject.SetActive(true);
            this.transform.Find("Projector").gameObject.SetActive(false);
        }

    }

    public void Die()
    {
        isDead = true;
        playerStunned = false;
        dizzyStars.SetActive(false);
        Instantiate(playerDieEffect, gameObject.transform.position, gameObject.transform.rotation);

        if (lastHitBy != null)
        {
            GameManager.instance.OnPlayerDeath(this, lastHitBy.GetComponent<PlayerController>());
        }
        else
        {
            GameManager.instance.OnPlayerDeath(this, null);
        }

        lastHitBy = null;

        // Makes player disappear
        HidePlayerWhenDead();

        // Respawns player
        if (shouldRespawn)
            StartCoroutine(WaitToRespawn());

        OnPlayerDeath.Invoke(this);            
    }

    IEnumerator WaitToRespawn()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = stageCenterPos.position;
        transform.rotation = Quaternion.Euler(Vector3.zero); // Reset rotation.
        yield return new WaitForSeconds(waitToRespawn);
        StartCoroutine(RespawnPlayer());

    }

    IEnumerator RespawnPlayer()
    {
        if (isDead == true)
        {
            
            yield return new WaitForSeconds(respawnDelay);
            //gameObject.transform.position = respawnPosition.transform.position;
            
            isDead = false;
            HidePlayerWhenDead();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collide with "Hole", player dies.
        if (other.tag == "Hole")
        {
            Die();
        }
        if(other.GetComponent<IAffectedByWeight>() != null)
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


    /// <summary>
    /// Orienting player to ground normals
    /// </summary>

    public float dist = 1.0f;
    public LayerMask hitLayer;
    void OrientPlayerWithGround()
    {
        Vector3 pos = transform.position + transform.TransformDirection(Vector3.forward) * 0.4f + transform.TransformDirection(Vector3.up) * 0.2f;
        Vector3 dir = transform.TransformDirection(Vector3.down);
        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * dist, Color.red);

        if (Physics.Raycast(ray, out hit, dist, hitLayer))
        {
            //if (hit.collider.tag == "Ground")
            //{
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);

                Quaternion targetQuaternion;
                targetQuaternion = Quaternion.FromToRotation(transform.up, hit.normal);

                Quaternion correctedtarget;
                correctedtarget = targetQuaternion;
                correctedtarget.y = 0;
                correctedtarget = correctedtarget.normalized;
                correctedtarget = correctedtarget * transform.rotation;

                // update rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, correctedtarget, Time.deltaTime);
            //}
        }
    }

    public bool IsPlaying()
    {
        if (inputInfo.forceActive)
            return true;

        if (inputInfo.chosenCharacterData == null)
            return false;
        else
            return true;
    }
}