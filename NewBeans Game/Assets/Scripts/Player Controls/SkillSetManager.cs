﻿using System.Collections;
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
    public bool ultiIsActivated; // Is the ultimate skill currently in use?

    [Header("Skill prefabs in order of Fire, Water, Lightning & Earth")]
    public GameObject[] skillPrefabs = new GameObject[4]; // Container for skill prefabs
    Dictionary<characterChosen, SkillSet> playerSkills = new Dictionary<characterChosen, SkillSet>();

    // For updating UI
    public UnityEvent<SkillSetManager> OnChargeUltimate = new ChargeUltiEvent();

    // Start is called before the first frame update
    void Start()
    {
        // Set the dictionary
        // I'm not sure if there's a better way to do this other than to hard code, but thankfull there's only 4 enums
        // Why doesn't dictionary have an add range function
        if (skillPrefabs[0] != null)
        {
            playerSkills.Add(characterChosen.Fire, skillPrefabs[0].GetComponent<SkillSet>());
        }
        if (skillPrefabs[1] != null)
        {
            playerSkills.Add(characterChosen.Water, skillPrefabs[1].GetComponent<SkillSet>());
        }
        if (skillPrefabs[2] != null)
        {
            playerSkills.Add(characterChosen.Earth, skillPrefabs[2].GetComponent<SkillSet>());
        }
        if (skillPrefabs[3] != null)
        {
            playerSkills.Add(characterChosen.Lightning, skillPrefabs[3].GetComponent<SkillSet>());
        }

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

    /// *********************************
    /// Ulti Charge
    /// *********************************
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

    /// *********************************
    /// Release the ultimate
    /// *********************************
    [System.Serializable]
    public abstract class SkillSet : MonoBehaviour
    {
        public float skillDuration; // Duration of the skill

        public abstract void SkillAttack(SkillSetManager playerSkillManager); 
            // Create your own method for this skill, this will be called in SkillSetManager 
            // YOU MUST OVERRIDE THIS
            // Reference to SkillSetManager if necessary

        public virtual void EndUltimate(SkillSetManager playerSkillManager)
        {
            // Remember to set the ultiIsActivated bool to false when ulti ends
            // Bool playerCannotShoot to be set to false to re enable shooting
            // You CAN override this method with your own version

            playerSkillManager.ultiIsActivated = false;
            playerSkillManager.gameObject.GetComponent<Shoot>().playerCannotShoot = false;
        }
    }

    void ReleaseSpecialSkill()
    {
        //Set bool active
        ultiIsActivated = true;


        // Release the kraken!!!!!!! aaaaaaaaaaaaah

        if (playerSkills.ContainsKey(playerAvatar)) // Check if there is a skill for the enum, because there might not be a skill prefab for the enum
        {
            playerSkills[playerAvatar].SkillAttack(this); // Dictionary will return the skill set we want, and subsequently we call the attack method
        }

        else // Default fall back if the skill for the enum... doesn't exist yet
        {
            Debug.Log("Producing clones");

            // Muahaha
            GameObject go = Instantiate(gameObject, transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().velocity = transform.forward;
            Destroy(go, 5f);
            StartCoroutine(EndUlti(5));
        }

        ResetUltiCharge(); // Reset skill charge gauge
    }

    IEnumerator EndUlti(float duration)
    {
        yield return new WaitForSeconds(duration);
        ultiIsActivated = false;
        this.gameObject.GetComponent<Shoot>().playerCannotShoot = false;
    }

    void ResetUltiCharge()
    {
        Debug.Log("I am weak once again");
        currentCharge = 0;
        chargePercentage = currentCharge / fullChargeAmount;
        OnChargeUltimate.Invoke(this);
    }

}
