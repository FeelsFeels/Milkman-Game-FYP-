using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyOverTime : MonoBehaviour
{
    public bool chooseInInspector;
    public float chosenTime;

    private void Start()
    {
        if (chooseInInspector)
        {
            CancelInvoke("DestroyThis");
            Invoke("DestroyThis", chosenTime);
        }
    }

    void OnEnable()
    {
        // This calls DestroyThis function after default 5 seconds;
        Invoke("DestroyThis", 5f);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
        HoleSpawner.currentHoles -= 1;
    }

    public void DestroyWithTime(float timeToDestroy)
    {

        CancelInvoke("DestroyThis");
        //Use a new destroy invoker
        Invoke("DestroyThis", timeToDestroy);
    }
}
