using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class DebugSetPlayerData : MonoBehaviour
    {
        public PlayerInputInfo referencePlayer;

        public void ChangeControllerNumber(int number)
        {
            referencePlayer.SetInputStrings(number + 1);
        }

        public void ChangePlayerCharacter(int charIndex)
        {
            if (charIndex == 4)
            {
                referencePlayer.chosenCharacterData = null;
            }
            else
            {
                referencePlayer.chosenCharacterIndex = charIndex;
                referencePlayer.chosenCharacterData = DebugSetPlayerDataManager.instance.characters[charIndex];
            }
        }

        public void SetToggle(bool toggle)
        {
            referencePlayer.forceActive = toggle;
        }
    }
}
