using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDeathSkill : MonoBehaviour
{
    public PlayerController assignedPlayer;
    public DeathInfluenceController assignedController;

    [HideInInspector] public string HorizontalInputAxis;
    [HideInInspector] public string VerticalInputAxis;
    [HideInInspector] public string AButtonInput;
    [HideInInspector] public string BButtonInput;

    public void Initialise(PlayerController player, DeathInfluenceController controller)
    {
        assignedPlayer = player;
        assignedController = controller;

        PlayerInputInfo inputInfo = player.inputInfo;
        HorizontalInputAxis = inputInfo.HorizontalInputAxis;
        VerticalInputAxis = inputInfo.VerticalInputAxis;
        AButtonInput = inputInfo.AButtonInput;
        BButtonInput = inputInfo.BButtonInput;

        OnReady();
    }

    public abstract void OnReady();

    public abstract void OnUse();

    public abstract void OnFinishSkill();

}
