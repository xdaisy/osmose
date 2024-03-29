﻿using System.Collections.Generic;
using UnityEngine;

public class Investigate : MonoBehaviour {
    public Clue Clue;
    [SerializeField] private SceneName sceneToUnlock; // scene to unlock
    [SerializeField] private SceneName lockedByScene; // scene to unlock
    [SerializeField] private bool KeenEyes; // is Aren's ability?

    [Header("Dialogue")]
    public string[] PreObtainDialogue; // lines of generic dialogue
    public string[] PostObtainDialogue; // lines of dialogue for before/after specific event
    public string[] AbilityDialogue;
    public string[] PreInvestigationDialogue;

    private bool canActivate = false;

    void Start() {
        if (lockedByScene != null && (PreInvestigationDialogue == null || PreInvestigationDialogue.Length == 0) && !EventManager.Instance.DidEventHappened(lockedByScene.GetSceneName())) {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && canShowDialogue()) {
            string[] dialogue = getDialogue();
            int sfx = -1;
            if (canAddClue()) {
                // did not investigate yet
                sfx = 2;
                CluesManager.Instance.ObtainedClue(Clue.GetClueNumber());
                GameManager.Instance.AddClue(Clue);
                if (sceneToUnlock != null) {
                    EventManager.Instance.AddEvent(sceneToUnlock.GetSceneName());
                }
            }
            Dialogue.Instance.ShowDialogue(dialogue, false, sfx);
        }
    }

    /// <summary>
    /// Determine whether or not to show dialogue
    /// </summary>
    /// <returns>True if can show dialogue, false otherwise</returns>
    private bool canShowDialogue() {
        bool haveDialogue = haveInvestigated() ? PostObtainDialogue.Length > 0 : true;
        return haveDialogue && canActivate && GameManager.Instance.CanStartDialogue() &&  !Dialogue.Instance.dBox.activeSelf;
    }

    /// <summary>
    /// Get the correct dialogue to show and append getting item if there's an item obtained
    /// </summary>
    /// <returns>Correct dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue;
        if (lockedByScene != null && !EventManager.Instance.DidEventHappened(lockedByScene.GetSceneName())) {
            dialogue = new List<string>(this.PreInvestigationDialogue);
        } else if (haveInvestigated()) {
            dialogue = new List<string>(this.PostObtainDialogue);
        } else if (KeenEyes && GameManager.Instance.IsLeader(Constants.AREN)) {
            // if aren is in front and item needs keen eyes to show
            Dialogue.Instance.ShowClueImage(Clue.GetSprite());
            dialogue = new List<string>(this.AbilityDialogue);
        } else {
            dialogue = new List<string>(this.PreObtainDialogue);
        }

        return dialogue.ToArray();
    }

    /// <summary>
    /// Get whether or not the clue can be added to the inventory
    /// </summary>
    /// <returns>True if can add clue to the inventory, false otherwise</returns>
    private bool canAddClue() {
        bool investigated = (lockedByScene == null || EventManager.Instance.DidEventHappened(lockedByScene.GetSceneName())) && !haveInvestigated();
        if (KeenEyes) {
            // aren's special ability
            return GameManager.Instance.IsLeader(Constants.AREN) && investigated;
        }
        return investigated;
    }

    /// <summary>
    /// Return whether or not have been investigated
    /// </summary>
    /// <returns>True if have investigated, false otherwise</returns>
    private bool haveInvestigated() {
        return CluesManager.Instance.DidObtainClue(Clue.GetClueNumber());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = false;
        }
    }
}
