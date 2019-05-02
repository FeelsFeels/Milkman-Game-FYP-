using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    public float secondsTillActivation;

    private Renderer rend;

    public Material deactivatedMaterial;
    public Material activatedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        gameObject.GetComponent<Renderer>().material = deactivatedMaterial;
        StartCoroutine(ActivateHole());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ActivateHole()
    {
        yield return new WaitForSeconds(secondsTillActivation);
        gameObject.GetComponent<Renderer>().material = activatedMaterial;
        gameObject.tag = "Hole";
    }
}
