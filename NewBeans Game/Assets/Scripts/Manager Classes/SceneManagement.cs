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
    }
    private void Update()
    {

    }

    public void TitleScreenReady()
    {
        // For loading MainMenu screen from TitleScreen only.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            anim.Play("TitleScreen_ReadyToPlay");
        }
    }

    public void LoadScene(string sceneName)

    {
 //       anim.Play("Screen_FadeIn");
        anim.SetTrigger("FadeOut");
        print("Fading out");
        SceneManager.LoadScene(sceneName);
//        anim.Play("Screen_FadeOut");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}