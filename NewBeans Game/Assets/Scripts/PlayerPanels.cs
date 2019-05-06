using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanels : MonoBehaviour
{
    public PlayerController Player;
    public int PlayerNumber;
    public Text Header;
    public GameObject PressToJoin;

    public bool HasControllerAssigned = false;
    public int controllerNumber;
    public GameObject CharacterSelect;
    public Scrollbar Choices;
    public SetControllerToPlayer set;
    public bool PlayerIsReady = false;

    // Start is called before the first frame update
    void Awake()
    {
        set = FindObjectOfType<SetControllerToPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasControllerAssigned == true && PlayerIsReady != true)
        {
            CharacterSelect.SetActive(true);

        }

        if (CharacterSelect.activeInHierarchy)
        {
            //Choices = GetComponentInChildren<Scrollbar>();
            //if (Input.GetAxisRaw("Horizontal (Controller "+ controllerNumber + ")") > 0)
            //{
            //    Choices.value += 0.25f;
            //}
            //if (Input.GetAxisRaw("Horizontal (Controller " + controllerNumber + ")") < 0)
            //{
            //    Choices.value -= 0.25f;
            //}

            if (Input.GetButton("AButton (Controller " + controllerNumber + ")"))
            {
                set.PlayersReady++;
                CharacterSelect.SetActive(false);
                PlayerIsReady = true;
            }

        }

    }

    public void AssignController (int controllerNo)
    {
        Debug.Log("Setting player to controller");
        Player.SetControllerNumber(controllerNo);
        PressToJoin.SetActive(false);
        HasControllerAssigned = true;
        controllerNumber = controllerNo;

    }


}
