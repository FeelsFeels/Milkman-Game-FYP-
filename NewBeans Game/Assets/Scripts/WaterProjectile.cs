using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public Vector3 direction;
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
    }

    private void Update()
    {
        transform.Translate(direction * speed);
    }

    private void Explode()
    {
        playerHit.rb.AddExplosionForce(knockbackStrength, transform.position, knockbackRadius, upwardsModifier);

        Destroy(gameObject);    //Unless we wanna add object pooling coolbeans😎🆒 stuff
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player)
        {
            playerHit = player;

            Explode();
            print(playerHit);
        }
    }


}
