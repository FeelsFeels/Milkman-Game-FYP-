using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateObject : MonoBehaviour
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        rectTransform.RotateAround(rectTransform.position, rectTransform.forward, 360 * Time.deltaTime);
    }
}
