using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapUI : MonoBehaviour {
    [Header("Groups")]
    public CanvasGroup NodeGroup;
    public CanvasGroup AreaGroup;

    [Header("Select Area Popup UI")]
    public AreaPopUp AreaPopUpUi;

    [Header("Different regions and areas")]
    [SerializeField]
    private List<string> forestRegion;

    private EventSystem eventSystem;
    private const string FOREST = "forest";

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;
    }

    public void SelectRegion(Node node) {
        if (node.GetCanGo()) {
            // map avatar stopped moving and can go to select area
            NodeGroup.interactable = false;
            AreaGroup.gameObject.SetActive(true);

            string region = node.name.Split(' ')[0].ToLower();

            AreaPopUpUi.OpenPopUp(getAreasInRegion(region));
        }
    }

    public void SelectArea(string area) {

    }

    private List<string> getAreasInRegion(string region) {
        if (region.Equals(FOREST)) {
            return new List<string>(forestRegion);
        }
        return new List<string>();
    }
}
