using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePatternCircular : MonoBehaviour
{
    public Tile[] initTiles;    //Initial tiles you want to move down at the start of the game
    private List<Tile> outerTiles = new List<Tile>();

    public LayerMask groundLayerMask;

    private EventsManager eventsManager;

    private bool crRunning;

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
        crRunning = true;

        List<Tile> newTiles = new List<Tile>(); //A list containing the next outermost tiles for future calls

        foreach (Tile tile in outerTiles)
        {
            Collider[] hitColliders = Physics.OverlapSphere(tile.upPos, 1.5f, groundLayerMask); //Check which tiles are in radius of outermost tiles
            if (hitColliders != null)
            {
                foreach(Collider hit in hitColliders)
                {
                    Tile hitTile = hit.GetComponent<Tile>();
                    if (hitTile.tileState == Tile.TileState.up) //If the tiles that are in its radius are up, make them go down
                    {
                        hitTile.tileState = Tile.TileState.goingDown;
                        newTiles.Add(hitTile);
                    }
                    yield return new WaitForSeconds(1f);
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
