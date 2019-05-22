using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetControllerToPlayer : MonoBehaviour
{
    public List<int> assignedControllers = new List<int>();
    //public int assignedControllers = 0;
    public PlayerPanels[] playerPanels;

    public GameObject PressToPlay;
    public string startGameScene;

    public int PlayersReady;
    public bool ReadyToStart = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerPanels = FindObjectsOfType<PlayerPanels>().OrderBy(t => t.PlayerNumber).ToArray(); //find the player panel selection
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i<=4; i++) //check, for up to 4 controllers,
        {
            if (assignedControllers.Contains(i))
            {
                continue; //if it contains this shit, then skip the number
            }

            if (Input.GetButton("AButton (Controller " + i + ")")) //if there's an A button input from one of the controllers
            {
                AddPlayerController(i); //add a new controller and join the game
                break;
            }
           
        }

        if (assignedControllers.Count >= 2 && PlayersReady == assignedControllers.Count)
        {
            PressToPlay.SetActive(true); //if more than 2 assigned controllers, tell players that game can be started

        }

        if (ReadyToStart)
        {
            if (Input.GetButton("AButton (Controller " + i + ")")
                {
                SceneManager.LoadScene(startGameScene);
            }
        }
    }


    public void AddPlayerController(int controllerNo)
    {
        assignedControllers.Add(controllerNo);
        //assignedControllers++; //count no of controllers that are assigned
        for (int i = 0; i < playerPanels.Length; i++)
        {
            if (playerPanels[i].HasControllerAssigned == false) //if there are no controllers assigned to that player panel, then
            {
                playerPanels[i].AssignController(controllerNo); //assign a controller to that player
                break;
            }
        }


    }

}
