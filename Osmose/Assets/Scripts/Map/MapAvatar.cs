using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles how the map avatar move on the map
/// </summary>
public class MapAvatar : MonoBehaviour {
    public float MoveSpeed;

    private EventSystem eventSystem;

    private Vector3 offset;
    private bool isMoving;

    private void Awake() {
        eventSystem = EventSystem.current;
        isMoving = false;
        offset = new Vector3(2, 59, 0);
    }

    // Update is called once per frame
    void Update() {
        if (eventSystem.currentSelectedGameObject.GetComponent<Node>() != null) {
            Vector3 nodePos = eventSystem.currentSelectedGameObject.transform.position + offset;
            if (this.transform.position != nodePos) {
                // move if avatar is not on top of current node
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    nodePos,
                    MoveSpeed + Time.deltaTime
                );
                isMoving = true;
            } else {
                isMoving = false;
            }
        }
    }
}
