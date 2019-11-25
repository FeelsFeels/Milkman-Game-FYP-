using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTornado : MonoBehaviour
{

    public float speed;
    Rigidbody rb;
    Transform skillUser;
    public float suctionForce = 1000f;
    bool tornadoStarted = false;
    public float tornadoRadius = 5f;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSkillUser(Transform user)
    {
        skillUser = user;
        tornadoStarted = true;
    }


    public void BrewStorm(Vector3 dir)
    {
        //Move it forward
        rb.velocity = dir * speed;
        StartCoroutine(Storm());
    }

    // Note: Have just removed collider on this obj so no longer using this
    //private void OnCollisionEnter(Collision collision)
    //{
        //    PlayerController player = collision.transform.GetComponent<PlayerController>();

        //    if (player && collision.gameObject != skillUser)
        //    {
        //        Vector3 direction = collision.transform.position - transform.position;
        //        direction = direction.normalized;

        //        //Push player towards centre
        //        player.GetComponent<Rigidbody>().AddForce(direction * suctionForce);

        //    }


        // IF under water
     //   if (transform.position.y< -20f || collision.gameObject.tag == "Water")
     //   {
     //           DestroyStorm();
     //   }
     //}

    IEnumerator Storm()
    {
        while (tornadoStarted) { 
            List<Collider> colliders = new List<Collider>();
            colliders.AddRange(Physics.OverlapSphere(transform.position, tornadoRadius, playerLayer));

            if (colliders.Contains(skillUser.GetComponent<Collider>())) //If it includes this player,
            {
                colliders.Remove(skillUser.GetComponent<Collider>()); // remove
            }

            foreach (Collider collider in colliders)
            {
                //Debug.Log("Player found");
                //Get the inwards direction
                Vector3 direction =  transform.position - collider.transform.position;
                direction = direction.normalized;

                //Push player towards centre of tornado
                collider.GetComponent<Rigidbody>().AddForce(direction * suctionForce);
                //Debug.Log("Sucking");
            }

            yield return null;
        }
    }

    public void DestroyStorm()
    {
        tornadoStarted = false;
        Destroy(this.gameObject);
    }


}
