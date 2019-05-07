using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;

    public GameObject waterProjectile;
    public GameObject hookProjectile;

    public GrapplingHook hProjectile;

    public PlayerController playerScript;

    private float waterGunCooldownTimer;
    public float waterGunCooldown;
    public bool canHook;


    ///If we want to use the Input Manager
    public string watergunInput;
    public string hookInput;
    /// //////////////////////////////////


    private void Start()
    {
        //shootOrigin = transform.Find("ShootOrigin");
        waterGunCooldownTimer = 0;
        canHook = true;

        playerScript = GetComponent<PlayerController>();

       // hProjectile.GetComponent<Shoot>().castedByPlayer = player;
    }

    private void Update()
    {
        waterGunCooldownTimer -= Time.deltaTime;
        //Shoot Watergun
        if (Input.GetButtonDown(watergunInput) && waterGunCooldownTimer <= 0)
        {
            ShootWaterGun();
            waterGunCooldownTimer = waterGunCooldown;
        }


        //Shoot Grappling Hook
        if (Input.GetButtonDown(hookInput))
        {
            if (hProjectile == null && canHook)
            {
                ShootHook();
                canHook = false;
            }
            else
            {
                hProjectile.PullFromLatch();
            }
        }

    }

    private void ShootWaterGun()
    {
        //Bloody bugs
        //WaterProjectile projectile = Instantiate(waterProjectile, shootOrigin.transform.position, Quaternion.identity).GetComponent<WaterProjectile>();
        WaterProjectile projectile = Instantiate(waterProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<WaterProjectile>();
        projectile.direction = shootOrigin.forward;
        projectile.ownerPlayer = gameObject;
    }

    private void ShootHook()
    {
        GrapplingHook projectile = Instantiate(hookProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<GrapplingHook>();
        //projectile.Init();
        projectile.direction = shootOrigin.forward;
        projectile.hookOwner = gameObject;
        hProjectile = projectile;

        
    }


}
