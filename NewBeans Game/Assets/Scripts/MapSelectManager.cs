using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectManager : MonoBehaviour
{
    public int mapNumber;
    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mapNumber = 1;
    }

    public void RightButtonPressed()
    {
        if (mapNumber == 1)
        {
            anim.Play("MapSelect_1_to_2");
            mapNumber = 2;
            return;
        }

        if (mapNumber == 2)
        {
            anim.Play("MapSelect_2_to_3");
            mapNumber = 3;
            return;
        }

        if (mapNumber == 3)
        {
            anim.Play("MapSelect_3_to_4");
            mapNumber = 4;
            return;
        }
    }

    public void LeftButtonPressed()
    {
        if (mapNumber == 2)
        {
            anim.Play("MapSelect_2_to_1");
            mapNumber = 1;
            return;
        }
        if (mapNumber == 3)
        {
            anim.Play("MapSelect_3_to_2");
            mapNumber = 2;
            return;
        }
        if (mapNumber == 4)
        {
            anim.Play("MapSelect_4_to_3");
            mapNumber = 3;
            return;
        }

    }
}