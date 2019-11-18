using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "New Character")]
public class CharacterData : ScriptableObject
{
    public GameObject characterPrefab;
    public Color32 characterColor;
    public Sprite characterUISprite;
    public string character;
}
