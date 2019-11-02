using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodyhellshitUnity{
    public class ControllerInputTest : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (Input.GetButtonDown("AButton (Controller " + i + ")")) //if there's an A button input from one of the controllers
                {
                    print("Input: Controller " + i);
                }
            }
        }
    }
}