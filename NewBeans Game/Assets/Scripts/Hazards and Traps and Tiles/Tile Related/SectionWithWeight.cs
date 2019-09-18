using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionWithWeight : MonoBehaviour, IAffectedByWeight
{
    public enum DurabilityState    //0 for completely healthy, 1 for half broken, 2 for almost broken.
    {
        Full,
        TwoThirds,
        OneThird,
        Broken
    }
    public DurabilityState curDurabilityState;

    public List<Tile> tileList = new List<Tile>();
    public float weightOnObject { get; set; }
    public float maxSectionDurability = 1000f;
    public float curSectionDurability;
    
    public Material materialFull, materialTwoThirds, materialOneThird, materialBroken;
    public GameObject crumblingParticles;

    bool crumbling = false;

    public void Start()
    {
        curSectionDurability = maxSectionDurability;
        InvokeRepeating("UpdateTileMaterial", 5, 1);

        foreach (Transform child in transform)
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
        curSectionDurability -= weightOnObject;

        if (curSectionDurability <= 0)
        {
            PlatformCrumble();
        }
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
        float percentBroken = curSectionDurability / maxSectionDurability;
        DurabilityState state = curDurabilityState;

        if(percentBroken >= 0.66f && percentBroken <= 1)
        {
            state = DurabilityState.Full;
        }
        else if(percentBroken >= 0.33f)
        {
            state = DurabilityState.TwoThirds;
        }
        else if(percentBroken >= 0)
        {
            state = DurabilityState.OneThird;
        }
        else if(percentBroken <= 0)
        {
            state = DurabilityState.Broken;
        }
        
        if(curDurabilityState != state)
        {
            curDurabilityState = state;

            //CHANGE TILE MATERIAL
            Material newMaterial;
            switch (curDurabilityState)
            {
                case DurabilityState.Full:
                    newMaterial = materialFull;
                    break;
                case DurabilityState.TwoThirds:
                    newMaterial = materialTwoThirds;
                    break;
                case DurabilityState.OneThird:
                    newMaterial = materialOneThird;
                    break;
                case DurabilityState.Broken:
                    newMaterial = materialBroken;
                    break;
                default:
                    newMaterial = materialFull;
                    break;
            }

            foreach(Tile tile in tileList)
            {
                tile.GetComponent<Renderer>().material = newMaterial;
            }
        }
    }

    IEnumerator StartCrumbling()
    {
        //Delay
        //Show smoke particle effects
        foreach (Tile tile in tileList)
        {
            Instantiate(crumblingParticles, tile.transform.position + Vector3.up, Quaternion.identity);
        }
        yield return new WaitForSeconds(5);
        //Start crumbling
        foreach (Tile tile in tileList)
        {
            tile.MoveDown(15f);
        }
    }
}
