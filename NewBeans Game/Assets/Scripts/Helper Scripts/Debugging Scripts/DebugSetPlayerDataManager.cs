using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Debugging
{
    public class DebugSetPlayerDataManager : MonoBehaviour
    {
        public static DebugSetPlayerDataManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public CharacterData[] characters = new CharacterData[4];

        public void GOTOSCENE(string sceneFUckingName)
        {
            SceneManager.LoadScene(sceneFUckingName);
        }
    }
}
