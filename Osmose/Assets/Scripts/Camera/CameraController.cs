using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform Target; // follows a target in the game

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        Target = PlayerControls.Instance.transform;
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z); // set on player
        offset = transform.position - Target.transform.position;
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate() {
        // Set the position of the camera's transform to be the same as the player's except for z position
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
    }
}
