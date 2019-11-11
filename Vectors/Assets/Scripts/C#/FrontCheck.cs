using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour {
	public Transform target = null;

	void Update() {
		if(Input.GetKeyDown(KeyCode.C)) {
			Vector3 toTarget = (target.position - transform.position).normalized;

			if(Vector3.Dot(toTarget, transform.right) > 0) {
				Debug.Log(target.name + " is in front of " + this.gameObject.name);
			} else {
				Debug.Log(target.name + " is behind " + this.gameObject.name);
			}
		}
	}
}