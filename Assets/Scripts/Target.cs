using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Door,
    Workstation,
    None
}

// This class represents the target of the NPC, the NPC is always moving through doors towards a workstation
// the workstation is known from the beginning, however only the next door is maintained
public class Target
{   
    TargetType type;
    public TargetType Type => type;

    Door targetDoor;
    public Door TargetDoor => targetDoor;

    WorkStation targetWorkStation;
    public WorkStation TargetWorkStation => targetWorkStation;


    public Target()
    {
        Clear();
    }

    public void Clear()
    {
        type = TargetType.None;
        targetDoor = null;
        targetWorkStation = null;
    }

    //Set the target workstation (probably meaning we are beginning a route)
    public void SetDestination(WorkStation ws)
    {
        targetWorkStation = ws;
    }

    //We are at the last leg of the journey, the next stop is the workstation
    public void SetTargetToWorkstation()
    {
        type = TargetType.Workstation;
    }

    public void SetTargetDoor(Door d)
    {
        type = TargetType.Door;
        targetDoor = d;
    }

    public Vector2 GetTargetPosition()
    {
        switch(type)
        {
            case TargetType.Door:
                return targetDoor.transform.position;
            case TargetType.Workstation:
                return targetWorkStation.transform.position;
            default:
                Debug.Log("<color=red>Error: </color> Read Target Position while type was none!");
                return Vector2.zero;
        }
    }
}   
