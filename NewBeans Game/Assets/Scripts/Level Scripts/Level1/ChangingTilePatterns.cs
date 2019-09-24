using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChangingTilePatterns : MonoBehaviour
{

    Tile[] tileArray;

    public List<GameObject> patternHolders = new List<GameObject>();    //Each gameobject holds the tiles wanted for the different pattern.
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
        foreach (Transform t in patternHolders[currentPatternIndex].transform)  //Move desired tiles down
        {
            Tile tile = t.GetComponent<Tile>();

            GameObject particles = Instantiate(crumblingParticleEffect, tile.transform.position + Vector3.up, Quaternion.identity);
            particles.transform.parent = tile.transform;
            particles.GetComponent<AutoDestroyOverTime>().DestroyWithTime(10f);
        }

        yield return new WaitForSeconds(5f);

        foreach (Transform t in patternHolders[currentPatternIndex].transform)  //Move desired tiles down
        {
            Tile tile = t.GetComponent<Tile>();
            tile.MoveDown(10f);
        }
    }
}
