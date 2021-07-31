using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles how the map avatar move on the map
/// </summary>
public class MapAvatar : MonoBehaviour {
    public float MoveSpeed;

    private Vector3 offset;

    private void Awake() {
        offset = new Vector3(2, 59, 0);
    }

    // Update is called once per frame
    void Update() {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Node>() != null) {
            Vector3 nodePos = EventSystem.current.currentSelectedGameObject.transform.position + offset;
            if (this.transform.position != nodePos) {
                // move if avatar is not on top of current node
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    nodePos,
                    MoveSpeed + Time.deltaTime
                );
            }
        }
    }
}
