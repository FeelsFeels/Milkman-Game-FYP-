#pragma strict

private var vector1 : Vector3;
private var vector2 : Vector3;
private var resultantVector : Vector3;

function Start() {
	vector1 = new Vector3(3, 0, 2);
	vector2 = new Vector3(4, 0, -5);
	
	resultantVector = vector1 + vector2;
	Debug.Log(resultantVector);
}

function Update() {
	Debug.DrawLine(Vector3.zero, vector1, Color.red);
	Debug.DrawLine(Vector3.zero, vector2, Color.blue);
	Debug.DrawLine(Vector3.zero, resultantVector, Color.green);
}