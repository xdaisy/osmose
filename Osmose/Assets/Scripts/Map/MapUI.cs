using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapUI : MonoBehaviour {
    public CanvasGroup NodeGroup;
    public CanvasGroup AreaGroup;

    private EventSystem eventSystem;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SelectRegion(Node node) {
        if (node.GetCanGo()) {
            // map avatar stopped moving and can go to select area
            NodeGroup.interactable = false;
            AreaGroup.gameObject.SetActive(true);
        }
    }

    public void SelectArea(string area) {

    }
}
