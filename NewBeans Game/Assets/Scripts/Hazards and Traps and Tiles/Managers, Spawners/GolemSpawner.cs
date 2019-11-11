using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{
    public GameObject golemPrefab;

    public Transform[] spawnPoints; //In the scene, put these spawnpoints in the center of the section (1 October 2019, early prototype)


    public void GolemTestDebug()
    {
        print("spawnGolem");
    }

    public void SpawnGolem()
    {
        //Find an appropriate place to spawn the golem
        Vector3 spawnPoint = Vector3.zero;  //using vec3 as a pseudo null check
        while(spawnPoint == Vector3.zero)
        {
            int rand = Random.Range(0, spawnPoints.Length);

            RaycastHit hit;
            //Check whether the spawn point is nearby a hole
            if(Physics.Raycast(spawnPoints[rand].position, Vector3.down, out hit))
            {
                if (hit.collider.tag == "Hole")
                    continue;
                else
                    spawnPoint = spawnPoints[rand].position;
            }
        }
        RockGolem spawnedGolem = Instantiate(golemPrefab, spawnPoint, Quaternion.identity).GetComponent<RockGolem>();
        spawnedGolem.transform.position += Vector3.up * 100;    //Make sure golem is out of frame, so it can come crashing down like a majestic lit ass mofo 😎😎😎😎😎😎😎😎
        spawnedGolem.Initialise();

    }


}
