using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextBlink : MonoBehaviour
{
    private bool textShown = false;
    public float secBeforeNextBlink;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(secBeforeNextBlink);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(secBeforeNextBlink);
        }
    }
}
