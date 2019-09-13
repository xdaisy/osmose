using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapUI : MonoBehaviour {
    [Header("Groups")]
    public CanvasGroup NodeGroup;
    public CanvasGroup AreaGroup;

    [Header("Select Area Popup UI")]
    public AreaPopUp AreaPopUpUi;

    [Header("Different regions and areas")]
    [SerializeField]
    private List<string> forestRegion;

    [Header("Load Fields")]
    [SerializeField]
    private float waitToLoad = 1f;

    private EventSystem eventSystem;
    private const string FOREST = "forest";

    private Node currentNode;

    private bool shouldLoadAfterFade;
    private string region;
    private string area;

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update() {
        if (shouldLoadAfterFade) {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(area);
            }
        }

        if (AreaGroup.interactable && Input.GetButtonDown("Cancel")) {
            // if on area pop up and press cancel, go back to being on map
            AreaGroup.gameObject.SetActive(false);
            NodeGroup.interactable = true;

            eventSystem.SetSelectedGameObject(currentNode.gameObject);
        }
    }

    public void SelectRegion(Node node) {
        if (node.GetCanGo()) {
            // map avatar stopped moving and can go to select area
            NodeGroup.interactable = false;
            AreaGroup.gameObject.SetActive(true);

            currentNode = node;

            string region = node.name.Split(' ')[0].ToLower();
            this.region = region;

            AreaPopUpUi.OpenPopUp(getAreasInRegion(region));
        }
    }

    public void SelectArea(int index) {
        List<string> areas = getAreasInRegion(region);

        shouldLoadAfterFade = true;
        UIFade.Instance.FadeToBlack();
        PlayerControls.Instance.PreviousAreaName = Constants.MAP;
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.CurrentScene = areas[index];
        PlayerControls.Instance.SetLastMove(Vector2.down);

        area = areas[index];
    }

    private List<string> getAreasInRegion(string region) {
        if (region.Equals(FOREST)) {
            return new List<string>(forestRegion);
        }
        return new List<string>();
    }
}
