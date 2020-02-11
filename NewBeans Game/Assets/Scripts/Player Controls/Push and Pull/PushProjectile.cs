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
    public AudioManager audioManager;
    public GameObject openHandModel;
    public GameObject closedHandModel;


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

        //Changing Model for more oomph
        if (!openHandModel && !closedHandModel)
            return;
        if (multiplier > 5.25f) //The multiplier resulting from charging for 1.2 seconds (1.2 ^ 4)
        {
            //Change color of hand to solid red
            Renderer openHandRenderer = openHandModel.GetComponent<Renderer>();
            Color32 color = openHandRenderer.material.color;
            color.a = 255;
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            openHandRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", color);
            openHandRenderer.SetPropertyBlock(propBlock);

            //Shows open hand, disables closed hand
            openHandModel.gameObject.SetActive(true);
            closedHandModel.gameObject.SetActive(false);
        }
        else
        {
            openHandModel.gameObject.SetActive(false);
            closedHandModel.gameObject.SetActive(true);
        }

    }


    public void ReflectShot(Vector3 collisionNormal)
    {
        if(timesReflected >= 1)
        {
            Destroy(gameObject);
        }
        Vector3 reflectDirection = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        speed *= 0.8f;
        rb.velocity = reflectDirection * speed;
        transform.rotation = Quaternion.LookRotation(reflectDirection);
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
                FindObjectOfType<AudioManager>().Play("Player Knocked Back");
            }
            //AudioManager.instance.Play("Player Knocked Back");
            Vector3 direction = collision.transform.position - transform.position;
            direction = -direction.normalized;

            playerHit = player;
            Instantiate(player.playerPushedEffect, player.transform);
            if(hitParticles != null)
                Instantiate(hitParticles, player.transform.position + Vector3.up * 2, Quaternion.identity);
            //player.GetComponent<Rigidbody>().AddForce(direction * knockbackStrength);
            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackToUse);
            playerHit.lastHitBy = ownerPlayer.GetComponent<PlayerController>();

            //Player is hit, make ui show
            FindObjectOfType<LastManStandingTracker>().UpdateHitUI(playerHit.inputInfo);


            //Charging special skills

            if (ownerPlayer.GetComponent<SkillSetManager>() != null)
            {
                ownerPlayer.GetComponent<SkillSetManager>().ChargeSpecialSkill(knockbackToUse / 5); //Reduced amt of charge for ulti
            }

            if (player.GetComponent<SkillSetManager>() != null)
            {
                player.GetComponent<SkillSetManager>().ChargeSpecialSkill(knockbackToUse);
            }
            Destroy(gameObject);
        }


        //If after checking player collision, recheck and
        //Dont do anything if tagged player
        if (collision.collider.CompareTag("Player"))
        {
            return;
        }

        if (!player)
        {
            Instantiate(player.playerPushedEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void PushARigidbody(Rigidbody rb)
    {
        Vector3 direction = rb.transform.position - transform.position;
        direction = -direction.normalized;
        
        if (hitParticles != null)
            Instantiate(hitParticles, rb.transform.position + Vector3.up * 2, Quaternion.identity);
        //player.GetComponent<Rigidbody>().AddForce(direction * knockbackStrength);
        rb.AddForce(knockbackDirection * knockbackToUse);
        Destroy(gameObject);
    }
}
