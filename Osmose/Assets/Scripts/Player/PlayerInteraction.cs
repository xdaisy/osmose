using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    private GameObject currentInterObject = null;
    private InteractionObject currInterObjScript = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Interact") && currentInterObject) {
            // if the button is the interact button, interact with interactable object
            if (currInterObjScript.talks) {
                NPCFacing faceDir = currentInterObject.GetComponent<NPCFacing>(); // have if npc
                if (faceDir != null) {
                    Animator anim = GetComponent<Animator>(); // get the animator for player
                    float x = anim.GetFloat("LastMoveX");
                    float y = anim.GetFloat("LastMoveY");
                    faceDir.setFaceDirection(-1f * x, -1f * y); // set the npc to face the player
                    faceDir.setCanMove(false); // make the npc not move during dialogue
                }
                // talks if interactable object has a message
                currInterObjScript.Talk();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("interactableObject") || other.CompareTag("NPC")) {
            currentInterObject = other.gameObject; // if game object of other is interable, set currentInterObject to it
            currInterObjScript = currentInterObject.GetComponent<InteractionObject>(); // get the object's interaction 
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("interactableObject") || other.CompareTag("NPC")) {
            if (other.gameObject == currentInterObject) {
                currentInterObject = null; // if getting out of range of interable object, set currentInterObject to null
                currInterObjScript = null; // set to null if get out of range of interactable object
            }
        }
    }
}
