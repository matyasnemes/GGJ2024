using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Character> charactersInRoom;
    public List<WorkStation> workStationsInRoom;
    public List<Door> doors;

    /**
     * Adds a character to the room.
     * @param[in] character: The character that entered the room.
     */
    public void EnterRoom(Character character)
    {
        charactersInRoom.Add(character);
    }

    /**
     * Removes a character from the room.
     * @param[in] character: The character that left the room.
     */
    public void LeaveRoom(Character character)
    {
        charactersInRoom.Remove(character);
    }

    /**
     * Initializes the room.
     */
    void Start()
    {
        workStationsInRoom = new List<WorkStation>(GetComponentsInChildren<WorkStation>());
    }

    /**
     * Queries the number of free (unreserved & unoccopied) workstations in the room.
     */
    public int GetFreeSpaces()
    {
        int freeSpaces = 0;
        foreach (WorkStation workStation in workStationsInRoom)
        {
            freeSpaces += workStation.IsReserved() ? 0 : 1;
        }

        return freeSpaces;
    }

    /**
     * Queries whether the specified room is a neighbour of this room. 
     */
    public bool IsNeighbourRoom(Room room)
    {
        foreach (Door door in doors)
        {
            if ((door.room1 == this && door.room2 == room) || (door.room1 == room && door.room2 == this))
            {
                return true;
            }
        }

        return false;
    }
}
