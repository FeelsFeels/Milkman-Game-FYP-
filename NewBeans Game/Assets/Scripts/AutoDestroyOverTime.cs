using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyOverTime : MonoBehaviour
{
    private float lifeTime;

    public float minTimeToDestroy;
    public float maxTimeToDestroy;

    private bool isDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        lifeTime = Random.Range(minTimeToDestroy, maxTimeToDestroy);

        // This calls DestroyThis function after 'lifetime' seconds
        Invoke("DestroyThis", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyThis()
    {
        Destroy(gameObject);
        HoleSpawner.currentHoles -= 1;
    }
}
