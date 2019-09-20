using UnityEngine;

/// <summary>
/// Class that handles whether the player can go to a region
/// </summary>
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

    /// <summary>
    /// Get a flag indicating whether or not the player can click on the node to go to the region
    /// </summary>
    /// <returns>true if the player can click, false otherwise</returns>
    public bool GetCanGo() {
        return canGo;
    }
}
