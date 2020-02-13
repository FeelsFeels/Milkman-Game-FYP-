using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThisMusicOnSceneStart : MonoBehaviour
{
    public string musicToPlay;
    public bool stopAllSounds;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer audio = FindObjectOfType<AudioPlayer>();
        if (audio)
            audio.PlayMusic(musicToPlay);

        if (!stopAllSounds)
            return;

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager)
            audioManager.StopAllSounds();
    }
}
