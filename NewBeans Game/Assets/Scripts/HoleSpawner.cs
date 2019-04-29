using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{

    public GameObject holePrefab;

    //public GameObject[] newHole;

    public int maxHoles;
    static public int currentHoles;

    public float nextHoleSpawn;
    public float spawnCoolDown;

    public bool canSpawnHole = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHoles < maxHoles && canSpawnHole)
        {
            canSpawnHole = false;
            StartCoroutine("SpawnHole");
        }

        //if (currentHoles < maxHoles)
        //{
        //    Invoke("SpawnHole", 5);
        //}

    }

    public void CheckNumOfHoles()
    {
        //for (int i = 0; i < maxHoles; i++)
        //{
        //    if (currentHoles < maxHoles)
        //    {
        //        Invoke("SpawnHole", 1);
        //    }
        //}
        //if (currentHoles < maxHoles)
        //{
        ////    SpawnHole();
        //}
    }

    //// Randomly spawns a hole
    //public void SpawnHole()
    //{
    //    nextHoleSpawn = Time.time + spawnCoolDown;

    //    Vector3 randomSpawnPosition = new Vector3(Random.Range(-10f, 10f), -0.355f, Random.Range(-10f, 10f));

    //    if (currentHoles < maxHoles)
    //    {
    //        Instantiate(holePrefab, randomSpawnPosition, Quaternion.identity);
    //        currentHoles += 1;
    //    }
    //    //newHole.transform.parent = transform; // For Hierarchy neatness. All holes spawned will be a child of the HoleSpawner GameObject.
    //}

    IEnumerator SpawnHole()
    {
        yield return new WaitForSeconds(spawnCoolDown);
        print("Test");
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-5f, 20f), -1.1f, Random.Range(-25f, 0f));

        if (!canSpawnHole)
        {
            GameObject hole = Instantiate(holePrefab, randomSpawnPosition, Quaternion.identity);
            currentHoles += 1;
            canSpawnHole = true;
        }

    }
}