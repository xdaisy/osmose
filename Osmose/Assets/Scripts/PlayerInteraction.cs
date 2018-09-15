using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public GameObject currentInterObject = null;
    public InteractionObject currInterObjScript = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Interact") && currentInterObject) {
            // if the button is the interact button, interact with interactable object
            if (currInterObjScript.talks) {
                // talks if interactable object has a message
                currInterObjScript.Talk();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("interactableObject")) {
            currentInterObject = other.gameObject; // if game object of other is interable, set currentInterObject to it
            currInterObjScript = currentInterObject.GetComponent<InteractionObject>(); // get the object's interaction script
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("interactableObject")) {
            if (other.gameObject == currentInterObject) {
                currentInterObject = null; // if getting out of range of interable object, set currentInterObject to null
                currInterObjScript = null; // set to null if get out of range of interactable object
            }
        }
    }
}
