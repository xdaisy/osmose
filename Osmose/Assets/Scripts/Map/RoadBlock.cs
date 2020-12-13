using UnityEngine;

public class RoadBlock : MonoBehaviour {
    [SerializeField] private string[] dialogue;
    [SerializeField] private Vector2 direction;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            PlayerControls.Instance.PushPlayer(direction);
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }
}
