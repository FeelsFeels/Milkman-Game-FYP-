using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public Vector3 knockbackDirection;
    public GameObject ownerPlayer;

    public float speed;

    public float knockbackStrength;
    public float knockbackRadius;
    public float upwardsModifier;
    public PlayerController playerHit;

    public bool exploding;

    private Rigidbody rb;

    private void Start()
    {
        Destroy(gameObject, 3);
        rb = GetComponent<Rigidbody>();
        rb.velocity = knockbackDirection * speed;
    }

    private void Update()
    {
        //transform.Translate(direction * speed);
    }

    // Rocky's non-AddForce Explode()
    //private void Explode()
    //{
    //    playerHit.rb.AddExplosionForce(knockbackStrength, transform.position, knockbackRadius, upwardsModifier);

    //    Destroy(gameObject);    //Unless we wanna add object pooling coolbeans😎🆒 stuff
    //}
    

    private void OnCollisionEnter(Collision collision)
    {

        PlayerController player = collision.transform.GetComponent<PlayerController>();

        Vector3 direction = collision.transform.position - transform.position;
        direction = -direction.normalized;

        if (player && collision.gameObject != ownerPlayer)
        {
            playerHit = player;
            //player.GetComponent<Rigidbody>().AddForce(direction * knockbackStrength);
            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackStrength);
            playerHit.lastHitBy = ownerPlayer;
            Destroy(gameObject);
            //Explode();
        }

        if (!player)
            Destroy(gameObject);
    }


}
