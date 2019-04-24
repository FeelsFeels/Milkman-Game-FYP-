using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control")]
    private float translateSpeed = 2.0f;
    private float rotateSpeed = 2.0f;
    public CharacterController playController;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 0.4f;
    private Vector3 angle;

    public string HorizontalInput;
    public string VerticalInput;
    //public static int PlayerNo; //may wanna ++ whenever new player join... etc etc hm.

    [Header("Player Die")]
    public bool isDead = false;

    public Transform respawnPosition;
    public float respawnDelay;

    // Object's Components
    private MeshRenderer boxRenderer;
    private BoxCollider boxCollider;
    private Rigidbody rb;

    void Reset()
    {
        playController = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {
       
        // Components
        boxRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //move front
        moveDirection = new Vector3(0, 0, Input.GetAxis(VerticalInput));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= translateSpeed;

        //rotate and change direction
        angle = transform.eulerAngles;
        angle.y += Input.GetAxis(HorizontalInput) * rotateSpeed;
        transform.eulerAngles = angle;
        moveDirection.y -= gravity * Time.deltaTime;
        playController.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collide with "Hole", player dies.
        if (other.tag == "Hole")
        {
            isDead = true;
            Die();
        }
    }

    // This hides the player when dead, and makes it reappear when alive.
    public void HidePlayerWhenDead()
    {
        if (isDead == true)
        {
            boxRenderer.enabled = false;
            boxCollider.enabled = false;
        }

        if (isDead == false)
        {
            boxRenderer.enabled = true;
            boxCollider.enabled = true;
        }

    }

    public void Die()
    {
        // Makes player disappear
        HidePlayerWhenDead();

        // Respawns player
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        if (isDead == true)
        {
            yield return new WaitForSeconds(respawnDelay);
            gameObject.transform.position = respawnPosition.transform.position;
            isDead = false;
            HidePlayerWhenDead();
        }
    }
}

