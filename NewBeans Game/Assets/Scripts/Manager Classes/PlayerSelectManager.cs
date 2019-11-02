using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerSelectManager : MonoBehaviour
{
    public PlayerInputInfo[] playerInfoArray = new PlayerInputInfo[4];      //Player Input scriptable object
    public CharacterData[] characterDataArray = new CharacterData[4];       //Specific character data scriptable objects
    public ChooseCharacter[] chooseCharacterArray = new ChooseCharacter[4]; //Player choosing functionality script

    public List<int> playersJoined = new List<int>();   //Player numbers

    bool canStartGame;

    public string SceneToLoad;

    private void Update()  
    {
        for (int i = 1; i <= 5; i++) //check, for up to 4 controllers,
        {
            if (playersJoined.Contains(i))
            {
                continue;
            }

            if (Input.GetButtonDown("AButton (Controller " + i + ")")) //if there's an A button input from one of the controllers
            {
                print("new controller connect");
                playersJoined.Add(i);
                AddNewPlayer(i); //add a new controller and join the game
                break;
            }
        }

        if (canStartGame)
        {
            if (Input.GetButtonDown("AButton (Controller " + playersJoined.First() + ")"))  //Take only player 1's input to start game
            {
                SceneManager.LoadScene(SceneToLoad);
            }
        }
    }

    void AddNewPlayer(int controllerNumber)
    {
        int latestPlayer = playersJoined.Count - 1;    //Making controller number and playernumber unrelated.

        chooseCharacterArray[latestPlayer].playerInfo = playerInfoArray[latestPlayer];
        playerInfoArray[latestPlayer].SetInputStrings(controllerNumber);
        playerInfoArray[latestPlayer].chosenCharacterIndex = 0; //Setting as default character selection.
        

        chooseCharacterArray[latestPlayer].gameObject.SetActive(true);  //Allows player to start selecting character.
        CheckIfCanStartGame();
    }

    void RemovePlayer(int controllerNummber)
    {

    }

    public void ChangeCharacter(PlayerInputInfo playerToChange, int characterIndex)
    {
        playerToChange.chosenCharacterIndex = characterIndex;
        playerToChange.chosenCharacterData = characterDataArray[characterIndex];
        CheckIfCanStartGame();
    }

    void CheckIfCanStartGame()
    {
        //Checking if game is ready to start

        //Must be more than one player
        if (playersJoined.Count < 2)
            return;

        List<int> chosenCharacterIndexes = new List<int>();

        //If two people chose the same character, cannot start game
        for (int i = 0; i < playersJoined.Count; i++)
        {
            int chosenChar = playerInfoArray[i].chosenCharacterIndex;
            if (chosenCharacterIndexes.Contains(chosenChar))
            {
                canStartGame = false;
                return;
            }
            else
            {
                chosenCharacterIndexes.Add(chosenChar);
            }
        }

        canStartGame = true;

    }
}
