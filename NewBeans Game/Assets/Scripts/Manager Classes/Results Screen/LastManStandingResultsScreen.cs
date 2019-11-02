using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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

        sb.Append("Winner: Player " + playerRanking[0].playerNumber + "\n");
        sb.Append("Second Place: Player " + playerRanking[1].playerNumber + "\n");

        if (numberOfPlayers > 2)
        {
            sb.Append("Third Place: Player " + playerRanking[2].playerNumber + "\n");
        }

        if(numberOfPlayers > 3)
        {
            sb.Append("Fourth Place: Player " + playerRanking[3].playerNumber + "\n");
        }

        playerRankText.text = sb.ToString();
    }
}
