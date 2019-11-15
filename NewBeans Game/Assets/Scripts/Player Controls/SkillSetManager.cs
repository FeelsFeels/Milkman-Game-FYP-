using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// For using special skills
/// </summary>

public class SkillSetManager : MonoBehaviour
{
    public enum characterChosen { Fire, Water, Lightning, Earth,};

    [Header("Charging Ultimate Skill Settings")]
    public characterChosen playerAvatar;
    public float fullChargeAmount‬ = 13195f;
    public float currentCharge = 0;
    public float chargePercentage;
    string AButtonInput;
    string BButtonInput;
    public bool ultiIsActivated;

    [Header("UI")]
    public Image chargeBar;

    public UnityEvent<SkillSetManager> OnChargeUltimate = new ChargeUltiEvent();

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
                this.gameObject.GetComponent<Shoot>().playerCannotShoot = true;

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


        //Charge bar update
        chargePercentage = currentCharge / fullChargeAmount;

        OnChargeUltimate.Invoke(this);

    }


    void ReleaseSpecialSkill()
    {
        //Set bool active
        ultiIsActivated = true;


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
