using UnityEngine;

public class PreObtainClue : MonoBehaviour {
    public Clue Clue;
    public string[] dialogue;

    private bool canActivate = false;

    // Start is called before the first frame update
    void Start() {
        if (CluesManager.Instance.DidObtainClue(Clue.GetClueNumber())) {
            // do not show if clue was obtained
            this.enabled = false;
        } else {
            this.enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            Dialogue.Instance.ShowDialogue(this.dialogue, false);
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
