using UnityEngine;

/// <summary>
/// Class that handles unlocking a scene after talking to a specific NPC
/// </summary>
public class UnlockArea : MonoBehaviour {
    public Clue ObtainedClue;

    public SceneName SceneToUnlock;

    public string[] PreUnlockedDialogue; // lines of generic dialogue

    public string[] PostUnlockedDialogue;

    private bool canActivate = false;

    // Start is called before the first frame update
    void Start() {
        if (CluesManager.Instance.DidObtainClue(ObtainedClue.GetClueNumber())) {
            // if clue was gotten, disable this
            this.enabled = false;
        } else {
            this.enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            if (EventManager.Instance.DidEventHappened(SceneToUnlock.GetSceneName())) {
                // if scene was unlocked
                Dialogue.Instance.ShowDialogue(this.PostUnlockedDialogue, false);
            } else {
                // scene was not unlocked
                EventManager.Instance.AddEvent(SceneToUnlock.GetSceneName());
                Dialogue.Instance.ShowDialogue(this.PreUnlockedDialogue, false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = false;
        }
    }
}
