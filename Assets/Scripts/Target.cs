using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    PreDoor,
    Door,
    PostDoor,
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
    Vector2 preDoorTarget;
    Vector2 postDoorTarget;

    Vector2 doorTargetOffset;

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

    // Parameters, the target door and the position of the character trying to move
    public void SetTargetDoor(Door d, Vector2 pos)
    {
        type = TargetType.PreDoor;
        targetDoor = d;

        //Set the pre and post targets based on which of the offsets is closer, in theory, they cant be equivalently close, if they are, that is a problem in itself

        if(Vector2.Distance(pos, d.Offset1) < Vector2.Distance(pos, d.Offset2))
        {
            preDoorTarget = d.Offset1;
            postDoorTarget = d.Offset2;
        }
        else
        {
            preDoorTarget = d.Offset2;
            postDoorTarget = d.Offset1;
        }
    }

    public void SetTargetToDoor()
    {
        type = TargetType.Door;
    }

    public void SetTargetToPostDoor()
    {
        type = TargetType.PostDoor;
    }

    public Vector2 GetTargetPosition()
    {
        switch(type)
        {
            case TargetType.Door:
                return targetDoor.transform.position;
            case TargetType.Workstation:
                return targetWorkStation.GetWorkingPosition();
            case TargetType.PreDoor:
                return preDoorTarget;
            case TargetType.PostDoor:
                return postDoorTarget;
            default:
                Debug.Log("<color=red>Error: </color> Read Target Position while type was none!");
                return Vector2.zero;
        }
    }
}   
