#pragma strict

private var vector1 : Vector3;
private var vector2 : Vector3;
private var crossVector : Vector3;

function Start() {
	vector1 = new Vector3(5, 0, 5);
	vector2 = new Vector3(5, 0, -2.5);
	
	crossVector = Vector3.Cross(vector1, vector2);
	Debug.Log(crossVector);
	Debug.Log("Area of parallelogram: " + crossVector.magnitude);
}

function Update() {
	Debug.DrawLine(Vector3.zero, Vector3(5, 0, 5), Color.red);
	Debug.DrawLine(Vector3.zero, Vector3(-5, 0, -2.5), Color.blue);
	Debug.DrawLine(Vector3.zero, crossVector, Color.yellow);
}