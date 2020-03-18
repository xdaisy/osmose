using System.Collections;
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
    
    private string region;
    private string area;

    private GameObject prevButton;

    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.FadingBetweenAreas = false;
        UIFade.Instance.FadeFromBlack();
        GameManager.Instance.OnMap = true;

        Node firstNode = GameObject.FindObjectsOfType<Node>()[0];
        EventSystem.current.SetSelectedGameObject(firstNode.gameObject);
        prevButton = firstNode.gameObject;
    }

    // Update is called once per frame
    void Update() {
        if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            && prevButton != EventSystem.current.currentSelectedGameObject
        ) {
            playClick();
            prevButton = EventSystem.current.currentSelectedGameObject;
        }
        if (AreaGroup.gameObject.activeSelf && Input.GetButtonDown("Cancel")) {
            // if on area pop up and press cancel, go back to being on map
            playClick();

            AreaGroup.gameObject.SetActive(false);
            NodeGroup.interactable = true;

            EventSystem.current.SetSelectedGameObject(currentNode.gameObject);
        } else if (Input.GetButtonDown("Cancel")) {
            // go back to previous scene
            playClick();

            string sceneName = PlayerControls.Instance.PreviousAreaName;

            PlayerControls.Instance.PreviousAreaName = Constants.MAP;
            GameManager.Instance.OnMap = false;
            
            LoadSceneLogic.Instance.LoadScene(sceneName);
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

        GameManager.Instance.CurrentScene = areas[index];
        PlayerControls.Instance.SetLastMove(Vector2.up);

        PlayerControls.Instance.PreviousAreaName = Constants.MAP;
        GameManager.Instance.OnMap = false;

        LoadSceneLogic.Instance.LoadScene(areas[index]);
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

    /// <summary>
    /// Wait and then load scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadScene() {
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(area);
    }
}
