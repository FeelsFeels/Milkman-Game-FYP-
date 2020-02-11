using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{

    public Animator anim;

    [Header("Loading Scenes")]
    public GameObject loadingPanel;

    private void Start()
    {
        anim = GetComponent<Animator>();
        AudioPlayer audioPlayer = FindObjectOfType<AudioPlayer>();
        if (audioPlayer) audioPlayer.PlayMusic("MenuTrack");
    }

    //Old
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
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        if (!loadingPanel)
        {
            LoadScene(sceneName);
            return;
        }
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}