using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChangingTilePatterns : MonoBehaviour
{
    
    public List<Tile> tilePattern = new List<Tile>();    //Each gameobject holds the tiles wanted for the different pattern.
    public int currentPatternIndex;


    [Header("Crumbling Indication")]
    public LayerMask groundLayer;
    public int segments;
    public float lengthBetweenSegment;
    public GameObject crumblingParticleEffect;
    
    
    public void SelectNewPattern()
    {
        //Keeps randomising until a new pattern is found
        int rand = Random.Range(0, tilePattern.Count);
        while (rand == currentPatternIndex)
        {
            rand = Random.Range(0, tilePattern.Count);
        }
        //New pattern is found
        currentPatternIndex = rand;

        //Start assembling the new pattern
        StartCoroutine("AssembleNewPattern");
    }

    IEnumerator AssembleNewPattern()
    {
        foreach (Tile tile in tilePattern) //Move everything up
        {
            if (tile.tileState == Tile.TileState.up || tile.tileState == Tile.TileState.goingUp)
                continue;
            else
                tile.MoveUp(5f);
        }
        //waiting for tiles to move back up
        yield return new WaitForSeconds(7);

        //Get the new tile to go down
        Tile tileToGoDown = tilePattern[7];

        //Give warning to players using smoke particles
        //Make a "tile map" of positions to spawn the smoke particles in.
        for (int x = 0; x < segments; x++)  //New Position x-axis wise
        {
            for (int z = 0; z < segments; z++)  //New Position z-axis wise
            {
                //Getting new Position
                float xPos = (tileToGoDown.transform.position.x - Mathf.CeilToInt(segments / 2) * lengthBetweenSegment) + (x * lengthBetweenSegment);
                float zPos = (tileToGoDown.transform.position.z -  Mathf.CeilToInt(segments / 2) * lengthBetweenSegment) + (z * lengthBetweenSegment);
                Vector3 spawnPosition = new Vector3(xPos, tileToGoDown.transform.position.y, zPos);

                //If new position is not over a tile, dont spawn crumbling particles
                RaycastHit hit;
                if(Physics.Raycast(spawnPosition + Vector3.up * 5, Vector3.down, out hit, 100f, 1 << LayerMask.NameToLayer("Ground")))
                {
                    Debug.DrawRay(spawnPosition, Vector3.down * 10, Color.red, 5f);
                    if (hit.collider.name != tileToGoDown.gameObject.name)  //If new position is over a different tile, dont spawn crumbling particles
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                //Spawn Particles
                GameObject particles = Instantiate(crumblingParticleEffect, spawnPosition + Vector3.up, Quaternion.identity);
                particles.transform.parent = tileToGoDown.transform;
                particles.GetComponent<AutoDestroyOverTime>().DestroyWithTime(10f);
            }
        }

        yield return new WaitForSeconds(5f);

        //Indication over, start moving tiles downwards.
        tileToGoDown.MoveDown(10f);
        
    }
}
