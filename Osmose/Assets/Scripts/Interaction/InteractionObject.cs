using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public bool talks; // if true, object here can talk to player

    public string message; // message this object will give to player

    private Dialogue dialogue; // dialogue manager that will send the message to

    // Use this for initialization
    void Start() {
        dialogue = FindObjectOfType<Dialogue>();
    }

    // Update is called once per frame
    void Update() {

    }

    void DoInteraction() {

    }

    // talk if this object has a message
    public void Talk() {
        //dialogue.showText(message);
        Debug.Log(message);
    }
}
