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
     * Tells a joke to every NPC character in the room.
     * @param[in] jokeType: The type of the joke to tell.
     */
    public void TellJoke(JokeItemType jokeType)
    {
        foreach (Character character in charactersInRoom)
        {
            character.Joke(jokeType);
        }
    }

    /**
     * Initializes the room.
     */
    void Start()
    {
        workStationsInRoom = new List<WorkStation>(GetComponentsInChildren<WorkStation>());
        foreach (WorkStation workStation in workStationsInRoom)
        {
            workStation.SetOwnerRoom(this);
        }
    }

    /**
     * Queries the list of free (unreserved and unoccopied) workstations in the room.
     */
    public List<WorkStation> GetFreeWorkstations()
    {   
        List<WorkStation> freeWorkStations = new List<WorkStation>();
        foreach (WorkStation workStation in workStationsInRoom)
        {
            if (!workStation.IsReserved())
            {
                freeWorkStations.Add(workStation);
            }
        }

        return freeWorkStations;
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
