using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {
	public Transform target;
//	private float edgeBorder  = 0.1f;
	private float horizontalSpeed = 360.0f;
	private float verticalSpeed = 120.0f;
	private float minHorizontal = -125.0f;
	private float maxHorizontal = 35.0f;
	private float minVertical = 0.0f;
	private float maxVertical = 90.0f;
	private float x = 0.0f;
	private float y = 0.0f;
	private float distance = 0.0f;
	
	void Start() {
		x = transform.eulerAngles.y;
		y = transform.eulerAngles.x;
		distance = (transform.position - target.position).magnitude;
	}

	void LateUpdate() {
		float dt = Time.deltaTime;
		x -= Input.GetAxis("Horizontal") * horizontalSpeed * dt;
		y += Input.GetAxis("Vertical") * verticalSpeed * dt;

		x = ClampAngle(x, minHorizontal, maxHorizontal);
		y = ClampAngle(y, minVertical, maxVertical);
		
		Quaternion orientation = Quaternion.Euler(y, x, 0);
		Vector3 location = orientation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
		
		transform.rotation = orientation;
		transform.position = location;
	}
	
	static public float ClampAngle(float angle, float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}