using UnityEngine;
using System.Collections;

public class AdditionSubtraction : MonoBehaviour {
	private Vector3 vector1;
	private Vector3 vector2;
	private Vector3 resultantVector;

	void Start() {
		vector1 = new Vector3(3, 0, 2);
		vector2 = new Vector3(4, 0, -5);
		
		resultantVector = vector1 + vector2;
		Debug.Log(resultantVector);
	}
	
	void Update() {
		Debug.DrawLine(Vector3.zero, vector1, Color.red);
		Debug.DrawLine(Vector3.zero, vector2, Color.blue);
		Debug.DrawLine(Vector3.zero, resultantVector, Color.green);
	}
}
