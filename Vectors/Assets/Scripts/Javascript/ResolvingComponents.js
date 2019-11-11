#pragma strict

private var vector1 : Vector3;
private var vector2 : Vector3;
private var vector3 : Vector3;
private var angleRadian : float;
private var angleDegree : float;
private var dotScalar : float;
private var xProjection : Vector3;
private var yProjection : Vector3;

function Start() {
	vector1 = new Vector3(7.5, 0, 0);
	vector2 = new Vector3(5, 2.5, 0);
	vector3 = new Vector3(0, 7.5, 0);
	
	dotScalar = Vector3.Dot(vector2, vector1);
	print(dotScalar);
	angleRadian = Mathf.Acos(dotScalar / (vector1.magnitude * vector2.magnitude));
	print("Angle in radian: " + angleRadian);
	angleDegree = angleRadian * Mathf.Rad2Deg;
	print("Angle in degree: " + angleDegree);
	xProjection = (Vector3.Dot(vector2, vector1) / Vector3.Dot(vector1, vector1)) * vector1;
	print("Resolved x component: " + xProjection);
	// yProjection = (Vector3.Dot(vector2, vector3) / Vector3.Dot(vector3, vector3)) * vector3; // Approach #1
	yProjection = vector2 - xProjection; // Approach #2
	print("Resolved y component: " + yProjection);
}

function Update() {
	Debug.DrawLine(Vector3.zero, Vector3(7.5, 0, 0), Color.red);
	Debug.DrawLine(Vector3.zero, Vector3(5, 2.5, 0), Color.green);
	Debug.DrawLine(Vector3.zero, Vector3(0, 7.5, 0), Color.blue);
	Debug.DrawLine(Vector3.zero, xProjection, Color.yellow);
	Debug.DrawLine(Vector3.zero, yProjection, Color.cyan);
}