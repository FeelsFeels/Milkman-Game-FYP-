using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyOverTime : MonoBehaviour
{
    private float lifeTime;

    public float minTimeToDestroy;
    public float maxTimeToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        lifeTime = Random.Range(minTimeToDestroy, maxTimeToDestroy);

        Destroy(gameObject, lifeTime);
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
