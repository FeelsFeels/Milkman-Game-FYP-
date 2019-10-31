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


    public void ShotInitialised(float multiplier, float smallMultiplier, Vector3 shotDirection, GameObject shootingPlayer)
    {
        knockbackToUse = multiplier * baseKnockback;
        speed *= smallMultiplier;
        ownerPlayer = shootingPlayer;
        knockbackDirection = shotDirection;
        rb.velocity = shotDirection * speed;
        print(knockbackToUse);

    }




    private void OnCollisionEnter(Collision collision)
    {
        PushProjectile pushProjectile = collision.transform.GetComponent<PushProjectile>();
            if (pushProjectile)
            return;

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
