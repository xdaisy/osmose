using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacing : MonoBehaviour {

    private Animator anim; // animatator for this object

    private float timeToWait; // the time we wait before change facing direction
    private float timeElapsed; // time that has elapsed

    private Dialogue dMang; // dialogue manager
    private bool canMove; // determine if npc can "move"

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        timeElapsed = 0f;
        timeToWait = Random.Range(2f, 5f); // wait between 1 to 3 sec
        canMove = true;
        dMang = FindObjectOfType<Dialogue>(); // get the dialogue manager
	}
	
	// Update is called once per frame
	void Update () {

        if(!dMang.getDialogueActive()) {
            // set canMove back to true if dialogue is not active aka no dialogue on screen
            canMove = true;
        }

        if (!canMove) {
            // if npc cannot move aka change directions, do nothing
            return;
        }

        timeElapsed += Time.deltaTime;

        if (canMove && timeElapsed > timeToWait) {

            // get a random num for the direction
            int dir = Random.Range(0, 4);

            // start off as zero
            float x = 0f;
            float y = 0f;

            switch (dir) {
                case 0:
                    // face down
                    y = -1f;
                    break;
                case 1:
                    // face up
                    y = 1f;
                    break;
                case 2:
                    // face left
                    x = -1f;
                    break;
                case 3:
                    // face right
                    x = 1f;
                    break;
            }

            anim.SetFloat("FaceX", x);
            anim.SetFloat("FaceY", y);

            timeToWait = Random.Range(2f, 5f); // set new time to wait
            timeElapsed = 0f; // reset timer
        }
	}

    public void setFaceDirection(float x, float y) {
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }

    public void setCanMove(bool canMove) {
        this.canMove = canMove;
    }

    public bool getCanMove() {
        return this.canMove;
    }
}
