using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState
    {
        goingDown,
        goingUp,
        down,
        up
    }
    /// <summary>
    /// goingDown: moving downwards
    /// goingUp: moving upwards
    /// down: in a resting state in its down position
    /// up: in a resting state in its up position
    /// </summary>

    public Vector3 upPos;
    public Vector3 downPos;

    private float proximity = 0.0001f;

    public TileState tileState = TileState.up;


    private void Awake()
    {
        upPos = transform.position;
        float initialYCoord = transform.position.y;
        downPos = new Vector3(transform.position.x, initialYCoord - 10, transform.position.z);
    }

    private void Update()
    {        
        //Hacks
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            tileState = TileState.goingUp;
        }
        //Hacks
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            tileState = TileState.goingDown;
        }

        //Must change this to coroutines in the future
        if (tileState == TileState.goingDown)
        {
            MoveDown();
        }
        if(tileState == TileState.goingUp)
        {
            MoveUp();
        }
    }

    public void MoveDown()
    {
        Vector3 distance = transform.position - downPos;

        transform.position = Vector3.MoveTowards(transform.position, downPos, 3);


        if(distance.magnitude <= proximity) //Tile reached its target. It is completely at its down pos
        {
            tileState = TileState.down;
        }

    }
    
    public void MoveUp()
    {
        Vector3 distance = transform.position - upPos;

        transform.position = Vector3.MoveTowards(transform.position, upPos, 3);


        if (distance.magnitude <= proximity) //Tile reached its target. It is completely at its up pos
        {
            tileState = TileState.up;
        }
    }

}
