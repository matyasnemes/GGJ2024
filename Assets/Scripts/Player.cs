using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private CharacterDisplay characterDisplay;
    private Room currentRoom = null;

    /**
     * Tells the specified joke type in the current room of the player.
     * @param[in] The type of the joke to tell.
     */
    public void TellJokeInCurrentRoom(JokeItemType jokeType)
    {
        if (currentRoom)
        {
            currentRoom.TellJoke(jokeType);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        characterDisplay = GetComponentInChildren<CharacterDisplay>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Room room = collider.gameObject.GetComponent<Room>();
        if (room)
        {
            Debug.Log("Player entered room: " + room.name);
            currentRoom = room;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Room room = collider.gameObject.GetComponent<Room>();
        if (room)
        {
            Debug.Log("Player left room: " + room.name);
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        characterDisplay.FaceDown();
        float speed = 10.0f;

        if (Input.GetKey(KeyCode.W)) 
        { 
            velocity += Vector2.up * speed;
            characterDisplay.FaceUp();
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            velocity += Vector2.down * speed;
            characterDisplay.FaceDown();
        }

        if (Input.GetKey(KeyCode.A)) 
        { 
            velocity += Vector2.left * speed;
        }
        else if (Input.GetKey(KeyCode.D)) 
        { 
            velocity += Vector2.right * speed;
        }

        rigidBody.velocity = velocity;

        if (Input.GetMouseButtonDown(0)) { Debug.Log("LeftClick"); }
    }
}
