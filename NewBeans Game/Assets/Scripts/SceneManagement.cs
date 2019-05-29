using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{

    public Animator anim;

    private void Start()
    {

    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        
    }

    public void LoadScene(string sceneName)

    {
        anim.SetTrigger("FadeOut");
        print("Fading out");
        SceneManager.LoadScene(sceneName);
    }

}