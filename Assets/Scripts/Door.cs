using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Door : MonoBehaviour
{
    public Room room1;
    public Room room2;

    public Vector2 Offset1 => transform.Find("Offset1").transform.position;
    public Vector2 Offset2 => transform.Find("Offset2").transform.position;

    /**
     * Given a room returns the other room this door connects to.
     */
    public Room GetOtherRoom(Room thisRoom)
    {
        if (thisRoom == room1) 
        {
            return room2;
        }
        else if (thisRoom == room2) 
        {
            return room1;
        }
        else
        {
            Debug.LogError("GetOtherRoom called with a non-connected room. Door is " + this + " called room is " + thisRoom);
            return null;
        } 
    }

    /**
     * Checks whether the specified room is connected to this door.
     */
    public bool IsConnectedToRoom(Room room)
    {
        return room1 == room || room2 == room;
    }
}
