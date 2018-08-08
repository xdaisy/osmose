using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float moveSpeed; // move speed for the character

    private Animator anim; // reference to animator

    private Rigidbody2D myRigidBody; // reference to ridgebody, use to add force, will push against collision boxes instead of bouncing off

    private bool playerMoving; // keep track if player is moving or not
    public Vector2 lastMove; // keep track if player was moving up/down or left/right

    private static bool playerExists; // keep track if player exist
    // all object with playercontroller attach will have playerExists bool

    private Vector2 vel; // need this for FixedUpdate so that can move in x-direction 

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>(); // get animator for player
        myRigidBody = GetComponent<Rigidbody2D>(); // get rigidbody2d

        // don't destroy object on load if player don't exist
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            // if another player exists, destroy game object
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        playerMoving = false; // default to false at start of every frame

        // if player is pressing right or left
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody.velocity.y); // change x-axis velocity for rigidbody
            playerMoving = true; // is moving right/left so true
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f); // last move input for x direction
        }

        // if player is pressing up or down
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed); // change y-axis velocity for rigidbody
            playerMoving = true; // is moving up/down so true
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical")); // last move input for y direction
        }

        // no force if no input from controls in x direction
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
        }
        // no force if no input from controls in y direction
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
        }

        vel = myRigidBody.velocity;

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal")); // set MoveX var in animator
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical")); // set MoveY var in animator
        anim.SetBool("PlayerMoving", playerMoving); // set PlayerMoving var in animator
        anim.SetFloat("LastMoveX", lastMove.x); // set LastMoveX var in animator
        anim.SetFloat("LastMoveY", lastMove.y); // set LastMoveY var in animator
    }

    void FixedUpdate()
    {
        myRigidBody.velocity = vel;
    }
}
