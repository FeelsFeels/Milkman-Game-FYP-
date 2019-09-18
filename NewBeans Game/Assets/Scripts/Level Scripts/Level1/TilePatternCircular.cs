using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilePatternCircular : MonoBehaviour
{
    public Tile[] initTiles;    //Initial tiles you want to move down at the start of the game
    private List<Tile> outerTiles = new List<Tile>();

    public LayerMask groundLayerMask;
    private EventsManager eventsManager;

    private bool crRunning;


    public GameObject crumblingParticleEffect;

    private void Awake()
    {
        crRunning = false;
        eventsManager = FindObjectOfType<EventsManager>();
    }

    private void Start()
    {
        eventsManager.OnNewPhase += Expand;

        //Makes the initial tiles move down
        foreach(Tile tile in initTiles)
        {
            tile.tileState = Tile.TileState.goingDown;
            outerTiles.Add(tile);
        }
    }

    public void Expand()
    {
        if(!crRunning)
            StartCoroutine(ExpandOverTime());
    }

    public IEnumerator ExpandOverTime()
    {
        print("Expanding");
        crRunning = true;

        List<Tile> newTiles = new List<Tile>(); //A list containing the next outermost tiles for future calls
        List<Collider> hitColliders = new List<Collider>(); //List containing the tiles to move down

        foreach (Tile tile in outerTiles)   //Checking which tiles to move down
        {
            Collider[] tilesToAffect = Physics.OverlapSphere(tile.upPos, 1.5f, groundLayerMask); //Check which tiles are in radius of outermost tiles

            foreach (Collider collider in tilesToAffect)
                hitColliders.Add(collider);
        }

        //Removing duplicate elements in list
        hitColliders = hitColliders.Distinct().ToList();

        //Make the ground shake, and move the tiles down.
        if (hitColliders != null)
        {
            foreach (Collider hit in hitColliders)
            {
                Instantiate(crumblingParticleEffect, hit.transform.position + Vector3.up, Quaternion.identity);
            }
            yield return new WaitForSeconds(5f);

            foreach (Collider hit in hitColliders)
            {
                Tile hitTile = hit.GetComponent<Tile>();
                if (hitTile.tileState == Tile.TileState.up) //If the tiles that are in its radius are up, make them go down
                {
                    hitTile.tileState = Tile.TileState.goingDown;
                    newTiles.Add(hitTile);
                }
            }
        }

        if (newTiles.Count > 0)
        {
            outerTiles.Clear();
            outerTiles = newTiles;  //Set the outermost tiles list for the next ExpandOverTime()
        }

        crRunning = false;
    }


    //Shuffling stuff
    //Fisher-Durstenfeld's Shuffle
    public void ShuffleByReference<T>(ref T[] listToShuffle)
    {
        for (int i = listToShuffle.Length; i > 1; i--)
        {
            print("Mother fucker");
            int roll = Random.Range(0, i - 1);
            T temp = listToShuffle[i - 1];
            listToShuffle[i - 1] = listToShuffle[roll];
            listToShuffle[roll] = temp;
        }
    }
    public void ShuffleByReference<T>(ref List<T> listToShuffle)
    {
        for (int i = listToShuffle.Count; i > 1; i--)
        {
            int roll = Random.Range(0, i - 1);
            T temp = listToShuffle[i - 1];
            listToShuffle[i - 1] = listToShuffle[roll];
            listToShuffle[roll] = temp;
        }
    }

    //public void Expand()    //This method causes the hole to expand in all directions
    //{
    //    List<Tile> newTiles = new List<Tile>(); //A list containing the next outermost tiles for future calls
    //    foreach(Tile tile in outerTiles)
    //    {
    //        Collider[] hitColliders = Physics.OverlapSphere(tile.upPos, 1.5f, groundLayerMask); //Check which tiles are in radius of outermost tiles
    //        if(hitColliders != null)
    //        {
    //            foreach(Collider hit in hitColliders)
    //            {
    //                Tile hitTile = hit.GetComponent<Tile>();
    //                if(hitTile.tileState == Tile.TileState.up)  //If the tiles that are in its radius are up, make them go down
    //                {
    //                    hitTile.tileState = Tile.TileState.goingDown;   
    //                    newTiles.Add(hitTile);
    //                }
    //            }
    //        }
    //    }

    //    if(newTiles.Count > 0)
    //    {
    //        outerTiles.Clear();
    //        outerTiles = newTiles;
    //    }

    //}

}
