using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterMeteor : MonoBehaviour
{
    public float moveSpeed;
    public float groundBreakRadius;

    private Rigidbody rb;

    Vector3 randomPos;

    public GameObject meteorMarking;
    private GameObject spawnedMarking;

    public bool instantiatingMarker = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        randomPos = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }


    private void FixedUpdate()
    {
        MoveTowardsLevel();
    }

    public void MoveTowardsLevel()
    {
        meteorMarking.transform.position = randomPos;

        Vector3 targetPos = randomPos;
        // print("Target Position: " + targetPos);

        float step = moveSpeed * Time.deltaTime;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed);

        //rb.velocity = targetPos;
    }

    void DestroyGround()
    {
        int layermask = 1 << LayerMask.NameToLayer("Ground");

        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, groundBreakRadius, layermask);
        foreach (Collider hit in collidersInRange)
        {
            Tile tile = hit.GetComponent<Tile>();
            if (tile)
            {
                if (tile.tileState == Tile.TileState.up)
                    tile.tileState = Tile.TileState.goingDown;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
        DestroyGround();
        Destroy(spawnedMarking);
        Destroy(gameObject);
    }
}