using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInfluenceController : MonoBehaviour
{
    DeathInfluenceManager deathInfluenceManager;
    PlayerController referencePlayer;
    bool controllerActivated;

    BaseDeathSkill skillToUse;
    bool isUsingSkill;
    public float abilityCooldown;
    float currentCooldown = 5f;
    

    private void Awake()
    {
        deathInfluenceManager = FindObjectOfType<DeathInfluenceManager>();
    }

    private void Update()
    {
        if (!controllerActivated)
            return;

        currentCooldown -= Time.deltaTime;
        if(currentCooldown < 0 && !isUsingSkill)
        {
            isUsingSkill = true;
            UseNewSkill();
        }
    }

    public void ActivateDeathInfluence(PlayerController newPlayer)
    {
        referencePlayer = newPlayer;
        controllerActivated = true;
    }

    void UseNewSkill()
    {
        GameObject newSkill = deathInfluenceManager.GetSkill();
        skillToUse = Instantiate(newSkill, Vector3.zero, Quaternion.identity).GetComponent<BaseDeathSkill>();

        skillToUse.Initialise(referencePlayer, this);
    }

    public void FinishUsingSkill()
    {
        isUsingSkill = false;
        skillToUse = null;

        currentCooldown = abilityCooldown;
    }
}
