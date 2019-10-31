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

        for (int i = 0; i < playerRanking.Count; i++)
        {
            sb.Append("First place: Player" + playerRanking[i].playerNumber + "\n");
        }
        playerRankText.text = sb.ToString();
    }
}
