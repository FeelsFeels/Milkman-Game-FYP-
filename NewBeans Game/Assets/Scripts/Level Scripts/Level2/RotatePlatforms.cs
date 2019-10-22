using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatforms : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public Transform pathHolder;
    public float platformMoveSpeed = 5;
    public float rotateSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }


        for (int i = 0; i < platforms.Count; i++) //For each platforms,
        {
            StartCoroutine(FollowPath(waypoints, platforms[i], i)); //Make the platform section revolve by following path
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator FollowPath(Vector3[] waypoints, GameObject platform, int index)
    {

        platform.transform.position = waypoints[index];
        int targetWaypointIndex = (index + 1) % waypoints.Length;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];

        while (true)
        {
            platform.transform.RotateAround(platform.transform.position, platform.transform.forward, Time.deltaTime * rotateSpeed); //Rotate the platform. Using local z axis as it is upwards in the world space
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetWaypoint, platformMoveSpeed * Time.deltaTime); //Lerp pos
            if (platform.transform.position == targetWaypoint) //If reached position
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length; //Change to next target pos
                targetWaypoint = waypoints[targetWaypointIndex];
            }
            yield return null;
        }

    }




    //Visualise points on the pathway
    private void OnDrawGizmos()
    {
        Vector3 startPos = pathHolder.GetChild(0).position;
        Vector3 prevPos = startPos;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 3f);
            Gizmos.DrawLine(prevPos, waypoint.position);
            prevPos = waypoint.position;
        }
        Gizmos.DrawLine(prevPos, startPos);
    }

}
