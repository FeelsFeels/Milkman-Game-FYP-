using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AIPlayerSkillSetManager : MonoBehaviour
    {
        public enum characterChosen { Fire, Water, Earth, Lightning, };

        [Header("Charging Ultimate Skill Settings")]
        public characterChosen playerAvatar;
        public float fullChargeAmount‬ = 13195f;
        public float currentCharge = 0;
        public float chargePercentage;
        string AButtonInput;
        string BButtonInput;
        public bool ultiIsActivated; // Is the ultimate skill currently in use?

        [Header("Skill prefabs in order of Fire, Water, Earth & Lightning")]
        //public GameObject[] skillPrefabs = new GameObject[4]; // Container for skill prefabs
        public Transform skillContainer;
        Dictionary<characterChosen, SkillSet> playerSkills = new Dictionary<characterChosen, SkillSet>();

        public ChargingPushVFXController chargingPushVFX;   //Needed to stop charging vfx when ulti is activated
        public GameObject skillReadyParticleFX;
        public bool testingPurposes;

        // Start is called before the first frame update
        void Start()
        {
            // Set the dictionary
            // I'm not sure if there's a better way to do this other than to hard code, but thankfull there's only 4 enums
            // Why doesn't dictionary have an add range function

            if (skillContainer != null)
            {
                if (skillContainer.GetChild(0).GetComponent<SkillSet>() != null)
                {
                    playerSkills.Add(characterChosen.Fire, skillContainer.GetChild(0).GetComponent<SkillSet>());
                }
                if (skillContainer.GetChild(1).GetComponent<SkillSet>() != null)
                {
                    playerSkills.Add(characterChosen.Water, skillContainer.GetChild(1).GetComponent<SkillSet>());
                }
                if (skillContainer.GetChild(2).GetComponent<SkillSet>() != null)
                {
                    playerSkills.Add(characterChosen.Earth, skillContainer.GetChild(2).GetComponent<SkillSet>());
                }
                if (skillContainer.GetChild(3).GetComponent<SkillSet>() != null)
                {
                    playerSkills.Add(characterChosen.Lightning, skillContainer.GetChild(3).GetComponent<SkillSet>());
                }
            }
            chargingPushVFX = GetComponentInChildren<ChargingPushVFXController>();
            ChargeSpecialSkill(1);
        }
        

        void ActivateReadyParticles()
        {
            skillReadyParticleFX.SetActive(true);
        }
        void DeactivateReadyParticles()
        {
            skillReadyParticleFX.SetActive(false);
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

            //Increase thy ulti charge
            currentCharge = Mathf.Min(currentCharge + projectileKnockback, fullChargeAmount);//Make sure current charge never exceedsss the max

            //Debug.Log("My power is now at: " + currentCharge);


            //Charge bar update
            chargePercentage = currentCharge / fullChargeAmount;

            if (chargePercentage >= 1)
                ActivateReadyParticles();

        }

        /// *********************************
        /// Release the ultimate
        /// *********************************
        [System.Serializable]
        public abstract class SkillSet : MonoBehaviour
        {
            public float skillDuration; // Duration of the skill

            public abstract void SkillAttack(AIPlayerSkillSetManager playerSkillManager);
            // Create your own method for this skill, this will be called in SkillSetManager 
            // YOU MUST OVERRIDE THIS
            // Reference to SkillSetManager if necessary

            public virtual void EndUltimate(AIPlayerSkillSetManager playerSkillManager)
            {
                // Remember to set the ultiIsActivated bool to false when ulti ends
                // Bool playerCannotShoot to be set to false to re enable shooting
                // You CAN override this method with your own version
                playerSkillManager.ultiIsActivated = false;
                playerSkillManager.gameObject.GetComponent<AIShoot>().playerCannotShoot = false;
            }

            public abstract void SetManagerAndCharacter(AIPlayerSkillSetManager player);
        }

        public void ReleaseSpecialSkill()
        {

            if (ultiIsActivated) return;

            //Set bool active
            ultiIsActivated = true;

            chargingPushVFX.StopVFX();
            

            if (playerSkills.ContainsKey(playerAvatar)) // Check if there is a skill for the enum, because there might not be a skill prefab for the enum
            {
                playerSkills[playerAvatar].SetManagerAndCharacter(this);
                playerSkills[playerAvatar].SkillAttack(this); // Dictionary will return the skill set we want, and subsequently we call the attack method
            }

            else // Default fall back if the skill for the enum... doesn't exist yet
            {
                Debug.Log("Producing clones");

                // Muahaha
                GameObject go = Instantiate(gameObject, transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody>().velocity = transform.forward;
                Destroy(go, 5f);
                StartCoroutine(EndUltiTimer(5));
            }

            gameObject.GetComponent<AIShoot>().PlayerResetCharge();

            ResetUltiCharge(); // Reset skill charge gauge
        }

        public void ForceEndUltimateSkill()
        {
            playerSkills[playerAvatar].EndUltimate(this);
        }

        IEnumerator EndUltiTimer(float duration)
        {
            yield return new WaitForSeconds(duration);
            ultiIsActivated = false;
            this.gameObject.GetComponent<AIShoot>().playerCannotShoot = false;
            gameObject.GetComponent<AIShoot>().PlayerResetCharge();
        }

        void ResetUltiCharge()
        {
            currentCharge = 0;
            chargePercentage = currentCharge / fullChargeAmount;
            DeactivateReadyParticles();
        }

    }
}