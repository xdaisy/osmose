using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSpawner : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null) {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<OnlyKeyboardInputModule>();
        }
    }
}
