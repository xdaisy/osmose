using UnityEngine;

public class UpdateClue : MonoBehaviour {
    public Clue Clue;

    public string[] preUpdateDialogue; // lines of generic dialogue

    public string[] postUpdateDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    // Start is called before the first frame update
    void Start() {
        if (!CluesManager.Instance.DidObtainClue(Clue.GetClueNumber())) {
            // cannot update clue if clue was not obtained
            this.enabled = false;
        } else {
            this.enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            if (CluesManager.Instance.DidUpdateClue(Clue.GetClueNumber())) {
                // if clue got updated
                Dialogue.Instance.ShowDialogue(this.postUpdateDialogue, false);
            } else {
                // clue did not get updated
                CluesManager.Instance.UpdateClue(Clue.GetClueNumber());
                Dialogue.Instance.ShowDialogue(this.preUpdateDialogue, false);
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
