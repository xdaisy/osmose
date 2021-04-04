using UnityEngine;

public class InteractMark : MonoBehaviour {
    public GameObject InteractSprite;
    public float YOffset;

    public void SetMarkOff() {
        InteractSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "interactableObject") {
            InteractSprite.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + YOffset, InteractSprite.transform.position.z);
            InteractSprite.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "interactableObject") {
            InteractSprite.SetActive(false);
        }
    }
}
