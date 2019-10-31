using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastManStandingData : ScriptableObject
{
    public int livesRemaining;
    public Dictionary<PlayerController, int> playerLivesInfo = new Dictionary<PlayerController, int>();
}
