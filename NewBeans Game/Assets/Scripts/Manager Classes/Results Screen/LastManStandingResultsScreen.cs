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
    public Sprite[] rankingImg;
    public Sprite[] icons; //Fire, water, earth, lightning
    public Transform holder;
    public GameObject rankPrefab;

    private void Awake()
    {
        resultsScreen.SetActive(false);
    }

    public override void DisplayScreen(List<PlayerController> ranking)
    {
        resultsScreen.SetActive(true);
        playerRanking = ranking;

        //StringBuilder sb = new StringBuilder();

        int numberOfPlayers = ranking.Count;

        //sb.Append("Winner: Player " + playerRanking[0].playerNumber + " [Kills: " + playerRanking[0].killCount + "]" + "\n");
        //sb.Append("Second Place: Player " + playerRanking[1].playerNumber + " [Kills: " + playerRanking[1].killCount + "]" + "\n");

        //if (numberOfPlayers > 2)
        //{
        //    sb.Append("Third Place: Player " + playerRanking[2].playerNumber + " [Kills: " + playerRanking[2].killCount + "]" + "\n");
        //}

        //if(numberOfPlayers > 3)
        //{
        //    sb.Append("Fourth Place: Player " + playerRanking[3].playerNumber + " [Kills: " + playerRanking[3].killCount + "]" + "\n");
        //}

        //playerRankText.text = sb.ToString();


        //Generate ranks
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject p = Instantiate(rankPrefab, holder);
            RankingPrefab r = p.GetComponent<RankingPrefab>();

            r.rankPng.sprite = rankingImg[i];

            switch (playerRanking[i].inputInfo.chosenCharacterData.character)
            {
                case "Fire":
                    {
                        r.icon.sprite = icons[0];
                        break;
                    }
                case "Water":
                    {
                        r.icon.sprite = icons[1];
                        break;
                    }
                case "Earth":
                    {
                        r.icon.sprite = icons[2];
                        break;
                    }
                case "Lightning":
                    {
                        r.icon.sprite = icons[3];
                        break;
                    }
            }

            r.playerName.text = "Player " + playerRanking[i].playerNumber;
        }

    }

}
