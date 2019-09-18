using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionWithWeight : MonoBehaviour, IAffectedByWeight
{
    public List<Tile> tileList = new List<Tile>();
    public float weightOnObject { get; set; }
    public float sectionDurability = 1000f;

    bool crumbling = false;

    public void Start()
    {
        foreach(Transform child in transform)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile)
                tileList.Add(tile);
        }
    }

    public void AddWeight(float weight)
    {
        weightOnObject += weight;
    }

    public void RemoveWeight(float weight)
    {
        weightOnObject -= weight;
    }

    private void FixedUpdate()
    {
        sectionDurability -= weightOnObject;

        if (sectionDurability <= 0)
        {
            PlatformCrumble();
        }
        UpdateTileMaterial();
    }

    void PlatformCrumble()
    {
        if (crumbling)
            return;

        crumbling = true;
        StartCoroutine("StartCrumbling");
    }

    void UpdateTileMaterial()
    {

    }

    IEnumerator StartCrumbling()
    {
        //Delay
        //Show smoke particle effects
        yield return new WaitForSeconds(5);
        //Start crumbling
        foreach (Tile tile in tileList)
        {
            tile.MoveDown(15f);
        }
    }
}
