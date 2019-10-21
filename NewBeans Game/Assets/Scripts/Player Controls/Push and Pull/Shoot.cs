using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;

    public GameObject waterProjectile;
    public GameObject hookProjectile;

    public GameObject aimingArrows;
    public Image chargingIndication;

    private GrapplingHook hProjectile;

    private PlayerController playerScript;


    //States and timing
    private float pushCooldownTimer;
    public float pushCooldown;
    private float pushChargedTime;
    private float pushChargedMaxTime = 1.7f;
    public bool chargingPushProjectile;
    public bool chargingGrapplingHook;

    public float baseKickbackForce;

    ///If we want to use the Input Manager
    public string watergunInput;
    public string hookInput;
    /// //////////////////////////////////


    private void Start()
    {
        //shootOrigin = transform.Find("ShootOrigin");
        pushCooldownTimer = 0;

        playerScript = GetComponent<PlayerController>();

        // hProjectile.GetComponent<Shoot>().castedByPlayer = player;
        watergunInput = playerScript.AButtonInput;
        hookInput = playerScript.BButtonInput;

        aimingArrows.SetActive(false);
    }

    private void Update()
    {
        pushCooldownTimer -= Time.deltaTime;

        if (playerScript.playerStunned)
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
            chargingIndication.fillAmount = pushChargedTime / 1.5f;

            if (pushChargedTime >= pushChargedMaxTime)
            {
                PushBackfire();
            }
        }

        //Charge Push Projectile
        if (Input.GetButtonDown(watergunInput) && pushCooldownTimer <= 0)
        {
            ChargePushProjectile();
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
        projectile.InitialiseShot(pushChargedTime, shootOrigin.forward, gameObject);

        //kickback to player
        Vector3 direction = -transform.forward;
        float percentage = 1 + (pushChargedTime / pushChargedMaxTime) * 3;
        playerScript.rb.AddForce(direction * (baseKickbackForce * percentage));
        

        //Reset states
        aimingArrows.SetActive(false);
        chargingIndication.gameObject.SetActive(false);
        chargingPushProjectile = false;
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
