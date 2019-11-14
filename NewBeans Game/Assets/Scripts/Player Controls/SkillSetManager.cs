using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For using special skills
/// </summary>

public class SkillSetManager : MonoBehaviour
{
    public enum characterChosen { Fire, Water, Lightning, Earth,};
    [Header("Charging Ultimate Skill Settings")]
    public characterChosen playerAvatar;
    public float fullChargeAmount‬ = 13195f;
    float currentCharge = 0;
    string AButtonInput;
    string BButtonInput;

    [Header("UI")]
    public Image chargeBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharge >= fullChargeAmount)
        {
            //Insert visual feedback that my ultimate form is ready to show


            //Check inputs
            if(Input.GetButtonDown(AButtonInput) && Input.GetButtonDown(BButtonInput))
            {
                //Disable Push and Pull

                //Release unique ulti skill
                Debug.Log("You have released the kraken");
                ReleaseSpecialSkill();
            }
        }
    }

    /// ***********
    //Set the character stuff
    /// ***********
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

    public void SetInputs(string A, string B)
    {
        AButtonInput = A;
        BButtonInput = B;
    }



    public void ChargeSpecialSkill(float projectileKnockback)
    {
        Debug.Log("I feel stronger each time");

        //Increase thy ulti charge
        currentCharge = Mathf.Min(currentCharge + projectileKnockback, fullChargeAmount);//Make sure current charge never exceedsss the max

        //Debug.Log("My power is now at: " + currentCharge);

        //Insert UI stuff here for charge bar update
        

    }


    void ReleaseSpecialSkill()
    {
        // Release the kraken

        switch (playerAvatar) {

            case (characterChosen.Fire):
            {
                //Insert Fire
                break;
            }


            case (characterChosen.Water):
            {
                //Insert Water
                break;
            }

            case (characterChosen.Lightning):
            {
                //Insert Ziggity Zaggity
                break;
            }
            case (characterChosen.Earth):
            {
                //Insert Golem??? 
                break;
            }


            default:
            {
                Debug.Log("Oh heckie");
                break;
            }
        }

    }


}
