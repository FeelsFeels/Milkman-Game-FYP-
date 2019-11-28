using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastManStandingUI : MonoBehaviour
{
    [SerializeField] PlayerInputInfo trackedPlayer;

    public GameObject characterUI;
    //public Text livesText;
    public Slider ultiCharge;
    public bool UIOnLeft;

    private void Awake()
    {
        //livesText = transform.GetChild(1).GetComponent<Text>();
        //livesBG = transform.GetChild(0).GetComponent<Image>();

        //Check if tracked player is playing the game

        //if (trackedPlayer.forceActive)
        //{
        //    characterUI.sprite = trackedPlayer.chosenCharacterData.characterUISprite;
        //    gameObject.SetActive(true);
        //}
        //else if(trackedPlayer.chosenCharacterData == null)
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    characterUI.sprite = trackedPlayer.chosenCharacterData.characterUISprite;
        //    gameObject.SetActive(true);
        //}

        if (trackedPlayer.chosenCharacterData != null)
        {
            if (trackedPlayer.playerNumber % 2 == 0) //If even,
            {
                //Use Right UI
                characterUI = Instantiate(trackedPlayer.chosenCharacterData.characterUI_Right, transform) as GameObject; //May wanna change this later on to just set active...
            }
            else //If odd,
            {
                //Use Left UI
                characterUI = Instantiate(trackedPlayer.chosenCharacterData.characterUI_Left, transform) as GameObject;
                ultiCharge = characterUI.transform.Find("Ulti_Charge").GetComponent<Slider>();
            }
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);

    }

    public void UpdateUI(PlayerInputInfo playerToUpdate, float livesRemaining)
    {
        if (trackedPlayer != playerToUpdate)
            return;

        //livesText.text = livesRemaining.ToString();

        //Sprite update
        Transform livesContainer = characterUI.transform.Find("Lives_Sprites");
        //Debug.Log(livesContainer);
         if(livesContainer.childCount > livesRemaining)
         {
            for (int i = livesContainer.childCount-1;  i > livesRemaining-1; i--)
            {
                livesContainer.GetChild(i).gameObject.SetActive(false);
            }
         }

        if (livesRemaining == 0) //Darken the images (not really greyscale) when player is out
        {
            Image[] playerImages = characterUI.GetComponentsInChildren<Image>();
            Debug.Log(playerImages.Length);
            foreach (Image img in playerImages)
            {
                img.color = new Color32(58, 58, 58, 255);
            }
        }


    }
     
    //public void SetUIBackground()
    //{
    //    characterUI.sprite = trackedPlayer.chosenCharacterData.characterUISprite;
    //}

    /// *********************************
    /// Update tracked player UI
    /// *********************************
    public void UpdateUltiUI(PlayerInputInfo playerToUpdate, float percent)
    {
        if (trackedPlayer != playerToUpdate)
            return;

        if(ultiCharge != null)
        ultiCharge.value = percent;
    }


}
