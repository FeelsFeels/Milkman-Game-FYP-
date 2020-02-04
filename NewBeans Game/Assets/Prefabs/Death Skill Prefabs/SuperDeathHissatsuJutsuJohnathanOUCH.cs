using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.SuperDeathHissatsuJustsus
{
    public class SuperDeathHissatsuJutsuJohnathanOUCH : MonoBehaviour
    {
        public AudioSource maleHissatsu;
        public AudioSource femaleHissatsu;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                maleHissatsu.Play();
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                femaleHissatsu.Play();
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                FindObjectOfType<AudioPlayer>().PlayMusic("EarthTrack");
            }
        }
    }
}
