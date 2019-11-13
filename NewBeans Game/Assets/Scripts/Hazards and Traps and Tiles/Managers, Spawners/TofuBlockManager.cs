using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TofuBlockManager : MonoBehaviour
{
    public static TofuBlockManager instance = null;

    public GameObject tofuPrefab;


    public float tofuDelayTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SpawnTofu();
        SpawnTofu();
        SpawnTofu();
        SpawnTofu();
        SpawnTofu();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    // -------- This spawns the 5 tofu blocks at random positions at the start of the game.
    public void SpawnTofu()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(35f, -20), 0.8f, Random.Range(-30f, 10f));

        GameObject newTofu = (GameObject)Instantiate(tofuPrefab, randomSpawnPosition, Quaternion.Euler(-90, 0, 0));
    }

    // -------- This spawns 1 tofu at random position, but with a delay time.
    public void SpawnTofuWithDelay()
    {
        StartCoroutine(TofuSpawnWithDelay());
    }

    IEnumerator TofuSpawnWithDelay()
    {
        yield return new WaitForSeconds(tofuDelayTime);
        SpawnTofu();
    }
}
