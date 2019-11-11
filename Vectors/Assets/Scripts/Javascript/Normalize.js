#pragma strict

var point1 : Transform;
var point2 : Transform;
private var vector1 : Vector3;
private var multiplier : int = 10;

function Start() {
	var vector1 = (point2.position - point1.position);
	Debug.Log("Original vector: " + vector1); // Original vector
	Debug.Log("Original vector magnitude: " + vector1.magnitude); // Original vector magnitude
	Debug.Log("Scalar multiplication of vector magnitude by " + multiplier + " times: " + vector1.magnitude * multiplier); // Scale original vector by multiplier
	Debug.Log("Normalized vector: " + vector1.normalized); // Unit vector
	Debug.Log("Normalized vector magnitude: " + vector1.normalized.magnitude); // Unit vector magnitude
	Debug.Log("Scalar multiplication of vector magnitude by " + multiplier + " times: " + vector1.normalized.magnitude * multiplier); // Scale unit vector by a multiplier
}

function Update() {
	Debug.DrawLine(point1.position, point2.position, Color.red);
}