using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public Button resumeBut;
    public Button optionsBut;
    public Button quitBut;

    Animator animator;

    GameManager gameManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AnimatePause()
    {
        animator.SetBool("Paused", true);
    }

    public void AnimateResume()
    {
        animator.SetBool("Paused", false);
    }

    //Called using OnButtonClicked
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }

    public void QuitGame(string mainMenuSceneString)
    {
        gameManager.LoadScene(mainMenuSceneString);
    }
}
