using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSkill_ShootingMonkeyProjectile : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    Vector3 knockbackDirection;
    public GameObject monkey;


    public void Shoot(Vector3 direction)
    {
        transform.forward = direction;
        rb.velocity = direction * speed;
        knockbackDirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == monkey)
            return;

        PlayerController player = collision.transform.GetComponent<PlayerController>();

        if (player)
        {
            Instantiate(player.playerPushedEffect, player.transform);

            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * 200f);

            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
