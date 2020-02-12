using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsScreenUINavigation : MonoBehaviour
{
    public EventSystem eventSystem;

    public GameObject[] selectableButtons;
    public int currentSelectedIndex = 0;
    public bool panelSelected;

    public bool directionHeld;
    public float timeHeldDirection = 0f;
    public float autoRepeatTimer = 0f;
    public float timeBeforeAutoRepeat;
    public float autoRepeatDelay;

    public string verticalInputString;
    public string selectionInputString;
    public string cancelInputString;

    AudioManager audioManager;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(selectableButtons[0]);
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        //Selecting the selection
        if (Input.GetButtonDown(selectionInputString))
        {
            if (!panelSelected)
            {
                Select();
                panelSelected = true;
            }
        }
        if (Input.GetButtonDown(cancelInputString))
        {
            if (panelSelected)
            {
                Cancel();
                panelSelected = false;
            }
            else
            {
                ReturnToMenu();
            }
        }

        //Moving the selection
        float verticalInput = Input.GetAxis(verticalInputString);
        if (verticalInput > -0.1f && verticalInput < 0.1f)
        {
            timeHeldDirection = 0f;
            autoRepeatTimer = 0f;
            directionHeld = false;
        }
        
        //Disable movement
        if (panelSelected)
            return;
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

    public void Select()
    {
        //selectableButtons[currentSelectedIndex].GetComponent<ControlsScreenUIPanel>().ShowPanel();
        selectableButtons[currentSelectedIndex].GetComponentInChildren<Button>().onClick.Invoke();
        if(audioManager)
            audioManager.Play("Button Selected");
    }

    public void Cancel()
    {
        selectableButtons[currentSelectedIndex].GetComponent<ControlsScreenUIPanel>().HidePanel();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
