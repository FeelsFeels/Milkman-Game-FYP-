using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : MonoBehaviour
{
    public Vector3 knockbackDirection;
    public GameObject ownerPlayer;

    public float speed;

    public float baseKnockback; //Base knockback currently set to 500
    public float knockbackToUse;
    public float knockbackRadius;
    public float upwardsModifier;
    public PlayerController playerHit;

    public bool exploding;

    public Rigidbody rb;
    public TrailRenderer trailRenderer;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    public void InitialiseShot(float durationCharged, Vector3 shotDirection, GameObject shootingPlayer)
    {
        //if(durationCharged < 0.5f)
        //{
        //    knockbackToUse = baseKnockback / 2;
        //    speed *= 0.8f;
        //}
        //else if(durationCharged < 1f)
        //{
        //    knockbackToUse = baseKnockback;
        //}
        //else if(durationCharged < 1.5f)
        //{
        //    knockbackToUse = baseKnockback * 3;
        //    speed *= 1.2f;
        //}
        //else if (durationCharged >= 1.5f)
        //{
        //    knockbackToUse = baseKnockback * 5;
        //    speed *= 1.4f;
        //}

        float timeCharged = Mathf.Min(durationCharged, 1.2f); //Clamp to max of 1.2f
        knockbackToUse = ChargedKnockback(timeCharged);
        speed *= SpeedMultiplier(timeCharged);

        ownerPlayer = shootingPlayer;
        knockbackDirection = shotDirection;
        rb.velocity = shotDirection * speed;
        //knockbackToUse = baseKnockback * Mathf.Clamp(durationCharged, 1, 3);
        print(knockbackToUse);
    }

    public void ShotInitialised(float multiplier, float smallMultiplier, Vector3 shotDirection, GameObject shootingPlayer)
    {
        knockbackToUse = multiplier * baseKnockback;
        speed *= smallMultiplier;
        ownerPlayer = shootingPlayer;
        knockbackDirection = shotDirection;
        rb.velocity = shotDirection * speed;
        print(knockbackToUse);

    }


    
    float ChargedKnockback(float time) //Calculations for exponential power
    {
        float grantedPower = Mathf.Pow(4, time) * baseKnockback;
        return grantedPower;
    }

    float SpeedMultiplier(float time) //Calculations for speed
    {
        float giveMeSpeed = Mathf.Pow(1.3f, time); //Used 1.3f for smaller multiplier
        return giveMeSpeed;
    }


    private void OnCollisionEnter(Collision collision)
    {

        PlayerController player = collision.transform.GetComponent<PlayerController>();

        Vector3 direction = collision.transform.position - transform.position;
        direction = -direction.normalized;

        if (player && collision.gameObject != ownerPlayer)
        {
            playerHit = player;
            Instantiate(player.playerPushedEffect, player.transform.position, player.transform.rotation);
            //player.GetComponent<Rigidbody>().AddForce(direction * knockbackStrength);
            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackToUse);
            playerHit.lastHitBy = ownerPlayer;
            Destroy(gameObject);
            //Explode();
        }

        if (!player)
            Destroy(gameObject);
    }


}
