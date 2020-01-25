using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "New Character")]
public class CharacterData : ScriptableObject
{
    public GameObject characterPrefab;
    public GameObject pushProjectile;
    public GameObject hookProjectile;
    public GameObject characterUltiReadyVFX;
    public GameObject chargingPushModel;
    public GameObject chargingPushVFX;
    public Color32 characterColor;
    public GameObject characterUI_Left;
    public GameObject characterUI_Right;
    public GameObject characterUI_LeftUp;
    public GameObject characterUI_RightUp;
    public string character;
}
