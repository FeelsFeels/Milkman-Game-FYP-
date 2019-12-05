using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource[] sources;
    public float fadeSpeed = 0.5f;
    public float fadeInOutMultiplier = 0.0f;
    public bool isPlaying;

    public string playingTrackName = "Nothing";
    public int playingTrackIndex;
    public float playingTrackVolume = 0.000f;

    public string lastTrackName = "Nothing";
    public int lastTrackIndex;
    public float lastTrackVolume = 0.000f;

    public IEnumerator FadeOutOldMusic_FadeInNewMusic()
    {
        sources[playingTrackIndex].volume = 0.000f;
        sources[playingTrackIndex].Play();
        while (sources[playingTrackIndex].volume < 1f)
        {
            sources[lastTrackIndex].volume -= fadeSpeed * 2;
            sources[playingTrackIndex].volume += fadeSpeed * 2;
            //Debug.Log("Fade: " + lastTrackName + " " + _audioSources[lastTrackIndex].volume.ToString() + " Rise: " + playingTrackName + " " + _audioSources[playingTrackIndex].volume.ToString());
            yield return new WaitForSeconds(0.001f);
            lastTrackVolume = sources[lastTrackIndex].volume;
            playingTrackVolume = sources[playingTrackIndex].volume;
        }
        sources[lastTrackIndex].volume = 0.000f;
        sources[lastTrackIndex].Stop();

        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    public IEnumerator FadeInNewMusic()
    {
        sources[playingTrackIndex].volume = 0.000f;
        sources[playingTrackIndex].Play();
        while (sources[playingTrackIndex].volume < 1f)
        {
            sources[playingTrackIndex].volume += fadeSpeed * 2;
            //Debug.Log("Fading In: " + _audioSources[track_index].volume.ToString());
            yield return new WaitForSeconds(0.001f);
            playingTrackVolume = sources[playingTrackIndex].volume;
        }
        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        sources = GetComponentsInChildren<AudioSource>();
    }

    public void PlayMusic(string transformName)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].name == transformName)
            {
                Debug.Log("Found Track Name (" + transformName + ") at Index(" + i.ToString() + ")");
                playingTrackIndex = i;
                playingTrackName = transformName;
                break;
            }
        }
        if (playingTrackIndex == lastTrackIndex)
        {
            Debug.Log("Same Track Selected");
            return;
        }
        else
        {
            if (isPlaying)
            {
                Debug.Log("Fading in new music - Fading out old music");
                StartCoroutine(FadeOutOldMusic_FadeInNewMusic());
            }
            else
            {
                Debug.Log("Fading in new music");
                StartCoroutine(FadeInNewMusic());
            }
        }
    }
}
