using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : MonoBehaviour
{
    public Vector3 knockbackDirection;
    public GameObject ownerPlayer;

    public float speed;
    public Vector3 lastFrameVelocity;

    public float baseKnockback; //Base knockback currently set to 500
    public float knockbackToUse;
    public PlayerController playerHit;
    private int timesReflected;

    public bool exploding;



    public Rigidbody rb;
    public GameObject hitParticles;
    public TrailRenderer trailRenderer;


    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void FixedUpdate()
    {
        lastFrameVelocity = rb.velocity;
    }

    public void ShotInitialised(float multiplier, float smallMultiplier, Vector3 shotDirection, GameObject shootingPlayer)
    {
        knockbackToUse = multiplier * baseKnockback;

        speed *= smallMultiplier;
        ownerPlayer = shootingPlayer;
        knockbackDirection = shotDirection;
        rb.velocity = shotDirection * speed;
    }


    public void ReflectShot(Vector3 collisionNormal)
    {
        Vector3 reflectDirection = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        speed *= 0.8f;
        rb.velocity = reflectDirection * speed;
        timesReflected++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PushProjectile pushProjectile = collision.transform.GetComponent<PushProjectile>();
        if (pushProjectile)
        {
            ReflectShot(collision.contacts[0].normal);
            return;
        }

        if((collision.gameObject.tag == "Rock" || collision.gameObject.tag == "GrabbableEnvironment") && timesReflected <= 1)
        {
            ReflectShot(collision.contacts[0].normal);
            return;
        }

        PlayerController player = collision.transform.GetComponent<PlayerController>();
        
        if ((player && collision.gameObject != ownerPlayer) || (player && timesReflected >= 1))
        {
            if (FindObjectOfType<AudioManager>())
            {
                FindObjectOfType<AudioManager>().Play("Player Knock Back");
            }
            Vector3 direction = collision.transform.position - transform.position;
            direction = -direction.normalized;

            playerHit = player;
            Instantiate(player.playerPushedEffect, player.transform);
            if(hitParticles != null)
                Instantiate(hitParticles, player.transform.position + Vector3.up * 2, Quaternion.identity);
            //player.GetComponent<Rigidbody>().AddForce(direction * knockbackStrength);
            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackToUse);
            playerHit.lastHitBy = ownerPlayer.GetComponent<PlayerController>();

            //Charging special skills

                if (ownerPlayer.GetComponent<SkillSetManager>() != null)
                {
                    ownerPlayer.GetComponent<SkillSetManager>().ChargeSpecialSkill(knockbackToUse/5); //Reduced amt of charge for ulti
                }

                if (player.GetComponent<SkillSetManager>() != null)
                {
                    player.GetComponent<SkillSetManager>().ChargeSpecialSkill(knockbackToUse);
                }
            

            Destroy(gameObject);
        }

        if (!player)
        {
            Instantiate(player.playerPushedEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
