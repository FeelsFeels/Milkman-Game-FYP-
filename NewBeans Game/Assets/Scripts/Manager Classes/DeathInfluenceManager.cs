using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInfluenceManager : MonoBehaviour
{
    public List<PlayerController> deadPlayers = new List<PlayerController>(4);
    public List<DeathInfluenceController> controllers = new List<DeathInfluenceController>(4);

    public GameObject[] deathSkills;

    
    public GameObject GetSkill()
    {
        return deathSkills[Random.Range(0, deathSkills.Length)];
    }

    public void AddNewDeadPlayer(PlayerController deadPlayer)
    {
        deadPlayers.Add(deadPlayer);
        controllers[deadPlayers.Count - 1].ActivateDeathInfluence(deadPlayer);
    }
}
