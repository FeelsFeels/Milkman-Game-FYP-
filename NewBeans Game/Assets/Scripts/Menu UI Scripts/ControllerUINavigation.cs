using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerUINavigation : MonoBehaviour
{
    public EventSystem eventSystem;

    public GameObject[] selectableButtons;
    public int currentSelectedIndex = 0;

    public bool directionHeld;
    public float timeHeldDirection = 0f;
    public float autoRepeatTimer = 0f;
    public float timeBeforeAutoRepeat;
    public float autoRepeatDelay;

    public string verticalInputString;
    public string selectionInputString;
    public string cancelInputString;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        //eventSystem.SetSelectedGameObject(selectableButtons[0]);
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis(verticalInputString);
        if (verticalInput > -0.1f && verticalInput < 0.1f)
        {
            timeHeldDirection = 0f;
            autoRepeatTimer = 0f;
            directionHeld = false;
        }
        if (verticalInput > 0.1f)
        {
            if (!directionHeld)
            {
                MoveNavigationDown();
                directionHeld = true;
            }
            if (directionHeld)
            {
                timeHeldDirection += Time.deltaTime;
                if (timeHeldDirection >= timeBeforeAutoRepeat)
                {
                    autoRepeatTimer += Time.deltaTime;
                    if (autoRepeatTimer >= autoRepeatDelay)
                    {
                        MoveNavigationDown();
                        autoRepeatTimer = 0f;
                    }
                }
            }
        }
        if (verticalInput < -0.1f)
        {
            if (!directionHeld)
            {
                MoveNavigationUp();
                directionHeld = true;
            }
            if (directionHeld)
            {
                timeHeldDirection += Time.deltaTime;
                if (timeHeldDirection >= timeBeforeAutoRepeat)
                {
                    autoRepeatTimer += Time.deltaTime;
                    if (autoRepeatTimer >= autoRepeatDelay)
                    {
                        MoveNavigationUp();
                        autoRepeatTimer = 0f;
                    }
                }
            }
        }
        if (Input.GetButtonDown(selectionInputString))
        {
            selectableButtons[currentSelectedIndex].GetComponent<Button>().onClick.Invoke();
        }

    }

    public void MoveNavigationDown()
    {
        if (currentSelectedIndex >= selectableButtons.Length - 1)
            return;

        currentSelectedIndex++;
        eventSystem.SetSelectedGameObject(selectableButtons[currentSelectedIndex]);
    }
    public void MoveNavigationUp()
    {
        if (currentSelectedIndex <= 0)
            return;

        currentSelectedIndex--;
        eventSystem.SetSelectedGameObject(selectableButtons[currentSelectedIndex]);
    }
}
