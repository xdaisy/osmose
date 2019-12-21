using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles all the map UI
/// </summary>
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
    private float waitToLoad = Constants.WAIT_TIME;
    
    private const string FOREST = "forest";

    private Node currentNode;

    private bool shouldLoadAfterFade;
    private string region;
    private string area;

    private GameObject prevButton;

    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.FadingBetweenAreas = false;
        UIFade.Instance.FadeFromBlack();
        GameManager.Instance.OnMap = true;

        prevButton = EventSystem.current.firstSelectedGameObject;
    }

    // Update is called once per frame
    void Update() {
        if (prevButton != EventSystem.current.currentSelectedGameObject) {
            playClick();
            prevButton = EventSystem.current.currentSelectedGameObject;
        }

        if (shouldLoadAfterFade) {
            // loading
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(area);
            }
        } else {
            // not loading
            if (AreaGroup.gameObject.activeSelf && Input.GetButtonDown("Cancel")) {
                // if on area pop up and press cancel, go back to being on map
                playClick();

                AreaGroup.gameObject.SetActive(false);
                NodeGroup.interactable = true;

                EventSystem.current.SetSelectedGameObject(currentNode.gameObject);
            } else if (Input.GetButtonDown("Cancel")) {
                // go back to previous scene
                playClick();

                shouldLoadAfterFade = true;
                UIFade.Instance.FadeToBlack();
                PlayerControls.Instance.PreviousAreaName = Constants.MAP;
                GameManager.Instance.FadingBetweenAreas = true;
                GameManager.Instance.OnMap = false;

                area = GameManager.Instance.CurrentScene;
            }
        }
    }

    /// <summary>
    /// Select the Region the player wants to go to
    /// </summary>
    /// <param name="node">Region's node which the player selected</param>
    public void SelectRegion(Node node) {
        if (node.GetCanGo()) {
            // map avatar stopped moving and can go to select area
            playClick();

            NodeGroup.interactable = false;
            AreaGroup.gameObject.SetActive(true);

            currentNode = node;

            this.region = node.GetRegionName();

            AreaPopUpUi.OpenPopUp(getAreasInRegion(region));
            prevButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    /// <summary>
    /// Select the area where the player wants to go to
    /// </summary>
    /// <param name="index">Index that indicates when area the players want to go to</param>
    public void SelectArea(int index) {
        playClick();
        List<string> areas = getAreasInRegion(region);

        shouldLoadAfterFade = true;
        UIFade.Instance.FadeToBlack();
        PlayerControls.Instance.PreviousAreaName = Constants.MAP;
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.OnMap = false;
        GameManager.Instance.CurrentScene = areas[index];
        PlayerControls.Instance.SetLastMove(Vector2.up);

        area = areas[index];
    }

    /// <summary>
    /// Get the list of areas in the region
    /// </summary>
    /// <param name="region">Name of the region that want the list of areas of</param>
    /// <returns>List of area names that are in the region</returns>
    private List<string> getAreasInRegion(string region) {
        if (region.Equals(FOREST)) {
            return new List<string>(forestRegion);
        }
        return new List<string>();
    }

    /// <summary>
    /// Play the click sound effect
    /// </summary>
    private void playClick() {
        SoundManager.Instance.PlaySFX(0);
    }
}
