using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float translateSpeed = 2.0f;
    private float rotateSpeed = 2.0f;
    public CharacterController playController;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 0.4f;
    private Vector3 angle;


    public bool fallIntoHole = false;

    public Transform respawnPosition;

    void Reset()
    {
        playController = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //move front
        moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical (Player 1)"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= translateSpeed;

        //rotate and change direction
        angle = transform.eulerAngles;
        angle.y += Input.GetAxis("Horizontal (Player 1)") * rotateSpeed;
        transform.eulerAngles = angle;
        moveDirection.y -= gravity * Time.deltaTime;
        playController.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            fallIntoHole = true;
        }
    }

    void Die()
    {
        if (fallIntoHole == true)
        {
            // Player disappears
            gameObject.SetActive(false);
            // Player respawns at 0,0,0

        }
    }

    IEnumerator RespawnPlayer()
    {
        gameObject.transform.position = respawnPosition;
    }
}

