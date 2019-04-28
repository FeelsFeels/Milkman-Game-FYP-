using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootOrigin;

    public GameObject waterProjectile;
    public GameObject hookProjectile;


    private float shootCooldown;


    ///If we want to use the Input Manager
    //public string watergunInput;
    //public string hookInput;
    /// //////////////////////////////////



    private void Start()
    {
        //shootOrigin = transform.Find("ShootOrigin");
        
        shootCooldown = 0;
    }

    private void Update()
    {
        if (shootCooldown <= 0)
        {
            //Shoot Watergun
            if (Input.GetKeyDown(KeyCode.J))
            {
                ShootWaterGun();
            }

            //Shoot Grappling Hook
            if (Input.GetKeyDown(KeyCode.K))
            {
                ShootHook();
            }
        }
    }

    private void ShootWaterGun()
    {
        //Bloody bugs
        //WaterProjectile projectile = Instantiate(waterProjectile, shootOrigin.transform.position, Quaternion.identity).GetComponent<WaterProjectile>();
        WaterProjectile projectile = Instantiate(waterProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<WaterProjectile>();
        print(shootOrigin.position);
        projectile.direction = shootOrigin.forward;
    }

    private void ShootHook()
    {
        GrapplingHook projectile = Instantiate(hookProjectile, new Vector3(shootOrigin.transform.position.x, shootOrigin.transform.position.y, shootOrigin.transform.position.z), Quaternion.identity).GetComponent<GrapplingHook>();
        projectile.direction = shootOrigin.forward;
        projectile.hookOwner = gameObject;
    }


}
