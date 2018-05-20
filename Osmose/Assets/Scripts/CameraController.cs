using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject followTarget; // follows a target in the game
    private Vector3 targetPos; // position of target
    public float moveSpeed; // speed that camera will follow target

    private static bool cameraExists;

    // Use this for initialization
    void Start () {
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z); // set the x and y as the same of the target, set z as same as this object (whatever it's attached to)
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime); // transform the object's position
    }
}
