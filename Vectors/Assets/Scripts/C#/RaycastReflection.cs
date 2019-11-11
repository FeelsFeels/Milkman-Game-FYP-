using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class RaycastReflection : MonoBehaviour {
	private Transform player;
	private LineRenderer lineRenderer;

	private Ray ray; // A ray
	private RaycastHit hit; // To gather informartion about the ray's collision
	private Vector3 inDirection; // Reflection direction
	public int nReflections = 2; // Number of reflections
	private int nPoints; // Number of points at the line renderer

	void Awake() {
		// Get the attached transform component
		player = this.GetComponent<Transform>();

		// Get the attached LineRenderer component
		lineRenderer = this.GetComponent<LineRenderer>();
	}

	void Update() {
		// Clamp the number of reflections between 1 and int capacity
		nReflections = Mathf.Clamp(nReflections, 1, nReflections);
		// Cast a new ray forward, from the current attached game object position
		ray = new Ray(player.position, player.forward);
		
		// Represent the ray using a line that can only be viewed at the scene tab
		Debug.DrawRay(player.position, player.forward * 100, Color.white);
		
		// Set the number of points to be the same as the number of reflections
		nPoints = nReflections;
		// Make the lineRenderer have nPoints
		lineRenderer.positionCount = nPoints; 
		// Set the first point of the line at the current attached game object position
		lineRenderer.SetPosition(0, player.position);
		
		for(int i = 0; i <= nReflections; i++) {
			// If ray has not reflected yet
			if(i == 0) {
				// Check if the ray has hit something
				if(Physics.Raycast(ray.origin, ray.direction, out hit, 100)) { // Cast the ray 100 units at the specified direction
					// The reflection direction is the reflection of the current ray direction flipped at the hit normal
					inDirection = Vector3.Reflect(ray.direction, hit.normal);
					// Cast the reflected ray, using the hit point as the origin and the reflected direction as the direction
					ray = new Ray(hit.point, inDirection);
					
					// Draw the normal - only can be seen at the Scene tab for debugging purposes
					Debug.DrawRay(hit.point, hit.normal * 4, Color.blue);
					// Represent the ray using a line that can only be viewed at the scene tab
					Debug.DrawRay(hit.point, inDirection * 100, Color.white);

					// Print the name of the object the cast ray has hit, at the console
					Debug.Log("Hit object name: " + hit.transform.name);

					// If the number of reflections is set to 1
					if(nReflections == 1) {
						// Add a new vertex to the line renderer
						lineRenderer.positionCount = ++nPoints;
					}
					
					// Set the position of the next vertex at the line renderer to be the same as the hit point
					lineRenderer.SetPosition(i+1, hit.point);
				}
			} else { // The ray has reflected at least once
				// Check if the ray has hit something
				if(Physics.Raycast(ray.origin, ray.direction, out hit, 100)) { // Cast the ray 100 units at the specified direction
					// Refletion direction is the reflection of the ray's direction at the hit normal
					inDirection = Vector3.Reflect(inDirection, hit.normal);
					// Cast the reflected ray, using the hit point as the origin and the reflected direction as the direction
					ray = new Ray(hit.point, inDirection);
					
					// Draw the normal - only can be seen at the Scene tab for debugging purposes
					Debug.DrawRay(hit.point, hit.normal * 4, Color.blue);
					// Represent the ray using a line that can only be viewed at the scene tab
					Debug.DrawRay(hit.point, inDirection * 100, Color.white);
					
					// Print the name of the object the cast ray has hit, at the console
					Debug.Log("Object name: " + hit.transform.name);
					
					// Add a new vertex to the line renderer
					lineRenderer.positionCount = ++nPoints;
					// Set the position of the next vertex at the line renderer to be the same as the hit point
					lineRenderer.SetPosition(i + 1, hit.point);
				} 
			}
		}
	}
}