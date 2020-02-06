using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewBeans.InstructionsScreen
{
    public class AIControlsScreenUI : MonoBehaviour
    {
        [SerializeField] PlayerInputInfo trackedPlayer;

        public GameObject characterUI;
        //public Text livesText;
        public Slider ultiCharge;
        public GameObject auraObj;


        private void Start()
        {
            UpdateUltiUI(trackedPlayer, 100);
        }

        //public void UpdateUI(PlayerInputInfo playerToUpdate, float livesRemaining)
        //{
        //    if (trackedPlayer != playerToUpdate)
        //        return;

        //    //livesText.text = livesRemaining.ToString();

        //    //Sprite update
        //    Transform livesContainer = characterUI.transform.Find("Lives_Sprites");
        //    //Debug.Log(livesContainer);
        //    if (livesContainer.childCount > livesRemaining)
        //    {
        //        for (int i = livesContainer.childCount - 1; i > livesRemaining - 1; i--)
        //        {
        //            livesContainer.GetChild(i).gameObject.SetActive(false);
        //        }
        //    }

        //    if (livesRemaining == 0) //Darken the images (not really greyscale) when player is out
        //    {
        //        Image[] playerImages = characterUI.GetComponentsInChildren<Image>();
        //        foreach (Image img in playerImages)
        //        {
        //            img.color = new Color32(58, 58, 58, 255);
        //        }
        //    }
        //}

        /// *********************************
        /// Update tracked player UI
        /// *********************************
        public void UpdateUltiUI(PlayerInputInfo playerToUpdate, float percent)
        {
            if (trackedPlayer != playerToUpdate)
                return;

            if (ultiCharge != null)
                ultiCharge.value = percent;

            if (ultiCharge.value == 1) //if max charge for ulti
            {
                if (auraObj != null)
                {
                    auraObj.SetActive(true); //show aura
                }
            }
            else //otherwise set aura false
            {
                if (auraObj != null)
                {
                    auraObj.SetActive(false);
                }
            }
        }
        public void FullCharge()
        {
            UpdateUltiUI(trackedPlayer, 1);
        }
        public void UseUlti()
        {
            UpdateUltiUI(trackedPlayer, 0);
        }
    }
}