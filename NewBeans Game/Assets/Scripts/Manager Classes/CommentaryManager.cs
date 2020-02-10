using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommentaryManager : MonoBehaviour
{
    public static CommentaryManager instance;

    public Dictionary<PlayerController, int> killStreaks = new Dictionary<PlayerController, int>();

    public Transform[] audienceHolders;
    bool jumping;

    AudioManager audioManager;
    AudioSource audioSource;
    private void Awake()
    {
        instance = this;
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            killStreaks.Add(player, 0);
        }

        audioManager = FindObjectOfType<AudioManager>();
        audioSource = GetComponent<AudioSource>();
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
        if (jumping)
            return;

        jumping = true;
        StartCoroutine("JumpingTimer");
        foreach (Transform audience in audienceHolders)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.PrependInterval(Random.Range(0.0f, 0.3f));
            float originalYPos = audience.transform.position.y;
            for(int i = 0; i < 7; i++)
            {
                mySequence.Append(audience.DOMoveY(originalYPos + 2, 0.5f));
                mySequence.Append(audience.DOMoveY(originalYPos, 0.3f));
            }
        }

        if (audioSource)
        {
            StopCoroutine("FadeCheeringSounds");
            audioSource.volume = 1;
            audioSource.Play();
            StartCoroutine("FadeCheeringSounds");
        }
    }

    IEnumerator FadeCheeringSounds()
    {
        yield return new WaitForSeconds(5f);
        audioSource.DOFade(0, 0.7f);
    }
    
    IEnumerator JumpingTimer()
    {
        yield return new WaitForSeconds(6.4f);
        jumping = false;
    }
}
