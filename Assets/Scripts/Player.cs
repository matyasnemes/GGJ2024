using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /**
     * Movement speed of the player.
     */
    public float MovementSpeed = 10.0f;

    /**
     * RigidBody of the player for colliding with solid objects.
     */
    private Rigidbody2D rigidBody;

    /**
     * Character display for drawing and animating the player character.
     */
    private CharacterDisplay characterDisplay;

    /**
     * The room in which the player currently resides.
     */
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

    /**
     * Initializes the player.
     */
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        characterDisplay = GetComponentInChildren<CharacterDisplay>();
    }

    /**
     * Called when the player collides with a trigger (used for entering rooms).
     * @param[in] collider: Collider the player collided with.
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        Room room = collider.gameObject.GetComponent<Room>();
        if (room)
        {
            Debug.Log("Player entered room: " + room.name);
            currentRoom = room;
        }
    }

    /**
     * Called when the player stops colliding with a trigger (used for exiting rooms).
     * @param[in] collider: Collider the player stopped collidings with.
     */
    void OnTriggerExit2D(Collider2D collider)
    {
        Room room = collider.gameObject.GetComponent<Room>();
        if (room)
        {
            Debug.Log("Player left room: " + room.name);
        }
    }

    /**
     * Update called with a fixed interval within the physics simulation.
     * Used for handling movement input commands.
     */
    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        characterDisplay.FaceDown();

        if (Input.GetKey(KeyCode.W)) 
        { 
            velocity += Vector2.up * MovementSpeed;
            characterDisplay.FaceUp();
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            velocity += Vector2.down * MovementSpeed;
            characterDisplay.FaceDown();
        }

        if (Input.GetKey(KeyCode.A)) 
        { 
            velocity += Vector2.left * MovementSpeed;
        }
        else if (Input.GetKey(KeyCode.D)) 
        { 
            velocity += Vector2.right * MovementSpeed;
        }

        rigidBody.velocity = velocity;
    }
}
