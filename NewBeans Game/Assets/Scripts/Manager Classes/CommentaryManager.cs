using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommentaryManager : MonoBehaviour
{
    public static CommentaryManager instance;

    public Dictionary<PlayerController, int> killStreaks = new Dictionary<PlayerController, int>();

    public Transform[] audienceHolders;
    private void Awake()
    {
        instance = this;
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            killStreaks.Add(player, 0);
        }
    }
    

    public void CheckPlayerKill(PlayerController player, PlayerController killer)
    {
        //Player died. Reset his scorestreak.
        if (player)
        {
            killStreaks[player] = 0;
        }
        if (killer)
        {
            killStreaks[killer]++;
            if(killStreaks[killer] >= 4)
            {
                AudienceJump();
            }
        }
    }

    public void AudienceJump()
    {
        foreach(Transform audience in audienceHolders)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.PrependInterval(Random.Range(0.0f, 0.3f));
            float originalYPos = audience.transform.position.y;
            for(int i = 0; i < 10; i++)
            {
                mySequence.Append(audience.DOMoveY(originalYPos + 2, 0.5f));
                mySequence.Append(audience.DOMoveY(originalYPos, 0.3f));
            }
        }
    }
}
