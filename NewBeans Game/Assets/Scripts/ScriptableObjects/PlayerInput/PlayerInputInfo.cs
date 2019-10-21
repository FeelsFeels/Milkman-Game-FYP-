using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
public class PlayerInputInfo : ScriptableObject
{
    public int ControllerNumber;
    public string HorizontalInputAxis;
    public string VerticalInputAxis;
    public string AButtonInput;
    public string BButtonInput;

    public int chosenCharacterIndex;

    public void SetInputStrings(int controllerNumber)
    {
        ControllerNumber = controllerNumber;
        HorizontalInputAxis = string.Format("Horizontal (Controller {0})", controllerNumber);
        VerticalInputAxis = string.Format("Vertical (Controller {0})", controllerNumber);
        AButtonInput = string.Format("AButton (Controller {0})", controllerNumber);
        BButtonInput = string.Format("BButton (Controller {0})", controllerNumber);
    }
}
