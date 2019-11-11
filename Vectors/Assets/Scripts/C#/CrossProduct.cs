using UnityEngine;
using System.Collections;

public class CrossProduct : MonoBehaviour {
	private Vector3 vector1;
	private Vector3 vector2;
	private Vector3 crossVector;

	void Start() {
		vector1 = new Vector3(5f, 0f, 5f);
		vector2 = new Vector3(5f, 0f, -2.5f);
		
		crossVector = Vector3.Cross(vector1, vector2);
		Debug.Log(crossVector);
		Debug.Log("Area of parallelogram: " + crossVector.magnitude);
	}

	void Update() {
		Debug.DrawLine(Vector3.zero, new Vector3(5f, 0f, 5f), Color.red);
		Debug.DrawLine(Vector3.zero, new Vector3(-5f, 0f, -2.5f), Color.blue);
		Debug.DrawLine(Vector3.zero, crossVector, Color.yellow);
	}
}