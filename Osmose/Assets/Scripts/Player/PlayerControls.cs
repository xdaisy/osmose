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

    public static PlayerControls Instance; // keep track if player exist

    public string PreviousAreaName; // keep track of previous area player was in

    private bool canMove; // determines if player can move or not

    private float amountPlayerMoved; // keep track how much the player has moved
    private float movementTilEncounter; // amount of movement before player launch into battle

    private void Awake() {
        // don't destroy object on load if player don't exist
        if (Instance == null) {
            Instance = this;
        } else {
            // if another player exists, destroy game object
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>(); // get animator for player
        myRigidBody = GetComponent<Rigidbody2D>(); // get rigidbody2d

        canMove = true;
        amountPlayerMoved = 0f;
        movementTilEncounter = UnityEngine.Random.Range(300f, 500f);
    }
	
	// Update is called once per frame
	void Update () {}

    void FixedUpdate() {

        if (IsBattleMap && amountPlayerMoved >= movementTilEncounter) {
            Debug.Log("Random encounter");
            amountPlayerMoved = 0f; // reset the amount that the player has moved
            movementTilEncounter = UnityEngine.Random.Range(300f, 500f); // new amount of movement until random encounter
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (canMove) {
            myRigidBody.velocity = new Vector2(xInput, yInput) * MoveSpeed;
        } else {
            // if can't move, make the velocity zero so not moving
            myRigidBody.velocity = Vector2.zero;
        }

        playerMoving = false; // default to false at start of every frame

        anim.SetFloat("MoveX", myRigidBody.velocity.x); // set MoveX var in animator
        anim.SetFloat("MoveY", myRigidBody.velocity.y); // set MoveY var in animator

        if (xInput == 1 || xInput == -1 || yInput == 1 || yInput == -1) {
            // if player press any of controls keys, set the last move to it
            if (canMove) {
                anim.SetFloat("LastMoveX", xInput); // set LastMoveX var in animator
                anim.SetFloat("LastMoveY", yInput); // set LastMoveY var in animator
            }
        }
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    public bool GetCanMove() {
        return this.canMove;
    }
}
