using UnityEngine;
using System.Collections;

public class ResolvingComponents : MonoBehaviour {
	private Vector3 vector1;
	private Vector3 vector2;
	private Vector3 vector3;
	private float angleRadian;
	private float angleDegree;
	private float dotScalar;
	private Vector3 xProjection;
	private Vector3 yProjection;

	void Start() {
		vector1 = new Vector3(7.5f, 0f, 0f);
		vector2 = new Vector3(5f, 2.5f, 0f);
		vector3 = new Vector3(0f, 7.5f, 0f);
		
		dotScalar = Vector3.Dot(vector2, vector1);
		Debug.Log(dotScalar);
		angleRadian = Mathf.Acos(dotScalar / (vector1.magnitude * vector2.magnitude));
		Debug.Log("Angle in radian: " + angleRadian);
		angleDegree = angleRadian * Mathf.Rad2Deg;
		Debug.Log("Angle in degree: " + angleDegree);
		xProjection = (Vector3.Dot(vector2, vector1) / Vector3.Dot(vector1, vector1)) * vector1;
		Debug.Log("Resolved x component: " + xProjection);
		//	yProjection = (Vector3.Dot(vector2, vector3) / Vector3.Dot(vector3, vector3)) * vector3; // Approach #1
		yProjection = vector2 - xProjection; // Approach #2
		Debug.Log("Resolved y component: " + yProjection);
	}
	
	void Update() {
		Debug.DrawLine(Vector3.zero, new Vector3(7.5f, 0f, 0f), Color.red);
		Debug.DrawLine(Vector3.zero, new Vector3(5f, 2.5f, 0f), Color.green);
		Debug.DrawLine(Vector3.zero, new Vector3(0f, 7.5f, 0f), Color.blue);
		Debug.DrawLine(Vector3.zero, xProjection, Color.yellow);
		Debug.DrawLine(Vector3.zero, yProjection, Color.cyan);
	}
}