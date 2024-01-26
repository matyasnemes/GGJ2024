using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Character> charactersInRoom;
    public List<WorkStation> workStationsInRoom;
    public List<Door> doors;

    public void EnterRoom(Character character)
    {
        charactersInRoom.Add(character);
    }

    public void LeaveRoom(Character character)
    {
        charactersInRoom.Remove(character);
    }

    // Start is called before the first frame update
    void Start()
    {
        workStationsInRoom = new List<WorkStation>(GetComponentsInChildren<WorkStation>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int freeSpaces()
    {
        return 0;
    }

    public bool isNeighbour(Room room)
    {
        return false;
    }


}
