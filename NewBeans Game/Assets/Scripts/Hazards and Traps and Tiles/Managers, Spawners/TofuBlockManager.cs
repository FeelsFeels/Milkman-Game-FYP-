using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TofuBlockManager : MonoBehaviour
{
    public static TofuBlockManager instance = null;

    public Transform[] spawnLocations;

    public GameObject tofuPrefab;

    public float tofuDelayTime = 1f;

    public int startingTofuCount;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnStartingTofu());
    }



    // -------- This spawns the 5 tofu blocks at random positions at the start of the game.
    void SpawnTofu()
    {
        print("spawntofuuwu");
        HazardBoulder newTofu;
        if (spawnLocations.Length > 0)
        {
            bool canSpawn = false;
            int iterationCount = 0;
            Vector3 newSpawnPos = Vector3.up * 100; //Temp value to avoid compile error
            while (canSpawn == false)
            {
                if(iterationCount > 15)
                {
                    SpawnTofuWithDelay(10);
                    return;
                }

                newSpawnPos = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
                newSpawnPos.x += Random.Range(-5f, 5f);
                newSpawnPos.z += Random.Range(-5f, 5f);
                newSpawnPos.y = 100f;

                //raycast from newSpawnPos to check if it is over some shit
                RaycastHit hit;
                if (Physics.Raycast(newSpawnPos, Vector3.down, out hit, Mathf.Infinity))
                {
                    //If ground, continue trying to find another spot
                    if(hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    {
                        iterationCount++;
                        continue;
                    }
                    else
                    {
                        canSpawn = true;
                    }
                }
            }
            newTofu = Instantiate(tofuPrefab, newSpawnPos, Quaternion.Euler(-90, 0, 0)).GetComponent<HazardBoulder>();
        }
        else
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(35f, -20), 100f, Random.Range(-30f, 10f));
            newTofu = Instantiate(tofuPrefab, randomSpawnPosition, Quaternion.Euler(-90, 0, 0)).GetComponent<HazardBoulder>();
        }

        newTofu.Initialise();
    }

    // -------- This spawns 1 tofu at random position, but with a delay time.
    public void SpawnTofuWithDelay()
    {
        StartCoroutine(TofuSpawnWithDelay(0));
    }

    public void SpawnTofuWithDelay(float tofuDelayTime)
    {
        StartCoroutine(TofuSpawnWithDelay(tofuDelayTime));
    }

    IEnumerator TofuSpawnWithDelay(float tofuDelayTime)
    {
        yield return new WaitForSeconds(tofuDelayTime);
        SpawnTofu();
    }

    //Spawns starting tofy blocks
    IEnumerator SpawnStartingTofu()
    {
        for (int i = 0; i < startingTofuCount; i++)
        {
            SpawnTofu();
            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
    }
}
