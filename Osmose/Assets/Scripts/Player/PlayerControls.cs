using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float MoveSpeed; // move speed for the character

    public static PlayerControls Instance; // keep track if player exist

    public string PreviousAreaName; // keep track of previous area player was in

    private Animator anim; // reference to animator
    private Rigidbody2D myRigidBody; // reference to ridgebody, use to add force, will push against collision boxes instead of bouncing off
    private Vector2 lastMove; // keep track if player was moving up/down or left/

    private bool canMove; // determines if player can move or not

    private bool menuOpen;

    private EnterBattle enterBattle;

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
        enterBattle = GetComponent<EnterBattle>();

        canMove = true;
        menuOpen = false;
    }
	
	// Update is called once per frame
	void Update () {}

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.P)) {
            // testing save
            SaveFileManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            // loading save
            SaveFileManager.Load();
        }

        if (GameManager.Instance.CanOpenMenu() && Input.GetButtonDown("OpenMenu")) {
            // if click m, do open/close menu
            if (menuOpen) {
                // close menu
                Menu.Instance.CloseGameMenu();
                menuOpen = false;
            } else {
                // open menu
                Menu.Instance.OpenGameMenu();
                menuOpen = true;
            }
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (canMove) {
            myRigidBody.velocity = new Vector2(xInput, yInput) * MoveSpeed;
        } else {
            // if can't move, make the velocity zero so not moving
            myRigidBody.velocity = Vector2.zero;
        }

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

    // set the player to face forward
    // use for after the cutscene
    public void SetPlayerForward() {
        anim.SetFloat("LastMoveX", 0);
        anim.SetFloat("LastMoveY", -1f);
    }

    // return the values for LastMoveX and LastMoveY
    public Vector2 GetLastMove() {
        return new Vector2(anim.GetFloat("LastMoveX"), anim.GetFloat("LastMoveY"));
    }

    public void SetLastMove(Vector2 lastMove) {
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    public void SetPosition(Vector3 newPos) {
        transform.position = newPos;
        enterBattle.UpdatePos();
    }
}
