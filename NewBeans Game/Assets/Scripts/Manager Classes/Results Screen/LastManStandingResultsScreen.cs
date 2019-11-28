using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastManStandingResultsScreen : BaseResultsScreen
{
    public GameObject resultsScreen;
    public Text playerRankText;

    private void Awake()
    {
        resultsScreen.SetActive(false);
    }

    public override void DisplayScreen(List<PlayerController> ranking)
    {
        resultsScreen.SetActive(true);
        playerRanking = ranking;

        StringBuilder sb = new StringBuilder();

        int numberOfPlayers = ranking.Count;

        sb.Append("Winner: Player " + playerRanking[0].playerNumber + " [Kills: " + playerRanking[0].killCount + "]" + "\n");
        sb.Append("Second Place: Player " + playerRanking[1].playerNumber + " [Kills: " + playerRanking[1].killCount + "]" + "\n");

        if (numberOfPlayers > 2)
        {
            sb.Append("Third Place: Player " + playerRanking[2].playerNumber + " [Kills: " + playerRanking[2].killCount + "]" + "\n");
        }

        if(numberOfPlayers > 3)
        {
            sb.Append("Fourth Place: Player " + playerRanking[3].playerNumber + " [Kills: " + playerRanking[3].killCount + "]" + "\n");
        }

        playerRankText.text = sb.ToString();
    }

}
