using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float eps = 0.0001f;
    Vector3 bubbleOffset = new Vector3(0, 7, 0);

    
    public float speed = 0.4f;
    public float bubbleTime = 2.0f;
    public List<string> funnyJokes;
    public GameObject laughBubble;
    public GameObject neutralBubble;

    enum State
    {
        Going,
        Working,
        Idle
    }

    bool iAmRobot = false;
    System.Random rd;
    State state;
    Target target;
    Room currentRoom;
    GameObject bubble = null;
    CharacterDisplay charDisp;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        target = new Target();
        rd = new System.Random();
        charDisp = GetComponentInChildren<CharacterDisplay>();
    }

     // Update is called once per frame
    void Update()
    {
        //Handle state specific behaviour
        switch(state)
        {
            case State.Going:

                Vector2 newpos = Vector2.MoveTowards(transform.position, target.GetTargetPosition(), speed * Time.deltaTime);
                Vector2 displacement = newpos - new Vector2(transform.position.x, transform.position.y);
                
                //facing up or down?
                var charDisp = GetComponentInChildren<CharacterDisplay>();
                bool down = displacement.y >= 0 ? false : true;
                if( charDisp.faceDown != down)
                {
                    if(down)
                    {
                        charDisp.FaceDown();
                    }
                    else
                    {
                        charDisp.FaceUp();
                    }
                }
                
                //Actually Go;
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

    public void ThisIsFunny(string joke)
    {
        funnyJokes.Add(joke);
    }

    public void YouAreARobot()
    {
        iAmRobot = true;
    }

    public bool AreYouARobot()
    {
        return iAmRobot;
    }

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

    public void Joke(string joke)
    {
        if(funnyJokes.Contains(joke))
        {
            //TODO: Laugh
            bubble = laughBubble;
        }
        else
        {
            //TODO: Meh
            bubble = neutralBubble;
        }


        bubble = Instantiate(bubble, transform);
        bubble.transform.position += bubbleOffset;
        Destroy(bubble, bubbleTime);
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
                if(target.TargetWorkStation.facingDown)
                {
                    charDisp.FaceDown();
                }
                else
                {
                    charDisp.FaceUp();
                }

                target.Clear();
                state = State.Working;

                break;
            default:
                //What
                break;
        }
    }
}
