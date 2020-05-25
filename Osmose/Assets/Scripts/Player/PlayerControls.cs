using UnityEngine;

/// <summary>
/// Class that handles the player's movement on the map
/// </summary>
public class PlayerControls : MonoBehaviour {

    public float MoveSpeed; // move speed for the character

    public static PlayerControls Instance; // keep track if player exist

    private Animator anim; // reference to animator
    private Rigidbody2D myRigidBody; // reference to ridgebody, use to add force, will push against collision boxes instead of bouncing off
    private Vector2 lastMove; // keep track if player was moving up/down or left/

    private bool canMove; // determines if player can move or not

    private bool menuOpen;

    // Use this for initialization
    void Start () {
        // don't destroy object on load if player don't exist
        if (Instance == null) {
            Instance = this;
        } else {
            // if another player exists, destroy game object
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        anim = GetComponent<Animator>(); // get animator for player
        myRigidBody = GetComponent<Rigidbody2D>(); // get rigidbody2d

        canMove = true;
        menuOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.CanOpenMenu() && Input.GetKeyDown(KeyCode.M)) {
            // if click open menu button, open/close menu
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

        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
    }

    void FixedUpdate() {
        // move
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

    /// <summary>
    /// Set whether or not the player can move
    /// </summary>
    /// <param name="canMove">Flag that indicates whether or not player can move</param>
    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    /// <summary>
    /// Return a flag of whether or not the player can move
    /// </summary>
    /// <returns>true if the player can move, false otherwise</returns>
    public bool GetCanMove() {
        return this.canMove;
    }

    /// <summary>
    /// Set the player to face forward
    /// </summary>
    public void SetPlayerForward() {
        // set the player to face forward
        // use for after the cutscene or loading into scene when defeated in battle
        anim.SetFloat("LastMoveX", 0);
        anim.SetFloat("LastMoveY", -1f);
    }

    /// <summary>
    /// Returns the x and y values for the player's last movement
    /// </summary>
    /// <returns>Vector2 with values of the player's last movement</returns>
    public Vector2 GetLastMove() {
        return new Vector2(anim.GetFloat("LastMoveX"), anim.GetFloat("LastMoveY"));
    }

    /// <summary>
    /// Set the player to face a direction
    /// </summary>
    /// <param name="lastMove">Vector2 that indicates when direction the player is facing</param>
    public void SetLastMove(Vector2 lastMove) {
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    /// <summary>
    /// Set the position of the player
    /// </summary>
    /// <param name="newPos">Position where want to place the player</param>
    public void SetPosition(Vector3 newPos) {
        transform.position = newPos;
    }
}
