using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class controls the timing in which platforms go down
/// 
/// </summary> 

public class PlatformManager : MonoBehaviour
{
    public event Action OnNewPhase;

    private void Update()
    {
        //Hacks
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            NewPhase();
        }
    }

    void NewPhase()
    {
        if(OnNewPhase != null)
        {
            OnNewPhase();
        }
    }



}
