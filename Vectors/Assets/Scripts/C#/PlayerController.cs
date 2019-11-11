using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float speed = 5.0f;
	private CharacterController playerController;
	private Vector3 moveDirection = Vector3.zero;
	private float gravity = 0.5f;

	// Use this for initialization
	void Start() {
		playerController = GetComponent<CharacterController>();
	}

	void Update() {
		if(playerController.isGrounded) {
			moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}

		Vector3 angle = transform.eulerAngles;
		angle.y += Input.GetAxis("Horizontal") * 5;
		transform.eulerAngles = angle;
		moveDirection.y -= gravity * Time.deltaTime;
		playerController.Move(moveDirection * Time.deltaTime);
	}
}