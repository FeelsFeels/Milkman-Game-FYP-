#pragma strict

var point1 : Transform;
var point2 : Transform;
var point3 : Transform;
var point4 : Transform;
var vector1 : Vector3;
var vector2 : Vector3;
var dotScalar : float;

function Start() {
	var vector1 = (point2.position - point1.position).normalized;
	Debug.Log(vector1);
	var vector2 = (point4.position - point3.position).normalized;
	Debug.Log(vector2);
	
	dotScalar = Vector3.Dot(vector1, vector2);
	Debug.Log(dotScalar);
	
	if(dotScalar < 0)
		Debug.Log("Vectors are in opposite direction");
	else if(dotScalar > 0)
		Debug.Log("Vectors are in same direction");
	else
		Debug.Log("Vectors are perpendicular to each other");
}

function Update() {
	Debug.DrawLine(point1.position, point2.position, Color.red);
	Debug.DrawLine(point3.position, point4.position, Color.blue);
}