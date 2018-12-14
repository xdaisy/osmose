using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float MoveSpeed; // move speed for the character

    public bool IsBattleMap; // determines if battle occurs on map

    private Animator anim; // reference to animator

    private Rigidbody2D myRigidBody; // reference to ridgebody, use to add force, will push against collision boxes instead of bouncing off

    private bool playerMoving; // keep track if player is moving or not
    private Vector2 lastMove; // keep track if player was moving up/down or left/right

    private static bool playerExists; // keep track if player exist
    // all object with playercontroller attach will have playerExists bool

    private bool canMove; // determines if player can move or not

    private float amountPlayerMoved; // keep track how much the player has moved
    private float movementTilEncounter; // amount of movement before player launch into battle

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>(); // get animator for player
        myRigidBody = GetComponent<Rigidbody2D>(); // get rigidbody2d

        canMove = true;
        amountPlayerMoved = 0f;
        movementTilEncounter = UnityEngine.Random.Range(300f, 500f);

        // don't destroy object on load if player don't exist
        if (!playerExists) {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            // if another player exists, destroy game object
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {}

    void FixedUpdate() {

        if (IsBattleMap && amountPlayerMoved >= movementTilEncounter) {
            Debug.Log("Random encounter");
            amountPlayerMoved = 0f; // reset the amount that the player has moved
            movementTilEncounter = UnityEngine.Random.Range(300f, 500f); // new amount of movement until random encounter
        }

        if (!canMove) {
            // if can't move, make the velocity zero so not moving
            myRigidBody.velocity = Vector2.zero;
            // set so that the walking animation does not play
            anim.SetFloat("MoveX", 0f);
            anim.SetFloat("MoveY", 0f);
            return;
        }

        playerMoving = false; // default to false at start of every frame

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        myRigidBody.velocity = new Vector2(xInput, yInput) * MoveSpeed;

        anim.SetFloat("MoveX", xInput); // set MoveX var in animator
        anim.SetFloat("MoveY", yInput); // set MoveY var in animator

        if (xInput == 1 || xInput == -1 || yInput == 1 || yInput == -1) {
            // if player press any of controls keys, set the last move to it
            anim.SetFloat("LastMoveX", xInput); // set LastMoveX var in animator
            anim.SetFloat("LastMoveY", yInput); // set LastMoveY var in animator
        }
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    public bool GetCanMove() {
        return this.canMove;
    }
}
