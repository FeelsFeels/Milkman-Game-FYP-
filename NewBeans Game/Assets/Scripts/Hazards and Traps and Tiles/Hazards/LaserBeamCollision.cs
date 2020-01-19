using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCollision : MonoBehaviour
{

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        PlayerController pc = other.GetComponent<PlayerController>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (pc)
            {
                pc.Hit(3f);
            }
            i++;
        }
    }
}
