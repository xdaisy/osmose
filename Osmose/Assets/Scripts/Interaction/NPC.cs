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

    // Update is called once per frame
    void Update() {
        if (canInteract && Input.GetButtonDown("Interact")) {
            // if interacting with npc
            float degree = GetDirectionToPlayer();
            if (degree < 45.0f || degree > 315.0f) {
                // npc face down
                NpcSprite.sprite = DownSprite;
                PlayerControls.Instance.SetLastMove(Vector2.up);
            } else if (degree > 45.0f && degree < 135.0f) {
                // npc face down
                NpcSprite.sprite = LeftSprite;
                PlayerControls.Instance.SetLastMove(Vector2.right);
            } else if (degree > 135.0f && degree < 225.0f) {
                // npc face right
                NpcSprite.sprite = UpSprite;
                PlayerControls.Instance.SetLastMove(Vector2.down);
            } else if (degree > 225.0f && degree < 315.0f) {
                // npc face left
                NpcSprite.sprite = RightSprite;
                PlayerControls.Instance.SetLastMove(Vector2.left);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canInteract = false;
        }
    }

    private float GetDirectionToPlayer() {
        Transform playerTransform = PlayerControls.Instance.GetComponent<Transform>();
        Vector3 direction = (this.transform.position - playerTransform.position).normalized;

        float angleRadian = Mathf.Atan2(direction.x, direction.y);

        float angleDegree = angleRadian * Mathf.Rad2Deg;
        if (angleDegree < 0.0f) {
            angleDegree += 360.0f;
        }

        return angleDegree;
    }
}
