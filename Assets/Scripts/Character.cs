using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float eps = 0.0001;

    enum State
    {
        Going,
        Working,
        DoneWorking,
    }

    enum Target
    {
        Door,
        Workstation
    }

    State state;

    public Vector2 goal = null;

    Target currentTarget;
    Room currentRoom;
    Room targetRoom;
    WorkStation targetWorkstation;

    /**
     * Requests the character to leave its current workstation.
     * The workstation unregisters the character after this method is done.
     */
    public void RequestLeaveWorkstation(WorkStation workStation)
    {
        if(state == Working)
        {
            state = DoneWorking;
        } 
        else
        {
            Debug.Log("<color=red>Error: </color> Invalid state change to DoneWorking");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Generate random workstation as target
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case Going:
                /*
                *   Go
                *   If reached targed, do the thing
                *   Room registration
                */
            break;
            case Working:

                //Work, work

            break;
            case DoneWorking:
            {
                /*
                * Deregister from workstation
                * Find new goal
                * Set state
                */
            }
            default:
                //What
            break
        }
        
    }
}
