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

public class Character : MonoBehaviour
{
    const float eps = 0.0001f;
    public float speed = 0.4f;
    public Room startRoom;

    enum State
    {
        Going,
        Working,
        Idle
    }

    System.Random rd;
    State state;
    Target target;
    Room currentRoom;

    /**
     * Requests the character to leave its current workstation.
     * The workstation unregisters the character after this method is done.
     */
    public void RequestLeaveWorkstation(WorkStation workStation)
    {
        if(state == State.Working)
        {
            state = State.Idle;
        } 
        else
        {
            Debug.Log("<color=red>Error: </color> Invalid state change to Idle");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        target = new Target();
        rd = new System.Random();
        currentRoom = startRoom;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Going:

                transform.position = Vector2.MoveTowards(transform.position, target.GetTargetPosition(), speed * Time.deltaTime);
                if(Vector2.Distance(target.GetTargetPosition(), transform.position) < eps)
                {
                    ReachedGoal();
                }

                break;
            case State.Working:

                //Work, work

                break;
            case State.Idle:

                //TODO: If time allows, go to the center of the current room

                if( FindNewGoal() )
                {
                    state = State.Going;
                }
                break;
            default:
                //What
                break;
        }
        
    }

    //Currently, a possible target can only be in the adjacent rooms, if there is no space there, try the current room
    bool FindNewGoal()
    {
        List<Room> possibleTargets = new List<Room>();

        //Choosing a target workstation
        foreach( var d in currentRoom.doors)
        {
            var r = d.GetOtherRoom(currentRoom);
            if( r.GetFreeWorkstations().Count > 0)
            {
                possibleTargets.Add(r);
            }
        }

        Room targetRoom;
        if(possibleTargets.Count == 0)
        {
            if(currentRoom.GetFreeWorkstations().Count == 0)
            {
                return false;
            }
            else
            {
                targetRoom = currentRoom;
            }
        }
        else
        {
            targetRoom = possibleTargets[rd.Next(possibleTargets.Count)];
        }

        var workstations = targetRoom.GetFreeWorkstations();
        target.SetDestination(workstations[rd.Next(workstations.Count)]);

        //Letting the workstation know we are coming 
        target.TargetWorkStation.ReserveWorkstation();

        //Calculating the target (remember, only possible targets are this room and its neighbours)
        if(currentRoom != targetRoom)
        {
            foreach( var d in currentRoom.doors)
            {
                if(d.GetOtherRoom(targetRoom))
                {
                    target.SetTargetDoor(d);
                }
            }
        }
        else
        {
            //If we stay in the current room there is no door needed to get there
            target.SetTargetToWorkstation();
        }

        return true;
    }

    void ReachedGoal()
    {
        switch(target.Type)
        {
            case TargetType.Door:
                currentRoom.LeaveRoom(this);
                currentRoom = target.TargetDoor.GetOtherRoom(currentRoom);
                currentRoom.EnterRoom(this);

                //Currently, the goal can only be in the neighbouring room. If this changes, change here.
                target.SetTargetToWorkstation();

                break;
            case TargetType.Workstation:
                target.TargetWorkStation.EnterWorkstation(this);
                target.Clear();
                state = State.Working;

                break;
            default:
                //What
                break;
        }
    }
}
