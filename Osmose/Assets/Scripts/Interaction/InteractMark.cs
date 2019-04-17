using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMark : MonoBehaviour {
    public GameObject InteractSprite;
    public float YOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
