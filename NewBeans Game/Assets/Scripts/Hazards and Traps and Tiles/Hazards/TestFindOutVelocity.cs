using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFindOutVelocity : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>())
            print(GetComponent<Rigidbody>().velocity);
    }
}
