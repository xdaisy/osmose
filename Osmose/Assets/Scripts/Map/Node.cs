using UnityEngine;

public class Node : MonoBehaviour {
    private bool canGo;

    // Start is called before the first frame update
    void Start() {
        canGo = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canGo = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canGo = false;
        }
    }

    public bool GetCanGo() {
        return canGo;
    }
}
