using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseResultsScreen : MonoBehaviour
{
    public List<PlayerController> playerRanking;

    public abstract void DisplayScreen(List<PlayerController> ranking);
}
