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
    public string RightHorizontalAxis;
    public string RightVerticalAxis;
    public string RightBumper;

    public int chosenCharacterIndex;
    public CharacterData chosenCharacter;

    public void SetInputStrings(int controllerNumber)
    {
        ControllerNumber = controllerNumber;
        HorizontalInputAxis = string.Format("Horizontal (Controller {0})", controllerNumber);
        VerticalInputAxis = string.Format("Vertical (Controller {0})", controllerNumber);
        AButtonInput = string.Format("AButton (Controller {0})", controllerNumber);
        BButtonInput = string.Format("BButton (Controller {0})", controllerNumber);

        RightHorizontalAxis = string.Format("RightHorizontal (Controller {0})", controllerNumber);
        RightVerticalAxis = string.Format("RightVertical (Controller {0})", controllerNumber);
        RightBumper = string.Format("RightBumper (Controller {0})", controllerNumber);
    }
}
