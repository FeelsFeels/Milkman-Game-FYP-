using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : MonoBehaviour
{
    public Vector3 knockbackDirection;
    public GameObject ownerPlayer;

    public float speed;

    public float baseKnockback;
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
        if(durationCharged < 0.5f)
        {
            knockbackToUse = baseKnockback / 2;
            speed *= 0.8f;
        }
        else if(durationCharged < 1f)
        {
            knockbackToUse = baseKnockback;
        }
        else if(durationCharged < 1.5f)
        {
            knockbackToUse = baseKnockback * 2;
            speed *= 1.2f;
        }
        else if (durationCharged >= 1.5f)
        {
            knockbackToUse = baseKnockback * 4;
            speed *= 1.4f;
        }

        ownerPlayer = shootingPlayer;
        knockbackDirection = shotDirection;
        rb.velocity = shotDirection * speed;
        //knockbackToUse = baseKnockback * Mathf.Clamp(durationCharged, 1, 3);
        print(knockbackToUse);
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
