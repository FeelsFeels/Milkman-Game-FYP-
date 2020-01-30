using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;
    public Transform playerCenter; //Needed as reference for grappling hook's linerenderer;

    public GameObject pushProjectile;
    public GameObject hookProjectile;

    private Animator animator;  //For blinking indication
    public Animator playerAnim; //player animation

    public GameObject aimingArrows;
    public Image chargingIndication;

    private GrapplingHook hProjectile;

    private PlayerController playerScript;
    private ChargingPushVFXController chargingVFXScript;


    //States and timing
    private float pushCooldownTimer;
    public float pushCooldown;
    public float pushChargedTime;
    private float pushChargedMaxTime = 1.5f;
    private float pullCooldownTimer;
    public float pullCooldown = 5f;

    public bool chargingPushProjectile;
    public bool chargingGrapplingHook;
    public bool playerCannotShoot;

    public float baseKickbackForce;
    public float explodeRadius = 10f;
    public LayerMask playerLayer;
    public float explodeSpreadAngle = 90;
    public float explodeForce = 2500;
    float timeCharged;
    public ParticleSystem shotgunParticles;
    public GameObject perfectChargeParticles;

    ///If we want to use the Input Manager
    public bool usingRightBumper = true;
    public string watergunInput;
    public string chargingInput;
    public string hookInput;
    /// //////////////////////////////////


    private void Start()
    {
        //shootOrigin = transform.Find("ShootOrigin");
        pushCooldownTimer = 0;

        playerScript = GetComponent<PlayerController>();
        chargingVFXScript = GetComponentInChildren<ChargingPushVFXController>();
        animator = transform.Find("Canvas").GetComponent<Animator>();
        playerAnim = transform.Find("Character Model").GetComponentInChildren<Animator>();

        // hProjectile.GetComponent<Shoot>().castedByPlayer = player;
        watergunInput = playerScript.AButtonInput;
        chargingInput = playerScript.RightBumper;
        hookInput = playerScript.BButtonInput;
        if (playerScript.inputInfo.chosenCharacterData.pushProjectile != null)
        {
            pushProjectile = playerScript.inputInfo.chosenCharacterData.pushProjectile;
        }
        if (playerScript.inputInfo.chosenCharacterData.hookProjectile != null)
        {
            hookProjectile = playerScript.inputInfo.chosenCharacterData.hookProjectile;
        }


        aimingArrows.SetActive(false);
    }

    private void Update()
    {
        pushCooldownTimer -= Time.deltaTime;
        pullCooldownTimer -= Time.deltaTime;

        // --- If player is stunned, stop the charging.
        if (playerScript.playerStunned)
        {
            PlayerResetCharge();
            return;
        }

        if (playerScript.playerStunned || playerScript.isDead || GetComponent<SkillSetManager>().ultiIsActivated) //Check if player is stunned or dead. If dead/stunned, do not shoot
        {
            return;
        }


        ////Shoot Watergun
        //if (Input.GetButtonDown(watergunInput) && waterGunCooldownTimer <= 0)
        //{
        //    ShootPushProjectile();
        //    pushCooldownTimer = pushCooldown;
        //}

        if (chargingPushProjectile)
        {
            //Player animator
            playerAnim.SetBool("Charging", true);


            pushChargedTime += Time.deltaTime;
            chargingIndication.gameObject.SetActive(true);
            //chargingIndication.fillAmount = pushChargedTime / 1.5f;

            //Calculate charge progress based on curve
            timeCharged = Mathf.Min(pushChargedTime, 1.2f); //Clamp to max of 1.2f
            float chargedPercent = KnockbackMultiplier(timeCharged) / KnockbackMultiplier(1.2f);
            chargingIndication.fillAmount = chargedPercent;

            if(timeCharged >= 1.2f)
            {
                animator.SetBool("activateChargeBlink", true);
            }
            else
            {
                animator.SetBool("activateChargeBlink", false);
            }

            if (pushChargedTime >= pushChargedMaxTime)
            {
                //PushBackfire();
                //StartCoroutine(ShotgunFireLight());
                animator.SetBool("activateChargeBlink", false);
                ShotgunAttack();
                Instantiate(shotgunParticles, transform.position, transform.rotation, transform);
                chargingVFXScript.StopVFX();
                //shotgunParticles.shape.angle = explodeSpreadAngle / 2;
            }
        }

        //Charge Push Projectile
        if (Input.GetButtonDown(watergunInput) && pushCooldownTimer <= 0)
        {
            ChargePushProjectile();
            chargingVFXScript.StartVFX();
        }
        
        if (Input.GetButtonUp(watergunInput) && chargingPushProjectile)
        {
            //Player anim
            playerAnim.SetBool("Charging", false);
            playerAnim.SetTrigger("Push");

            ShootPushProjectile();
        }

        //Charge Grappling Hook
        if (Input.GetButtonDown(hookInput) && pullCooldownTimer <= 0)
        {
            if (hProjectile == null)
            {
                ChargeHook();
            }
        }

        //Shoot grappling hook
        if (Input.GetButtonUp(hookInput) && chargingGrapplingHook)
        {
            ShootHook();

            //Player anim
            playerAnim.SetTrigger("Pull");
        }

        if (hProjectile != null)
            playerScript.shootingHook = true;
        else
            playerScript.shootingHook = false;
    }

    public void PlayerResetCharge()
    {
        aimingArrows.SetActive(false);
        pushChargedTime = 0;
        chargingPushProjectile = false;
        chargingIndication.fillAmount = 0;
        animator.SetBool("activateChargeBlink", false);

        //Player anim
        playerAnim.SetBool("Charging", false);
        playerAnim.SetTrigger("ResetCharge");
    }

    private void ChargePushProjectile()
    {
        if (playerCannotShoot) return;
        if (chargingGrapplingHook) return;


        aimingArrows.SetActive(true);
        chargingPushProjectile = true;

    }

    private void ShootPushProjectile()
    {
        if (playerCannotShoot) return;

        chargingVFXScript.StopVFX();
        chargingVFXScript.PlayShootSound();

        PushProjectile projectile = Instantiate(pushProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z)
                                                                            , Quaternion.identity).GetComponent<PushProjectile>();

        //print("Time charged: " + timeCharged + "\n" + "Multiplier: " + KnockbackMultiplier(timeCharged));
        projectile.ShotInitialised(KnockbackMultiplier(timeCharged), SmallMultiplier(timeCharged), shootOrigin.forward, gameObject);

        //Scale projectile size and rotation
        float scaling = SmallMultiplier(pushChargedTime / 2) / 2;
        projectile.gameObject.transform.localScale = new Vector3(scaling, scaling, scaling);
        projectile.transform.forward = transform.forward;

        //kickback to player
        Vector3 direction = -transform.forward;
        float percentage = 1 + (pushChargedTime / pushChargedMaxTime) * 3;
        playerScript.rb.AddForce(direction * (baseKickbackForce * percentage));

        //Cool Visuals
        if(timeCharged >= 1.2f)
        {
            //Instantiate(perfectChargeParticles, playerScript.transform);
        }

        //Reset states
        PlayerResetCharge();
        pushCooldownTimer = pushCooldown;
    }

    private void PushBackfire()
    {
        Vector3 direction = -transform.forward;
        playerScript.rb.AddForce(1800 * direction);
        playerScript.Hit(3.0f);

        //Reset states
        aimingArrows.SetActive(false);
        chargingPushProjectile = false;
        pushChargedTime = 0;
        pushCooldownTimer = pushCooldown;
        chargingIndication.fillAmount = 0;
    }

    void ShotgunAttack()
    {
        StartCoroutine(ShotgunFireLight());

        animator.SetBool("backToNull", true); // Stops the shoot charging animation

        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(Physics.OverlapSphere(transform.position, explodeRadius, playerLayer)); //Find all players in range
        Debug.Log("Shotgunning");

        if (colliders.Contains(this.GetComponent<Collider>())) //If it includes this player,
        {
            colliders.Remove(this.GetComponent<Collider>()); // remove
            //Debug.Log("Remove self");
        }

        Debug.Log(colliders.Count);

        foreach (Collider player in colliders)
        {
            //Debug.Log("hi");
            Transform other = player.transform;
            Vector3 dir = (other.position - this.transform.position).normalized; //Find the direction from origin to other player
            float angle = 90 - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg; //Find the angle of the direction
            if (Mathf.Abs(Mathf.DeltaAngle(this.transform.eulerAngles.y, angle)) < explodeSpreadAngle) //If the angle difference between this object's forward and the direction is < spread angle,
            {
                //push the other player back
                Rigidbody rb = other.GetComponent<Rigidbody>();
                rb.AddForce(explodeForce * dir);
            }
        }

        PushBackfire();

    }
    //Shotgun indication
    IEnumerator ShotgunFireLight()
    {
        FindObjectOfType<AudioManager>().Play("Shotgun");
        if (this.gameObject.transform.Find("Spotlight") != null)
        {
            this.gameObject.transform.Find("Spotlight").gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        if (this.gameObject.transform.Find("Spotlight") != null)
        {
            this.gameObject.transform.Find("Spotlight").gameObject.SetActive(false);
        }
    }



    /// **********************************
    /// Calculations using exponential curve
    /// **********************************

    float KnockbackMultiplier(float time) //Calculations for exponential power
    {
        float grantedPower = Mathf.Pow(4, time);
        return grantedPower;
    }

    float SmallMultiplier(float time) //Calculations for speed and localscale
    {
        float giveMeSpeed = Mathf.Pow(1.6f, time); //Used 1.3f for smaller multiplier
        return giveMeSpeed;
    }





    //Hooking!
    private void ChargeHook()
    {
        if (playerCannotShoot) return;
        if (chargingPushProjectile) return;

        aimingArrows.SetActive(true);
        chargingGrapplingHook = true;
    }

    private void ShootHook()
    {
        if (playerCannotShoot) return;

        chargingGrapplingHook = false;
        aimingArrows.SetActive(false);

        GrapplingHook projectile = Instantiate(hookProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<GrapplingHook>();
        projectile.transform.forward = transform.forward;
        projectile.direction = transform.forward;
        projectile.hookOwner = gameObject;
        projectile.playerCenter = playerCenter;
        hProjectile = projectile;
        pullCooldownTimer = pullCooldown;
    }

    //Called when player is stunned
    //Fixes bugs when player is aiming hook, and gets stunned in the middle.
    public void DisruptHookAiming()
    {
        chargingGrapplingHook = false;
    }

    //Force destroy grappling hook
    public void DestroyGrapplingHook()
    {
        if(hProjectile != null)
            hProjectile.FinishHookSequence();
    }
}
