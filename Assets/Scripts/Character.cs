using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float eps = 0.0001f;
    Vector3 bubbleOffset = new Vector3(0, 7, 0);
    public float speed = 0.4f;
    public float bubbleTime = 2.0f;
    public float distanceFromNpc = 3.0f;
    public List<JokeItemType> funnyJokes;
    public GameObject laughBubble;
    public GameObject neutralBubble;
    public WorkStation finalWorkstation;

    /**
     * Prefab for Joke Paper items that are spawned on the floor.
     */
    public Transform JokePaperPrefab;

    /**
     * Minimum time between spawning Joke Papers.
     */
    public float JokeSpawnTimeSecondsMin = 5.0f;

    /**
     * Maximum time between spawning Joke Papers.
     */
    public float JokeSpawnTimeSecondsMax = 10.0f;

    /**
     * Remaining time until spawning the next Joke Paper.
     */
    private float RemainingSecondsUntilJokeSpawn = 0.0f;

    enum State
    {
        Going,
        Working,
        Idle,
        Fin
    }

    bool iAmRobot = false;
    bool firstFindGoal = true;
    bool disableJokeSpawn = false;
    bool movingToNpc = false;
    Vector2 moveToNpcPos = Vector2.zero;
    State state = State.Idle;
    Target target = new Target();
    Room currentRoom;
    GameObject bubble = null;
    CharacterDisplay charDisp;
    JokeFactory jokeFactory;
    WorkStation.WorkstationType typeOfLastWorkstation = WorkStation.WorkstationType.Final;

    // Start is called before the first frame update
    void Start()
    {
        charDisp = GetComponentInChildren<CharacterDisplay>();
        jokeFactory = GameObject.Find("Joker").GetComponent<JokeFactory>();
        RemainingSecondsUntilJokeSpawn = Random.Range(JokeSpawnTimeSecondsMin, JokeSpawnTimeSecondsMax);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStateMachine();
        UpdateJokeSpawner();
    }

    void NoFirstFind()
    {
        firstFindGoal = false;
    }

    public void ThisIsFunny(JokeItemType joke)
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
            currentRoom.EnterRoom(this);
        }
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    public void GoToFinalWorkstation()
    {  
        disableJokeSpawn = true;
        target.SetDestination(finalWorkstation);
        finalWorkstation.ReserveWorkstation();
        CalculateAndSetRoute();
        state = State.Going;

    }

    /**
     * Requests the character to leave its current workstation.
     * The workstation unregisters the character after this method is done.
     */
    public void RequestLeaveWorkstation(WorkStation workStation)
    {
        if (state == State.Working)
        {
            state = State.Idle;
        }
    }

    public void Joke(JokeItemType jokeType)
    {
        if (funnyJokes.Contains(jokeType))
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

    /**
     * Updates the state machine responsible for moving the character around.
     */
    void UpdateStateMachine()
    {
        // Handle state specific behaviour
        switch (state)
        {
            case State.Going:
                Vector2 newpos = Vector2.MoveTowards(transform.position, target.GetTargetPosition(), speed * Time.deltaTime);
                Vector2 displacement = newpos - new Vector2(transform.position.x, transform.position.y);

                //facing up or down?
                charDisp.StartMove();
                bool down = displacement.y >= 0 ? false : true;
                if (charDisp.faceDown != down)
                {
                    if (down)
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

                if (Vector2.Distance(target.GetTargetPosition(), transform.position) < eps)
                {
                    ReachedGoal();
                }

                break;
            case State.Fin:
                charDisp.StartIdle();
                if(movingToNpc)
                {
                    charDisp.StartMove();
                    if(Vector2.Distance(moveToNpcPos, transform.position) > distanceFromNpc) transform.position = Vector2.MoveTowards(transform.position, moveToNpcPos, speed * Time.deltaTime);
                }
                break;
            case State.Working:

                //Work, work

                break;
            case State.Idle:
                charDisp.StartIdle();
                //TODO: If time allows, go to the center of the current room
                if (firstFindGoal)
                {
                    if (FirstFindGoal()) state = State.Going;
                    firstFindGoal = false;
                }
                else if (FindNewGoal())
                {
                    state = State.Going;
                }
                break;
            default:
                //What
                break;
        }
    }

    void SpawnJokePaper()
    {
        JokeItem jokeItem = jokeFactory.createRandomJokeItem();
        Transform spawnedJokePaper = Instantiate(JokePaperPrefab, transform.position, transform.rotation);
        spawnedJokePaper.GetComponent<JokePaper>().jokeItem = jokeItem;
        spawnedJokePaper.GetComponent<SpriteRenderer>().sprite = jokeItem.unopenedSprite();
    }

    /**
     * Updates the random joke spawner logic.
     */
    void UpdateJokeSpawner()
    {
        if (RemainingSecondsUntilJokeSpawn < 0.0f)
        {
            if (!disableJokeSpawn && jokeFactory.CanSpawnNewJokePaper())
            {
                SpawnJokePaper();
                jokeFactory.OnJokePaperSpawned();
            }
            RemainingSecondsUntilJokeSpawn = Random.Range(JokeSpawnTimeSecondsMin, JokeSpawnTimeSecondsMax);
        }
        else
        {
            RemainingSecondsUntilJokeSpawn -= Time.deltaTime;
        }
    }

    //The first time we search for goal, it makes sense to search in the room
    bool FirstFindGoal()
    {
        var workstations = currentRoom.GetFreeWorkstations();
        if (workstations.Count > 0)
        {
            target.SetDestination(workstations[Random.Range(0,workstations.Count)]);
            target.TargetWorkStation.ReserveWorkstation();
            typeOfLastWorkstation = target.TargetWorkStation.workstationType;
            CalculateAndSetRoute();
            return true;
        }

        return false;
    }

    //Currently, a possible target can only be in the adjacent rooms, if there is no space there, try the current room
    bool FindNewGoal()
    {
        List<Room> possibleTargets = new List<Room>();

        GameObject roomCollector = GameObject.Find("Rooms");

        foreach(Transform child in roomCollector.transform)
        {
            Room room = child.gameObject.GetComponent<Room>();
            if(room != null && room.GetFreeWorkstations(typeOfLastWorkstation).Count > 0) possibleTargets.Add(room);
        }

        if(possibleTargets.Count == 0)
        {
            Debug.LogError("Could not find any rooms when choosing targets");
            return false;
        }

        Room targetRoom = possibleTargets[Random.Range(0,possibleTargets.Count)];                  

        var workstations = targetRoom.GetFreeWorkstations(typeOfLastWorkstation);
        target.SetDestination(workstations[Random.Range(0,workstations.Count)]);
        typeOfLastWorkstation = target.TargetWorkStation.workstationType;

        //Letting the workstation know we are coming 
        target.TargetWorkStation.ReserveWorkstation();

        //Calculate and set route to target
        CalculateAndSetRoute();
        
        return true;
    }

    //Calculate and set the route to a workstation that has been set as Target
    void CalculateAndSetRoute()
    {
        if(target.TargetWorkStation == null)
        {
            Debug.LogError("Would like to calculate route but there is no workstation");
            return;
        }

        Room targetRoom = target.TargetWorkStation.GetOwnerRoom();
        //Three cases, the target workstation is in the given room, or it is in a neighbouring room, or it is accross the office
        if(currentRoom == targetRoom)
        {
            target.SetTargetToWorkstation();
        }
        else
        {
            bool neighbour = false;
            Door door = null;

            foreach (var d in currentRoom.doors)
            {
                if(d.IsConnectedToRoom(targetRoom))
                {
                    neighbour = true;
                    door = d;
                }
            }

            if(neighbour)
            {
                //target workstation is in a neighbouring room, go through the door leading there
                target.SetTargetDoor(door, transform.position);
            }
            else
            {
                //target workstation is in the room on the other side of the building, go to the closest room
                float dist = float.MaxValue;

                foreach (var d in currentRoom.doors)
                {

                    if(Vector2.Distance(d.transform.position, transform.position) < dist)
                    {
                        neighbour = true;
                        door = d;
                    }
                } 

                target.SetTargetDoor(door, transform.position);
            }

        }
    }

    void ReachedGoal()
    {
        switch (target.Type)
        {
            case TargetType.PreDoor:
                target.SetTargetToDoor();
                break;
            case TargetType.Door:
                currentRoom.LeaveRoom(this);
                currentRoom = target.TargetDoor.GetOtherRoom(currentRoom);
                currentRoom.EnterRoom(this);

                //Currently, the goal can only be in the neighbouring room. If this changes, change here.
                target.SetTargetToPostDoor();

                break;
            case TargetType.PostDoor:
//                target.SetTargetToWorkstation();
                CalculateAndSetRoute();
                break;
            case TargetType.Workstation:
                target.TargetWorkStation.EnterWorkstation(this);
                PlayWorkstationAnimation(target.TargetWorkStation.workstationType);
                if (target.TargetWorkStation.facingDown)
                {
                    charDisp.FaceDown();
                }
                else
                {
                    charDisp.FaceUp();
                }

                if(target.TargetWorkStation == finalWorkstation)
                {
                    GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

                    gameController.RegisterFinalArrival();
                    state = State.Fin;
                }
                else
                {
                    state = State.Working;
                }

                target.Clear();

                break;
            default:
                //What
                break;
        }
    }

    private void PlayWorkstationAnimation(WorkStation.WorkstationType workstationType)
    {
        switch(workstationType)
        {
            case WorkStation.WorkstationType.Piss:
                charDisp.StartPiss();
                break;
            case WorkStation.WorkstationType.Shit:
                charDisp.StartShit();
                break;
            case WorkStation.WorkstationType.Eat:
                charDisp.StartEat();
                break;
            case WorkStation.WorkstationType.Vend:
                charDisp.StartVend();
                break;
            case WorkStation.WorkstationType.Present:
                charDisp.StartPresent();
                break;
            case WorkStation.WorkstationType.Sit:
                charDisp.StartSit();
                break;
            case WorkStation.WorkstationType.Print:
                charDisp.StartPrint();
                break;
            case WorkStation.WorkstationType.Work:
                charDisp.StartWork();
                break;
        }
    }

    public void GoToNPC(Character npc)
    {
        movingToNpc = true;
        moveToNpcPos = npc.gameObject.transform.position;
    }
}
