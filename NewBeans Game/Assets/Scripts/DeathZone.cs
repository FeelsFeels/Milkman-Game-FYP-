using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public GameObject[] holeVar;
    public int currentHoleIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchHoles();
        }
    }

    void SwitchHoles()
    {
        holeVar[currentHoleIndex].SetActive(false);
        if(currentHoleIndex + 1 < holeVar.Length)
        {
            currentHoleIndex++;
        }
        else if (currentHoleIndex + 1 >= holeVar.Length)
        {
            currentHoleIndex = 0;
        }
        holeVar[currentHoleIndex].SetActive(true);
    }
}
