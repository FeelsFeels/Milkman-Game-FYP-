using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastManStandingUI : MonoBehaviour
{
    [SerializeField] PlayerInputInfo playerToCheckFor;

    public Text livesText;
    public Image livesBG;

    private void Awake()
    {
        livesText = transform.GetChild(1).GetComponent<Text>();
        livesBG = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateUI(PlayerInputInfo playerToUpdate, float livesRemaining)
    {
        if (playerToCheckFor != playerToUpdate)
            return;

        livesText.text = livesRemaining.ToString();
    }

    public void SetUIColor()
    {
        Color32 characterColor = playerToCheckFor.chosenCharacter.characterColor;
        livesBG.color = characterColor;
    }


}
