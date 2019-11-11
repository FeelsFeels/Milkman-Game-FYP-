using UnityEngine;
using System.Collections;

public class Normalize : MonoBehaviour {
	public Transform point1;
	public Transform point2;
	private Vector3 vector1;
	private int multiplier = 10;

	void Start() {
		var vector1 = (point2.position - point1.position);
		Debug.Log("Original vector: " + vector1); // Original vector
		Debug.Log("Original vector magnitude: " + vector1.magnitude); // Original vector magnitude
		Debug.Log("Scalar multiplication of vector magnitude by " + multiplier + " times: " + vector1.magnitude * multiplier); // Scale original vector by multiplier
		Debug.Log("Normalized vector: " + vector1.normalized); // Unit vector
		Debug.Log("Normalized vector magnitude: " + vector1.normalized.magnitude); // Unit vector magnitude
		Debug.Log("Scalar multiplication of vector magnitude by " + multiplier + " times: " + vector1.normalized.magnitude * multiplier); // Scale unit vector by multiplier
	}
	
	void Update() {
		Debug.DrawLine(point1.position, point2.position, Color.red);
	}
}