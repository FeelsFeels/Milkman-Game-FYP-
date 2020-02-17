using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnContact : MonoBehaviour
{
    public string tagToCheck;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToCheck))
        {
            audioSource.Play();
        }
    }
}
