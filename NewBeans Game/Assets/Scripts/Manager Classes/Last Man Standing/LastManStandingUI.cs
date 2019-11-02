using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastManStandingUI : MonoBehaviour
{
    [SerializeField] PlayerInputInfo trackedPlayer;

    public Image livesBG;
    public Text livesText;

    private void Awake()
    {
        livesText = transform.GetChild(1).GetComponent<Text>();
        livesBG = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateUI(PlayerInputInfo playerToUpdate, float livesRemaining)
    {
        if (trackedPlayer != playerToUpdate)
            return;

        livesText.text = livesRemaining.ToString();
    }
     
    public void SetUIBackground()
    {
        livesBG.sprite = trackedPlayer.chosenCharacterData.characterUISprite;
    }


}
