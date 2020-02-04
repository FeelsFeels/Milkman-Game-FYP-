using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        AudioPlayer audioPlayer = FindObjectOfType<AudioPlayer>();
        if (audioPlayer) audioPlayer.PlayMusic("MenuTrack");
    }
    private void Update()
    {

    }

    public void TitleScreenReady()
    {
        anim.Play("TitleScreen_ReadyToPlay");
    }

    public void LoadScene(string sceneName)

    {
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Play("Button Selected");
        }

        //       anim.Play("Screen_FadeIn");
        //     anim.SetTrigger("FadeOut");
        print("Fading out");
        SceneManager.LoadScene(sceneName);
//        anim.Play("Screen_FadeOut");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}