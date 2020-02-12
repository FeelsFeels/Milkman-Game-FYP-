using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThisMusicOnSceneStart : MonoBehaviour
{
    public string musicToPlay;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioPlayer>().PlayMusic(musicToPlay);   
    }
}
