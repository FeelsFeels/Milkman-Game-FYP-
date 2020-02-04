using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NewBeans.InstructionsScreen;

public class ControlsScreenUIPanel : MonoBehaviour
{
    public RectTransform panel;
    public RectTransform panelSlideInStartPos;
    public RectTransform panelSlideInEndPos;

    public BaseSimulation simulation;

    public void ShowPanel()
    {
        if (!panel)
        {
            Debug.LogError("Warning! No Panel Attached!");
            return;
        }
        panel.position = panelSlideInStartPos.position;
        panel.localScale = Vector3.one;
        panel.DOAnchorPos(new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y), 0.5f);
        simulation.StartSimulation();
    }

    public void HidePanel()
    {
        simulation.ResetSimulation();
        panel.DOScale(0f, 0.3f);
    }
}
