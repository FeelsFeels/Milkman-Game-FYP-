using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;

    public GameObject waterProjectile;
    public GameObject hookProjectile;

    public GameObject aimingArrows;

    private GrapplingHook hProjectile;

    private PlayerController playerScript;



    private float waterGunCooldownTimer;
    public float waterGunCooldown;
    public bool aiming;


    ///If we want to use the Input Manager
    public string watergunInput;
    public string hookInput;
    /// //////////////////////////////////


    private void Start()
    {
        //shootOrigin = transform.Find("ShootOrigin");
        waterGunCooldownTimer = 0;

        playerScript = GetComponent<PlayerController>();

        // hProjectile.GetComponent<Shoot>().castedByPlayer = player;
        watergunInput = playerScript.AButtonInput;
        hookInput = playerScript.BButtonInput;

        aimingArrows.SetActive(false);
    }

    private void Update()
    {
        waterGunCooldownTimer -= Time.deltaTime;

        if (playerScript.playerStunned)
        {
            return;
        }

        //Shoot Watergun
        if (Input.GetButtonDown(watergunInput) && waterGunCooldownTimer <= 0)
        {
            ShootWaterGun();
            waterGunCooldownTimer = waterGunCooldown;
        }


        //Shoot Grappling Hook
        if (Input.GetButtonDown(hookInput))
        {
            if (hProjectile == null)
            {
                ChargeHook();
            }
        }

        if (Input.GetButtonUp(hookInput) && aiming)
        {
            ShootHook();
        }

        if (hProjectile != null)
            playerScript.shootingHook = true;
        else
            playerScript.shootingHook = false;
    }

    private void ShootWaterGun()
    {
        if (playerScript.playerStunned)
            return;

        //WaterProjectile projectile = Instantiate(waterProjectile, shootOrigin.transform.position, Quaternion.identity).GetComponent<WaterProjectile>();
        PushProjectile projectile = Instantiate(waterProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<PushProjectile>();
        projectile.knockbackDirection = shootOrigin.forward;
        projectile.ownerPlayer = gameObject;

        //playerScript.animator.SetTrigger("Attack");
    }

    private void ChargeHook()
    {
        aimingArrows.SetActive(true);
        aiming = true;
    }

    private void ShootHook()
    {
        aiming = false;
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
        aiming = false;
    }

}
