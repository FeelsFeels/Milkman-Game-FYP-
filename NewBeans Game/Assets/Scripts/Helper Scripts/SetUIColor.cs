using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIColor : MonoBehaviour
{
    public Image[] colorsToChange;

    public PlayerInputInfo colorToReference;
    public Color32 colorToUse;

    private void Awake()
    {
        PlayerController pc = GetComponentInParent<PlayerController>();
        if (pc)
        {
            if (pc.inputInfo != null)
                colorToReference = pc.inputInfo;
        }

        if (colorToReference.chosenCharacterData != null)
            colorToUse = colorToReference.chosenCharacterData.characterColor;
    }

    private void Start()
    {
        foreach(Image image in colorsToChange)
        {
            image.color = colorToUse;
        }
    }

}
