using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerSelectManager : MonoBehaviour
{
    public PlayerInputInfo[] playerInfoArray = new PlayerInputInfo[4];      //Player Input scriptable object
    public CharacterData[] characterDataArray = new CharacterData[4];       //Specific character data scriptable objects
    public ChooseCharacter[] chooseCharacterArray = new ChooseCharacter[4]; //Player choosing functionality script

    public Image[] characterBorders = new Image[4];
    public Sprite[] characterBorderSprites = new Sprite[5];
    public Image[] characterPortraits = new Image[4];
    public Sprite[] characterPortraitSprites = new Sprite[8];

    public List<int> playersJoined = new List<int>();   //Player numbers

    bool canStartGame;

    public string SceneToLoad;


    private void Awake()
    {
        //Deselects all characters
        foreach(PlayerInputInfo player in playerInfoArray)
        {
            player.chosenCharacterData = null;
            player.chosenCharacterIndex = 0;
        }
    }

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

    public bool ChooseCharacter(PlayerInputInfo playerToChange, int characterIndex)
    {
        playerToChange.chosenCharacterIndex = characterIndex;
        playerToChange.chosenCharacterData = characterDataArray[characterIndex];

        //characterBorders[characterIndex].color = characterDataArray[characterIndex].characterColor;
        characterBorders[characterIndex].sprite = characterBorderSprites[characterIndex + 1];
        characterPortraits[characterIndex].sprite = characterPortraitSprites[characterIndex + 4];

        CheckIfCanStartGame();
        return true;
    }

    public void UnchooseCharacter(PlayerInputInfo playerToChange, int characterIndex)
    {
        //characterBorders[characterIndex].color = Color.white;
        characterBorders[characterIndex].sprite = characterBorderSprites[0];
        characterPortraits[characterIndex].sprite = characterPortraitSprites[characterIndex];
        playerToChange.chosenCharacterData = null;

        CheckIfCanStartGame();
    }

    public bool CheckIfCharacterTaken(PlayerInputInfo playerToCheck, int characterIndex)
    {
        //Check if character was already chosen
        bool characterTaken = false;

        if (playersJoined.Count < 2)
            return true;

        for (int i = 0; i < playersJoined.Count; i++)
        {
            //This player has not picked a character. Skip his ass
            if (playerInfoArray[i].chosenCharacterData == null)
                continue;

            int chosenCharIndex = playerInfoArray[i].chosenCharacterIndex;
            if (chosenCharIndex == characterIndex)
            {
                characterTaken = true;
                break;
            }
            else
            {
                characterTaken = false;
                continue;
            }
        }

        if (characterTaken)
            return false;
        else
            return true;
    }

    void CheckIfCanStartGame()
    {

        //Must be more than one player
        if (playersJoined.Count < 2)
            return;

        List<int> chosenCharacterIndexes = new List<int>();

        //If two people chose the same character, cannot start game
        for (int i = 0; i < playersJoined.Count; i++)
        {
            if(playerInfoArray[i].chosenCharacterData == null)
            {
                //A player has not chosen a character. Cannot start game.
                canStartGame = false;
                return;
            }

            //Checking if two people chose the same character
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
