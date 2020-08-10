using UnityEngine;

/// <summary>
/// Class for the NPC behaviors
/// </summary>
public class NPC : MonoBehaviour {
    public SpriteRenderer NpcSprite;
    [SerializeField] private Sprite DownSprite;
    [SerializeField] private Sprite UpSprite;
    [SerializeField] private Sprite LeftSprite;
    [SerializeField] private Sprite RightSprite;

    private bool canInteract = false;
    private Vector2 direction;

    // Update is called once per frame
    void Update() {
        if (canInteract && Input.GetButtonDown("Interact")) {
            // if interacting with npc
            if (direction.y > 0) {
                // npc face up
                NpcSprite.sprite = UpSprite;
            } else if (direction.y < 0) {
                // npc face down
                NpcSprite.sprite = DownSprite;
            } else if (direction.x > 0) {
                // npc face right
                NpcSprite.sprite = RightSprite;
            } else if (direction.x < 0) {
                // npc face left
                NpcSprite.sprite = LeftSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            direction = other.GetComponent<PlayerControls>().GetLastMove() * -1; // want the direction to be the inverse of the player's last movement
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canInteract = false;
        }
    }
}
