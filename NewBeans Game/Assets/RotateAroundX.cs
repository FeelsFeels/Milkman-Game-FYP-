using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundX : MonoBehaviour
{
    public float speed;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0);
    }
}
