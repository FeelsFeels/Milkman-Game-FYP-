using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnContact : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    string audioString;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager)
                audioManager.Play(audioString);
        }
    }
}
