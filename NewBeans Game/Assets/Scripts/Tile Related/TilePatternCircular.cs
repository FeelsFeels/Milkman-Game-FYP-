using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePatternCircular : MonoBehaviour
{
    public Tile[] initTiles;    //Initial tiles you want to move down at the start of the game
    private List<Tile> outerTiles = new List<Tile>();

    public LayerMask groundLayerMask;

    private PlatformManager platformManager;

    private void Awake()
    {
        platformManager = FindObjectOfType<PlatformManager>();
    }

    private void Start()
    {
        platformManager.OnNewPhase += Expand;

        foreach(Tile tile in initTiles)
        {
            tile.MoveDown();
            outerTiles.Add(tile);
        }
    }

    public void Expand()    //This method causes the hole to expand in all directions
    {
        List<Tile> newTiles = new List<Tile>(); //A list containing the next outermost tiles for future calls
        foreach(Tile tile in outerTiles)
        {
            Collider[] hitColliders = Physics.OverlapSphere(tile.upPos, 1.5f, groundLayerMask); //Check which tiles are in radius of outermost tiles
            if(hitColliders != null)
            {
                foreach(Collider hit in hitColliders)
                {
                    Tile hitTile = hit.GetComponent<Tile>();
                    if(hitTile.tileState == Tile.TileState.up)  //If the tiles that are in its radius are up, make them go down
                    {
                        hitTile.tileState = Tile.TileState.goingDown;   
                        newTiles.Add(hitTile);
                    }
                }
            }
        }
        
        if(newTiles.Count > 0)
        {
            outerTiles.Clear();
            outerTiles = newTiles;
        }

    }

}
