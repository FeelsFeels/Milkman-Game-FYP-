using UnityEngine;
using System.Collections;

public class DotProduct : MonoBehaviour {
	public Transform point1;
	public Transform point2;
	public Transform point3;
	public Transform point4;
	public Vector3 vector1;
	public Vector3 vector2;
	public float dotScalar;

	void Start() {
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
	
	void Update() {
		Debug.DrawLine(point1.position, point2.position, Color.red);
		Debug.DrawLine(point3.position, point4.position, Color.blue);
	}
}