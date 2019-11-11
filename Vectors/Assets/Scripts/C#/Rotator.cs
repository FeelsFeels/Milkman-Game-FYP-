using UnityEngine;
using System.Collections;

public class Rotator: MonoBehaviour {
	private float angle; // Stores rotation of mouse wheel
	public float scale = 1000.0f; // Multiplier for mouse wheel input
	private Vector3 storedOrientation; // Stores the rotation of the attached gameObject
	private RaycastHit hit; // Check if ray hits a collider
	private string objectName = null;

	void Update() {
		if(Input.GetMouseButtonDown(0)) { // Left mouse button clicked
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Send a ray from camera through mouse cursor position

			if(Physics.Raycast(ray, out hit)) {
				if(hit.transform.tag == "Wall") {
					storedOrientation = hit.transform.eulerAngles;
					objectName = hit.transform.name;
				}
			}
		}

		angle = Input.GetAxis("Mouse ScrollWheel") * scale * Time.deltaTime;

		if(angle != 0) {
			storedOrientation = new Vector3(storedOrientation.x, storedOrientation.y + angle, storedOrientation.z);
			hit.transform.eulerAngles = storedOrientation;
		}
	}

	void OnGUI() {
		GUI.color = Color.white;
		GUI.Label(new Rect(20, 20, 400, 200), "Press WASD to manipulate player\nUse mouse cursor to select a wall by left mouse click\nUse mouse scroll wheel to change its orientation");
	}
}