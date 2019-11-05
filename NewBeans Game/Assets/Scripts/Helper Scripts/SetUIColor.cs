using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIColor : MonoBehaviour
{
    public Image[] colorsToChange;

    public PlayerInputInfo colorToReference;
    Color32 colorToUse;

    private void Awake()
    {
        if(colorToReference.chosenCharacterData)
            colorToUse = colorToReference.chosenCharacterData.characterColor;
    }

    private void Start()
    {
        foreach(Image image in colorsToChange)
        {
            if(colorToReference.chosenCharacterData)
                image.color = colorToUse;
        }
    }

}
