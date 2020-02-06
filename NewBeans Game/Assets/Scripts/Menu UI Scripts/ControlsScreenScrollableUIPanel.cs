using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ControlsScreenScrollableUIPanel : MonoBehaviour
{
    public bool isActive;
    public bool transitioning;

    public RectTransform[] panels;
    public int currentPanelIndex;
    public RectTransform panelSlideInRightStartPos;
    public RectTransform panelSlideInLeftStartPos;
    public RectTransform panelSlideInEndPos;
    Tween slideTween;

    public string horizontalInputString;
    public string selectionInputString;
    public string cancelInputString;

    private void Update()
    {
        
    }

    public void ShowNextPanel()
    {
        if (currentPanelIndex >= panels.Length - 1)
            return;

        currentPanelIndex++;
        //Movement of the panel
    }

    public void ShowPreviousPanel()
    {
        if (currentPanelIndex <= 0)
            return;

        currentPanelIndex--;
        //Movement of the panel

    }

}
