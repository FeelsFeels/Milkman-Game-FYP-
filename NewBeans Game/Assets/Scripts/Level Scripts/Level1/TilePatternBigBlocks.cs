using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilePatternBigBlocks : MonoBehaviour
{
    Tile[] tileArray;
    public GameObject[] patternHolders;    //Each gameobject holds the tiles wanted for the different pattern.
    public GameObject currentPatternHolder = null;

    public GameObject crumblingParticleEffect;

    public void Awake()
    {
        tileArray = FindObjectsOfType<Tile>();
    }

    public void SelectNewPattern()
    {
        //Keeps randomising until a new pattern is found
        int rand = Random.Range(0, patternHolders.Length - 1);
        do
        {
            rand = Random.Range(0, patternHolders.Length) - 1;
        }
        while (patternHolders[rand] == currentPatternHolder);
        //New pattern is found
        currentPatternHolder = patternHolders[rand];

        //Set all the tiles to the pattern
        
        foreach(Tile tile in tileArray) //Move everything up
        {
            if (tile.tileState == Tile.TileState.up || tile.tileState == Tile.TileState.goingUp)
                continue;
            else
                tile.MoveUp();
        }

        foreach(Transform t in currentPatternHolder.transform)  //Move desired tiles down
        {
            t.GetComponent<Tile>().MoveDown(15f);

            GameObject particles = Instantiate(crumblingParticleEffect, t.transform.position + Vector3.up, Quaternion.identity);
            particles.transform.parent = t.transform;
            particles.GetComponent<AutoDestroyOverTime>().DestroyWithTime(15f);
        }
    }
    
}
