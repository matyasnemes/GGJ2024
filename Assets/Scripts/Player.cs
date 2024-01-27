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
     * Reference to the singleton joke factory.
     */
    private JokeFactory jokeFactory;

    /**
     * Reference to the inventory of the player.
     */
    private Inventory inventory;

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

        //DisplayJoke(jokestring);
    }

    /**
     * Initializes the player.
     */
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        characterDisplay = GetComponentInChildren<CharacterDisplay>();
        jokeFactory = GameObject.Find("Joker").GetComponent<JokeFactory>();
        inventory = GetComponent<Inventory>();
    }

    /**
     * Called when the player collides with a trigger. Used for 
     * entering rooms and collecting joke papers.
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
        else if (collider.gameObject.name.Contains("JokePaper"))
        {
            if (!inventory.isFull())
            {
                Destroy(collider.gameObject);
                jokeFactory.OnJokePaperCollected();
                inventory.addItem(jokeFactory.createRandomJokeItem());
            }
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

    void DisplayJoke(string joke)
    {
        //Around 250 characters can be displayed on the textbox
        if(joke.Length > 250)
        {
            Debug.LogError("The following joke is too long: " + joke);
            return;
        }

        
    }
}
