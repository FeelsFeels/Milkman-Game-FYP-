using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NewBeans.InstructionsScreen
{
    public class ControlsScreenScrollableUIPanel : MonoBehaviour
    {
        public bool isActive;

        public BaseSimulation[] simulations;
        public RectTransform[] panels;
        public int currentPanelIndex;
        public RectTransform panelSlideInRightStartPos;
        public RectTransform panelSlideInLeftStartPos;
        public RectTransform panelSlideInEndPos;
        Tween slideTween;

        public string horizontalInputString;
        public string selectionInputString;
        public string cancelInputString;

        private void Start()
        {
            slideTween = transform.DOMove(transform.position, 0);
        }

        private void Update()
        {
            if (!isActive)
                return;
            if (slideTween.IsActive())
                return;
            float horizontal = Input.GetAxis(horizontalInputString);

            if (horizontal > 0.1f)
            {
                ShowNextPanel();
            }
            if (horizontal < -0.1f)
            {
                ShowPreviousPanel();
            }
            if (Input.GetButtonDown(cancelInputString))
            {
                DeactivatePanel();
            }

        }

        public void ActivatePanel()
        {
            isActive = true;
        }
        public void DeactivatePanel()
        {
            isActive = false;
            if (currentPanelIndex == 0)
                return;

            panels[currentPanelIndex].anchoredPosition = new Vector2(panelSlideInRightStartPos.localPosition.x, panelSlideInRightStartPos.localPosition.y);
            currentPanelIndex = 0;
            panels[currentPanelIndex].anchoredPosition = new Vector2(panelSlideInRightStartPos.localPosition.x, panelSlideInRightStartPos.localPosition.y);
            slideTween = panels[currentPanelIndex].DOAnchorPos(new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y), 0.3f);
        }

        public void ShowNextPanel()
        {
            if (currentPanelIndex >= panels.Length - 1)
                return;

            //Stop simulation
            simulations[currentPanelIndex].ResetSimulation();

            RectTransform panelShown = panels[currentPanelIndex];
            currentPanelIndex++;

            ////Movement of the panel
            //Moving the old panel out.
            panelShown.anchoredPosition = new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y);
            panelShown.DOAnchorPos(new Vector2(panelSlideInLeftStartPos.localPosition.x, panelSlideInLeftStartPos.localPosition.y), 0.3f);
            //Moving the new panel inside
            panels[currentPanelIndex].anchoredPosition = new Vector2(panelSlideInRightStartPos.localPosition.x, panelSlideInRightStartPos.localPosition.y);
            slideTween = panels[currentPanelIndex].DOAnchorPos(new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y), 0.3f);

            //Play simulation
            simulations[currentPanelIndex].StartSimulation();
        }

        public void ShowPreviousPanel()
        {
            if (currentPanelIndex <= 0)
                return;

            //Stop simulation
            simulations[currentPanelIndex].ResetSimulation();

            RectTransform panelShown = panels[currentPanelIndex];
            currentPanelIndex--;

            ////Movement of the panel
            //Moving the old panel out.
            panelShown.anchoredPosition = new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y);
            panelShown.DOAnchorPos(new Vector2(panelSlideInRightStartPos.localPosition.x, panelSlideInRightStartPos.localPosition.y), 0.3f);
            //Moving the new panel inside
            panels[currentPanelIndex].anchoredPosition = new Vector2(panelSlideInLeftStartPos.localPosition.x, panelSlideInLeftStartPos.localPosition.y);
            slideTween = panels[currentPanelIndex].DOAnchorPos(new Vector2(panelSlideInEndPos.localPosition.x, panelSlideInEndPos.localPosition.y), 0.3f);

            //Play simulation
            simulations[currentPanelIndex].StartSimulation();
        }
    }
}