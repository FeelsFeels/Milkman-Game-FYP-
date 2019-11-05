using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;

    public GameObject waterProjectile;
    public GameObject hookProjectile;

    private Animator animator;  //For blinking indication
    public GameObject aimingArrows;
    public Image chargingIndication;

    private GrapplingHook hProjectile;

    private PlayerController playerScript;


    //States and timing
    private float pushCooldownTimer;
    public float pushCooldown;
    private float pushChargedTime;
    private float pushChargedMaxTime = 1.5f;
    public bool chargingPushProjectile;
    public bool chargingGrapplingHook;

    public float baseKickbackForce;
    public float explodeRadius = 10f;
    public LayerMask playerLayer;
    public float explodeSpreadAngle = 90;
    public float explodeForce=2500;
    float timeCharged;

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
        animator = transform.Find("Canvas").GetComponent<Animator>();

        // hProjectile.GetComponent<Shoot>().castedByPlayer = player;
        watergunInput = playerScript.AButtonInput;
        chargingInput = playerScript.RightBumper;
        hookInput = playerScript.BButtonInput;

        aimingArrows.SetActive(false);
    }

    private void Update()
    {
        pushCooldownTimer -= Time.deltaTime;

        if (playerScript.playerStunned || playerScript.isDead) //Check if player is stunned or dead. If dead/stunned, do not shoot
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
            }
        }

        //Charge Push Projectile
        if (Input.GetButtonDown(watergunInput) && pushCooldownTimer <= 0)
        {
            ChargePushProjectile();
        }

        if (usingRightBumper && Input.GetButtonDown(chargingInput) && pushCooldownTimer <= 0)
        {
            ChargePushProjectile();
        }

        if (usingRightBumper && Input.GetButtonUp(chargingInput) && chargingPushProjectile)
        {
            ShootPushProjectile();
        }

        if (Input.GetButtonUp(watergunInput) && chargingPushProjectile)
        {
            ShootPushProjectile();
        }

        

        //Charge Grappling Hook
        if (Input.GetButtonDown(hookInput))
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
        }

        if (hProjectile != null)
            playerScript.shootingHook = true;
        else
            playerScript.shootingHook = false;
    }

    private void ChargePushProjectile()
    {
        aimingArrows.SetActive(true);
        chargingPushProjectile = true;
    }

    private void ShootPushProjectile()
    {
        PushProjectile projectile = Instantiate(waterProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z)
                                                                            , Quaternion.identity).GetComponent<PushProjectile>();
        //projectile.InitialiseShot(pushChargedTime, shootOrigin.forward, gameObject);
        projectile.ShotInitialised(KnockbackMultiplier(timeCharged), SmallMultiplier(timeCharged), shootOrigin.forward, gameObject);

        //Scale projectile size
        projectile.gameObject.transform.localScale = new Vector3(SmallMultiplier(pushChargedTime), SmallMultiplier(pushChargedTime), SmallMultiplier(pushChargedTime)); 

        //kickback to player
        Vector3 direction = -transform.forward;
        float percentage = 1 + (pushChargedTime / pushChargedMaxTime) * 3;
        playerScript.rb.AddForce(direction * (baseKickbackForce * percentage));
        

        //Reset states
        aimingArrows.SetActive(false);
        chargingIndication.gameObject.SetActive(false);
        chargingPushProjectile = false;
        animator.SetBool("activateChargeBlink", false);
        pushChargedTime = 0;
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

    void ShotgunAttack() {
        StartCoroutine(ShotgunFireLight());

        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(Physics.OverlapSphere(transform.position, explodeRadius, playerLayer)); //Find all players in range
        Debug.Log("Shotgunning");

        if (colliders.Contains(this.GetComponent<Collider>())) //If it includes this player,
        {
            colliders.Remove(this.GetComponent<Collider>()); // remove
            Debug.Log("Remove self");
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

    float SmallMultiplier(float time) //Calculations for speed
    {
        float giveMeSpeed = Mathf.Pow(1.3f, time); //Used 1.3f for smaller multiplier
        return giveMeSpeed;
    }





    //Hooking!
    private void ChargeHook()
    {
        aimingArrows.SetActive(true);
        chargingGrapplingHook = true;
    }

    private void ShootHook()
    {
        chargingGrapplingHook = false;
        aimingArrows.SetActive(false);

        GrapplingHook projectile = Instantiate(hookProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<GrapplingHook>();
        //projectile.Init();
        projectile.direction = shootOrigin.forward;
        projectile.hookOwner = gameObject;
        hProjectile = projectile;

        //It looks terrible, dont
        //playerScript.animator.SetTrigger("Attack");
    }

    //Called when player is stunned
    //Fixes bugs when player is aiming hook, and gets stunned in the middle.
    public void DisruptHookAiming()
    {
        chargingGrapplingHook = false;
    }

}
