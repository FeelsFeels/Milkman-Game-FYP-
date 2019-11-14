using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For using special skills
/// </summary>

public class SkillSetManager : MonoBehaviour
{
    public enum characterChosen { Fire, Water, Lightning, Earth,};
    public characterChosen playerAvatar;
    public float fullChargeAmount‬ = 13195f;
    float currentCharge = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharge >= fullChargeAmount)
        {
            //check input
        }
    }

    //Set the character
    public void SetCharacter(string characterType)
    {
        switch (characterType)
        {
            case "Fire":
                {
                    playerAvatar = characterChosen.Fire;
                    break;
                }

            case "Water":
                {
                    playerAvatar = characterChosen.Water;
                    break;
                }

            case "Lightning":
                {
                    playerAvatar = characterChosen.Lightning;
                    break;
                }
            case "Earth":
                {
                    playerAvatar = characterChosen.Earth;
                    break;
                }
            default:
                {
                    Debug.Log("No character type, skills set to default: Fire");
                    break;
                }

        }
    }

    public void ChargeSpecialSkill(float projectileKnockback)
    {
        currentCharge += projectileKnockback;

        if (currentCharge > fullChargeAmount)
            currentCharge = fullChargeAmount; //Make sure current charge never exceedsss the max

        //Insert UI stuff here for charge bar


    }


    void ReleaseSpecialSkill()
    {
        // Release the kraken

    }


}
