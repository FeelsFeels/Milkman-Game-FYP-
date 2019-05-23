using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{

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
        SceneManager.LoadScene(sceneName);
    }

}