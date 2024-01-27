using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Door : MonoBehaviour
{
    public Room room1;
    public Room room2;

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

    public bool IsConnectedToRoom(Room thisRoom)
    {
        return room1 == thisRoom || room2 == thisRoom;
    }
}
