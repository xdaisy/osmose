using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacing : MonoBehaviour {

    private Animator anim; // animatator for this object

    private float timeToWait; // the time we wait before change facing direction
    private float timeElapsed; // time that has elapsed

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        timeElapsed = 0f;
        timeToWait = Random.Range(1f, 3f); // wait between 1 to 3 sec
	}
	
	// Update is called once per frame
	void Update () {
        /*timeElapsed += Time.deltaTime;

        if (timeElapsed > timeToWait) {
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

            timeToWait = Random.Range(1f, 3f); // set new time to wait
            timeElapsed = 0f; // reset timer
        }*/
	}

    public void setFaceDirection(float x, float y) {
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }
}
