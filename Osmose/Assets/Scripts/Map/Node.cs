using UnityEngine;

/// <summary>
/// Class that handles whether the player can go to a region
/// </summary>
public class Node : MonoBehaviour {
    [SerializeField]
    private string RegionName;
    private bool canGo;

    // Start is called before the first frame update
    void Start() {
        if (!EventManager.Instance.DidEventHappened(RegionName)) {
            // if haven't unlocked, delete node
            Destroy(gameObject);
        }
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

    /// <summary>
    /// Get the region's name from the node
    /// </summary>
    /// <returns>String of the region's name</returns>
    public string GetRegionName() {
        return RegionName;
    }
}
