using System.Collections.Generic;
using UnityEngine;

public class TutorialInvestigate : MonoBehaviour {
    [SerializeField] private SceneName sceneToLoad;
    [SerializeField] private string[] dialogue;

    private bool canActivate = false;
    private bool showingDialogue = false;

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && canActivate && !showingDialogue) {
            showingDialogue = true;
            Dialogue.Instance.ShowDialogue(dialogue, false);
        } else if (showingDialogue && !Dialogue.Instance.GetDialogueActive()) {
            LoadSceneLogic.Instance.LoadScene(sceneToLoad.GetSceneName());
        }
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
