using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float eps = 0.0001f;
    public float speed = 0.4f;
    public List<JokeItemType> funnyJokes;

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
     * Called automatically when entering another trigger collider.
     * Used to automatically set the starting room of the character.
     */
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Room room = collider.gameObject.GetComponent<Room>();
        if (currentRoom == null && room != null)
        {
            currentRoom = room;
        }
    }

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

    public void Joke(JokeItemType joke)
    {
        if(funnyJokes.Contains(joke))
        {
            //TODO: Laugh
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        target = new Target();
        rd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Going:

                Vector2 newpos = Vector2.MoveTowards(transform.position, target.GetTargetPosition(), speed * Time.deltaTime);

                //facing up or down?
                var charDisp = GetComponentInChildren<CharacterDisplay>();
                bool down = (newpos - new Vector2(transform.position.x, transform.position.y)).y >= 0 ? false : true;
                if( charDisp.faceDown != down)
                {
                    charDisp.faceDown = down;

                    if(down)
                    {
                        charDisp.FaceDown();
                    }
                    else
                    {
                        charDisp.FaceUp();
                    }
                }

                transform.position = newpos;

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
                if(d.IsConnectedToRoom(targetRoom))
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
