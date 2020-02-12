using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerSelectBehaviour : MonoBehaviour
{

    public Animator animator;

    [Space]

    public PlayerInputInfo[] playerInfoArray = new PlayerInputInfo[4];      //Player Input scriptable object
    public CharacterData[] characterDataArray = new CharacterData[4];       //Specific character data scriptable objects
    public PlayerSelectCharacterSelector[] characterSelection = new PlayerSelectCharacterSelector[4];

    public GameObject[] pressToJoinGame = new GameObject[4];
    public Image[] selectionBorders = new Image[4];
    public Image[] selectionOverlays = new Image[4];
    public Image[] playerNumsImage = new Image[16];
    public Image[] playerChosenCharPortraits = new Image[16];
    public GameObject[] playerChosenCharNames = new GameObject[16];

    public Dictionary<PlayerSelectCharacterSelector, int> selectorHoverOver = new Dictionary<PlayerSelectCharacterSelector, int>();
    public Dictionary<PlayerSelectCharacterSelector, int> playerCharacterChoices = new Dictionary<PlayerSelectCharacterSelector, int>();

    public List<int> playersJoined = new List<int>();   //Player numbers

    public bool canStartGame;

    public string SceneToLoad;


    private void Awake()
    {
        //Deselects all characters
        foreach (PlayerInputInfo player in playerInfoArray)
        {
            player.chosenCharacterData = null;
            player.chosenCharacterIndex = 0;
            player.forceActive = false;
        }

        //Disables all graphics
        for (int i = 0; i < 4; i++)
        {
            pressToJoinGame[i].SetActive(true);
            selectionBorders[i].GetComponent<Image>().enabled = false;
            selectionOverlays[i].GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < 16; i++)
        {
            playerNumsImage[i].GetComponent<Image>().enabled = false;
            playerChosenCharPortraits[i].GetComponent<Image>().enabled = false;
            playerChosenCharNames[i].SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 1; i <= 4; i++) //check, for up to 4 controllers,
        {
            if (playersJoined.Contains(i))
            {
                continue;
            }

            if (Input.GetButtonDown("AButton (Controller " + i + ")")) //if there's an A button input from one of the controllers
            {
                playersJoined.Add(i);
                AddNewPlayer(i); //add a new controller and join the game
                break;
            }
        }

        if (canStartGame)
        {
            if (Input.GetButtonDown("AButton (Controller " + playersJoined.First() + ")"))  //Take only player 1's input to start game
            {
                SceneManagement sceneManagement = FindObjectOfType<SceneManagement>();
                if (sceneManagement)
                {
                    sceneManagement.LoadSceneWithLoadingScreen(SceneToLoad);
                }
                else
                    SceneManager.LoadScene(SceneToLoad);
            }
        }
    }



    public void UpdateUI()
    {
        int numPlayers = playersJoined.Count;


        //This block disables all the selection borders
        for (int i = 0; i < 4; i++)
        {
            selectionBorders[i].enabled = false;
        }

        //This block sets the player number indicator and border
        for (int i = 0; i < numPlayers; i++)
        {
            //Disabling all graphics
            for (int p = 0; p < 4; p++)
            {
                playerNumsImage[(i * 4) + p].enabled = false;
            }

            int hoveredChar = characterSelection[i].charHoverOverIndex;
            //enables the player selection border
            selectionBorders[hoveredChar].enabled = true;
            //enables the number
            playerNumsImage[(i * 4) + hoveredChar].enabled = true;
        }
    }

    void AddNewPlayer(int controllerNumber)
    {
        print("New player has arrived! Controller " + controllerNumber);
        int latestPlayer = playersJoined.Count - 1;    //Making controller number and playernumber unrelated.

        pressToJoinGame[latestPlayer].SetActive(false);

        characterSelection[latestPlayer].PlayerJoined(playerInfoArray[latestPlayer]);
        characterSelection[latestPlayer].charHoverOverIndex = 0;
        playerInfoArray[latestPlayer].SetInputStrings(controllerNumber);
        playerInfoArray[latestPlayer].chosenCharacterIndex = 0; //Setting as default character selection
        UpdateUI();
        CheckIfCanStartGame();
    }

    void RemovePlayer(int controllerNummber)
    {

    }

    public void ChooseCharacter(PlayerInputInfo playerToChange, int characterIndex)
    {
        playerToChange.chosenCharacterIndex = characterIndex;
        playerToChange.chosenCharacterData = characterDataArray[characterIndex];

        //print(characterIndex);
        selectionOverlays[characterIndex].enabled = true;
        //print(selectionOverlays[characterIndex].name);
        //print(((playerToChange.playerNumber - 1) * 4) + characterIndex);
        playerChosenCharPortraits[(playerToChange.playerNumber - 1) * 4 + characterIndex].enabled = true;
        playerChosenCharNames[(playerToChange.playerNumber - 1) * 4 + characterIndex].SetActive(true);
        UpdateUI();
        CheckIfCanStartGame();
    }

    public void UnchooseCharacter(PlayerInputInfo playerToChange, int characterIndex)
    {
        playerToChange.chosenCharacterData = null;

        selectionOverlays[characterIndex].enabled = false;
        playerChosenCharPortraits[(playerToChange.playerNumber - 1) * 4 + characterIndex].enabled = false;
        playerChosenCharNames[(playerToChange.playerNumber - 1) * 4 + characterIndex].SetActive(false);

        UpdateUI();
        CheckIfCanStartGame();
    }

    public bool CheckIfCharacterTaken(PlayerInputInfo playerToCheck, int characterIndex)
    {
        //Check if character was already chosen
        bool characterTaken = false;

        if (playersJoined.Count < 2)
            return false;


        for (int i = 0; i < playersJoined.Count; i++)
        {
            //This player has not picked a character. Skip his ass
            if (playerInfoArray[i].chosenCharacterData == null)
                continue;

            int chosenCharIndex = playerInfoArray[i].chosenCharacterIndex;
            if (chosenCharIndex == characterIndex)
            {
                print("Taken: " + chosenCharIndex + " by player:" + i);
                characterTaken = true;
                break;
            }
            else
            {
                //print("character not taken, next loop");
                characterTaken = false;
                continue;
            }
        }

        if (characterTaken)
            return true;
        else
            return false;
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
            if (playerInfoArray[i].chosenCharacterData == null)
            {
                //A player has not chosen a character. Cannot start game.
                canStartGame = false;
                animator.SetBool("ReadyToStart", canStartGame);
                return;
            }

            //Checking if two people chose the same character
            int chosenChar = playerInfoArray[i].chosenCharacterIndex;
            if (chosenCharacterIndexes.Contains(chosenChar))
            {
                canStartGame = false;
                animator.SetBool("ReadyToStart", canStartGame);
                return;
            }
            else
            {
                chosenCharacterIndexes.Add(chosenChar);
            }
        }

        canStartGame = true;

        animator.SetBool("ReadyToStart", canStartGame);

    }
}
