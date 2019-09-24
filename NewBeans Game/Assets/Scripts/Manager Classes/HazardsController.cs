using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardsController : MonoBehaviour
{
    public List<GameObject> hazardList = new List<GameObject>();

    private EventsManager eventsManager;

    public Transform centreStage;
    public float spawnRadius;

    private void Awake()
    {
        eventsManager = FindObjectOfType<EventsManager>();
    }

    private void Start()
    {
        if(eventsManager)
            eventsManager.OnSpawnHazard += SpawnHazard;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            SpawnHazard();
    }

    public void SpawnHazard()
    {
        Instantiate(GetRandomHazard(), FindRandomPosition(), Quaternion.identity);
    }

    private Vector3 FindRandomPosition()
    {
        Vector3 spawnPosition;
        spawnPosition = centreStage.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 50, Random.Range(-spawnRadius, spawnRadius));
        return spawnPosition;
    }

    private GameObject GetRandomHazard()
    {
        return hazardList[Random.Range(0, hazardList.Count)];        
    }

}
