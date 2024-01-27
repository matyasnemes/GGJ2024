using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        float speed = 10.0f;

        if (Input.GetKey(KeyCode.W)) 
        { 
            velocity += Vector2.up * speed;
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            velocity += Vector2.down * speed;
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
