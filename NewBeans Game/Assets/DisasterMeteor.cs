using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterMeteor : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;

    Vector3 randomPos;

    public GameObject meteorMarking;
    private GameObject spawnedMarking;

    public bool instantiatingMarker = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        randomPos = new Vector3(Random.Range(-5f, 20f), -0.7f, Random.Range(-25f, 0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MoveTowardsLevel();
    }

    public void MoveTowardsLevel()
    {
        Vector3 targetPos = randomPos;
        // print("Target Position: " + targetPos);

        if (instantiatingMarker == false)
        {
            spawnedMarking = Instantiate(meteorMarking, targetPos, Quaternion.identity);
            instantiatingMarker = true;
        }

        float step = moveSpeed * Time.deltaTime;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed);

        //rb.velocity = targetPos;
        if (gameObject.transform.position == targetPos)
        {
            print("Target Position Reached");
            
        }
    }


    void OnCollisionEnter(Collision other)
    {
        print("collisionsoto");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
            print("Player died");
        }
        Destroy(spawnedMarking);
        Destroy(gameObject);
    }
}