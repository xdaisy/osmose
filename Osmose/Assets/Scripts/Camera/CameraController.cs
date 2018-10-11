using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject followTarget; // follows a target in the game

    private static bool cameraExists;

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z); // set on player
        offset = transform.position - followTarget.transform.position;
        if (!cameraExists) {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
            offset = transform.position - followTarget.transform.position;
        } else {
            Destroy(gameObject);
        }
    }

    void LateUpdate() {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = followTarget.transform.position + offset;
    }
}
