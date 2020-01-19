using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPrefab : MonoBehaviour
{
    public Text playerName;
    public Image rankPng;
    public Image icon;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("FlyIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
