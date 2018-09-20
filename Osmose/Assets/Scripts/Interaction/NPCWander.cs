using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWander : MonoBehaviour {

    public float moveSpeed; // move speed for the character

    private Animation anim; // reference to animator

    private Rigidbody2D myRigidBody; // reference to ridgebody, use to add force, will push against collision boxes instead of bouncing off
    
    private Vector2 lastMove; // keep track if player was moving up/down or left/right

    private float waitTime; // time elapsed before character do something
    private float timeElapsed; // current amount of time elapsed

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>(); // get animator for npc
        myRigidBody = GetComponent<Rigidbody2D>(); // get rigidbody2d
        waitTime = 3f;
        timeElapsed = 0f;
        anim.Play("NPC_face_down");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        anim.Play("NPC_face_right");
        timeElapsed += Time.deltaTime;

        // initially don't move
        myRigidBody.velocity = new Vector2(0f, 0f);

        if (timeElapsed >= waitTime) {
            int movement = Random.Range(0, 4); // get an int from 1 to 4
            anim.Stop();

            switch (movement) {
                case 1:
                    // move right
                    anim.Play("NPC_move_right");
                    myRigidBody.velocity = new Vector2(1f * moveSpeed, myRigidBody.velocity.y);
                    lastMove = new Vector2(1f, 0f);
                    anim.Stop();
                    anim.Play("NPC_face_right");
                    break;
                case 2:
                    // move left
                    anim.Play("NPC_move_left");
                    myRigidBody.velocity = new Vector2(-1f * moveSpeed, myRigidBody.velocity.y);
                    lastMove = new Vector2(-1f, 0f);
                    anim.Play("NPC_face_right");
                    break;
                case 3:
                    // move up
                    anim.Play("NPC_move_up");
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 1f * moveSpeed);
                    lastMove = new Vector2(0f, 1f);
                    anim.Play("NPC_face_right");
                    break;
                case 4:
                    // move down
                    anim.Play("NPC_move_down");
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, -1f * moveSpeed);
                    lastMove = new Vector2(0f, -1f);
                    anim.Play("NPC_face_right");
                    break;
            }
            
            timeElapsed = 0f;
        }
    }
}
