using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class controls the timing in which platforms go down
/// 
/// </summary> 

public class EventsManager : MonoBehaviour
{
    public event Action OnNewPhase;
    public event Action OnSpawnHazard;

    private void Update()
    {
        //Hacks
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            NewPhase();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SpawnHazard();
        }
    }

    void NewPhase()
    {
        if(OnNewPhase != null)
        {
            OnNewPhase();
        }
    }

    void SpawnHazard()
    {
        if (OnSpawnHazard != null)
        {
            OnSpawnHazard();
        }
    }

}
