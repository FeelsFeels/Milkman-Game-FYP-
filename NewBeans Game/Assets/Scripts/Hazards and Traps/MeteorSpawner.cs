using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public GameObject meteorPrefab;
    public float meteorMoveSpeed;

    public float firstSpawn;
    public float spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMeteor", firstSpawn, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMeteor()
    {
        Instantiate(meteorPrefab, gameObject.transform.position, Quaternion.identity);

    }
    
}
