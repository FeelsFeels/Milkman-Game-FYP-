using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilePatternBigBlocks : MonoBehaviour
{
    Tile[] tileArray;

    [System.Serializable]
    public struct TilePattern
    {
        public string PatternDescription;   //For organisation
        public Tile[] tilesInPattern;
    }

    public List<TilePattern> patternHolders = new List<TilePattern>();    //Each gameobject holds the tiles wanted for the different pattern.
    public int currentPatternIndex;

    public GameObject crumblingParticleEffect;

    public void Awake()
    {
        tileArray = FindObjectsOfType<Tile>();
    }

    
    public void SelectNewPattern()
    {
        //Keeps randomising until a new pattern is found
        int rand = Random.Range(0, patternHolders.Count);
        while (rand == currentPatternIndex)
        {
            rand = Random.Range(0, patternHolders.Count);
        }
        //New pattern is found
        currentPatternIndex = rand;

        //Start assembling the new pattern
        StartCoroutine("AssembleNewPattern");
    }

    IEnumerator AssembleNewPattern()
    {
        foreach (Tile tile in tileArray) //Move everything up
        {
            if (tile.tileState == Tile.TileState.up || tile.tileState == Tile.TileState.goingUp)
                continue;
            else
                tile.MoveUp(5f);
        }
        //waiting for tiles to move back up
        yield return new WaitForSeconds(7);

        //Give warning to players using smoke particles
        foreach (Tile tile in patternHolders[currentPatternIndex].tilesInPattern)  //Move desired tiles down
        {
            GameObject particles = Instantiate(crumblingParticleEffect, tile.transform.position + Vector3.up, Quaternion.identity);
            particles.transform.parent = tile.transform;
            particles.GetComponent<AutoDestroyOverTime>().DestroyWithTime(10f);
        }

        yield return new WaitForSeconds(5f);

        foreach (Tile tile in patternHolders[currentPatternIndex].tilesInPattern)  //Move desired tiles down
        {
            tile.MoveDown(10f);
        }
    }
}
