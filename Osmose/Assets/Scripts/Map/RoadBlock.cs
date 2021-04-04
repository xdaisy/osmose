using UnityEngine;

public class RoadBlock : MonoBehaviour {
    [SerializeField] private string[] dialogue;
    [SerializeField] private Vector2 direction;
    [SerializeField] private string eventName;
    [SerializeField] private Clue clue;

    void Start() {
        if (
            (eventName != null && eventName.Length > 0 && EventManager.Instance.DidEventHappened(eventName)) ||
            (clue != null && CluesManager.Instance.DidObtainClue(clue.GetClueNumber()))
           ) {
            // if there is an event to unblock, destroy block
            // or if there is a clue to unblock, destory block
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            PlayerControls.Instance.PushPlayer(direction);
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }
}
