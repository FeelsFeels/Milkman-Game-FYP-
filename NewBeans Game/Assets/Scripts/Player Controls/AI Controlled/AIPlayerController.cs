using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AIPlayerController : MonoBehaviour
    {
        public int playerNumber;

        [Header("Player Score")]
        public int killCount;
        public int deathCount;
        public int currentScore;

        [Header("Player Status Effects")]
        public bool playerStunned;  //Are you stunned
        public float stunnedTime;   //Time passed while being stunned
        public float stunDuration = 0.25f; //Time to spend stunned
        public bool shootingHook;   //Cannot move while shooting hook
        public bool respawnAudioPlaying;

        [Header("Visual Effects")]
        public GameObject playerDieEffect;
        public GameObject playerPushedEffect;
        public GameObject playerPulledEffect;

        //Player Input 
        [Header("Player Input and Initialisation")]
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
        public float moveRate = 5f;  // units moved per second holding down move input

        [Header("Player Death Variables")]
        public bool isDead = false;
        public bool shouldRespawn = true;
        public float waitToRespawn = 3f;
        public Transform stageCenterPos;
        public Transform respawnPosition;
        public float respawnDelay;
        public PlayerController lastHitBy;

        [Header("Object Components and References")]
        // Object's Components
        public AIShoot playerShoot;
        public SkillSetManager playerSkillSet;
        public Animator animator;
        CapsuleCollider capsuleCollider;
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public Rigidbody rb;
        Shield invincibilityShield;
        public GameObject dizzyStars;
        public GameObject defaultCharacterModel;
        public GameObject cameraRigObj;
        float cameraRigRot = 0f;


        public bool rightAnalogTargeting = false;

        //DeathEvent
        //[HideInInspector]

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
                //    playerNumber = inputInfo.playerNumber;

                //    HorizontalInputAxis = inputInfo.HorizontalInputAxis;
                //    VerticalInputAxis = inputInfo.VerticalInputAxis;
                //    AButtonInput = inputInfo.AButtonInput;
                //    BButtonInput = inputInfo.BButtonInput;

                //    if (inputInfo.RightHorizontalAxis != null)
                //        RightHorizontalAxis = inputInfo.RightHorizontalAxis;
                //    if (inputInfo.RightVerticalAxis != null)
                //        RightVerticalAxis = inputInfo.RightVerticalAxis;
                //    if (inputInfo.RightBumper != null)
                //        RightBumper = inputInfo.RightBumper;

                //Set what character this player is playing
                if (inputInfo.chosenCharacterData != null)
                {
                    playerSkillSet = GetComponentInChildren<SkillSetManager>();
                    playerSkillSet.SetCharacter(inputInfo.chosenCharacterData.character);
                    playerSkillSet.SetInputs(AButtonInput, BButtonInput);

                    if (inputInfo.chosenCharacterData.characterUltiReadyVFX != null)
                    {
                        GameObject newVFX = Instantiate(inputInfo.chosenCharacterData.characterUltiReadyVFX, transform);
                        newVFX.SetActive(false);
                        playerSkillSet.skillReadyParticleFX = newVFX;
                    }
                    if (inputInfo.chosenCharacterData.characterPrefab != null)
                    {
                        GameObject newModel = Instantiate(inputInfo.chosenCharacterData.characterPrefab, transform.Find("Character Model"));
                        animator = newModel.GetComponent<Animator>();
                        defaultCharacterModel.SetActive(false);
                    }
                }

                //No player assigned to character
                if (inputInfo.chosenCharacterData == null && inputInfo.forceActive == false)
                {
                    gameObject.SetActive(false);
                }
            }
            // Components
            if (animator == null)
                animator = transform.Find("Character Model").GetComponentInChildren<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            rb = GetComponent<Rigidbody>();
            invincibilityShield = GetComponentInChildren<Shield>();
        }


        // Start is called before the first frame update
        void Start()
        {
            //Set the camera rig rotation at the start. This will be the 'correction angle'
            cameraRigObj = transform.parent.GetComponentInChildren<Camera>().gameObject;
            cameraRigRot = cameraRigObj.transform.rotation.eulerAngles.y;
        }


        private void Update()
        {
            //Stunned timing
            if (playerStunned)
            {
                stunnedTime += Time.deltaTime;

                if (stunnedTime >= stunDuration)
                {
                    //animator.ResetTrigger("Hit");
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
        }


        /// ***********
        /// Moving character methods
        /// ***********
        public void Move(Vector3 direction)
        {
            if (playerStunned)
                return;

            //If Not moving, can set the animator values
            if (direction == Vector3.zero)
            {
                if (animator != null)
                    animator.SetFloat("Speed", 0);
            }
            else if (animator != null)  //If Moving, can set the animator values
            {
                animator.SetFloat("Speed", 1);
            }
            // Movement based on camera's rotation at the start.
            Vector3 velocity = Quaternion.AngleAxis(cameraRigRot, Vector3.up) * (direction * moveRate); //Multiply the direction by the camera rotation
            Vector3 movement = velocity * Time.deltaTime;
            transform.Translate(movement, Space.World);

            //Rotate player if NOT DEAD
            if (!isDead)
            {
                if (!rightAnalogTargeting)
                { //If not using right analog for turning
                    Quaternion correctedQ;
                    correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * direction); //Correct the rotation quaternion 
                    transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
                }
            }
        }


        public void Turn(Vector3 dir)
        {
            if (!isDead && !playerStunned) // If player is still alive or not stunned,
            {
                Quaternion correctedQ;
                correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * dir); //Correct the rotation quaternion 
                transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
            }

        }


        public void Hit()
        {
            //animator.SetTrigger("Hit");
            playerStunned = true;
            stunnedTime = 0;
            stunDuration = 0.25f;
            playerShoot.ForceStopChargingVFX();
            dizzyStars.SetActive(true);
        }

        public void Hit(float timeToStun)
        {
            //animator.SetTrigger("Hit");
            playerStunned = true;
            stunnedTime = 0;
            stunDuration = timeToStun;
            dizzyStars.SetActive(true);
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
                this.transform.Find("Respawn_Canvas").gameObject.SetActive(true);
                //this.transform.Find("Projector").gameObject.SetActive(true);
            }

            if (isDead == false)
            {
                skinnedMeshRenderer.enabled = true;
                capsuleCollider.enabled = true;
                invincibilityShield.ActivateShield();
                this.transform.Find("Canvas").gameObject.SetActive(true);
                this.transform.Find("Respawn_Canvas").gameObject.SetActive(false);
                //this.transform.Find("Projector").gameObject.SetActive(false);
            }

        }


        IEnumerator WaitToRespawn()
        {
            CancelInvoke("PlayRespawningSound");
            InvokeRepeating("PlayRespawningSound", 0f, 1f);
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.position = stageCenterPos.position;
            transform.rotation = Quaternion.Euler(Vector3.zero); // Reset rotation.

            //Controlling where to land
            float timePassed = 0f;
            GameObject targetter = transform.Find("Respawn_Canvas").gameObject;
            while (timePassed < waitToRespawn)
            {
                timePassed += Time.deltaTime;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 11f))
                {
                    targetter.transform.position = new Vector3(transform.position.x, hit.point.y + 0.2f, transform.position.z);
                }
                else
                {
                    targetter.transform.localPosition = new Vector3(0f, -7f, 0f);
                }
                yield return null;
            }
            CancelInvoke("PlayRespawningSound");
            StartCoroutine(RespawnPlayer());

        }

        // For looping the respawning audio clip.
        public void PlayRespawningSound()
        {
            if (isDead)
            {
                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("Respawning");
                }
            }
        }

        IEnumerator RespawnPlayer()
        {
            //print("respawning player");
            if (isDead == true)
            {

                yield return new WaitForSeconds(respawnDelay);
                //gameObject.transform.position = respawnPosition.transform.position;
                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("Respawned");
                }
                isDead = false;
                HidePlayerWhenDead();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(Vector3.down * 1500f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            PushProjectile pushProjectile = collision.collider.GetComponent<PushProjectile>();
            if (pushProjectile && pushProjectile.ownerPlayer != gameObject)
            {
                pushProjectile.PushARigidbody(rb);
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
}