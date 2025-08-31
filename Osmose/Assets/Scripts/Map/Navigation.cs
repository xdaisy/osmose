using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour {
    public SceneName[] Places;
    public Text[] AreaNames;

    // field for playing sound effects
    private GameObject prevButton;

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.NavigationActive && gameObject.activeInHierarchy && EventSystem.current != null && Input.GetButtonDown("Vertical")) {
            GameObject currButton = EventSystem.current.currentSelectedGameObject;

            if (currButton != prevButton) {
                playClick();

                prevButton = currButton;
            }
        }
        /*if (gameObject.activeSelf && Input.GetButtonDown("Cancel")) {
            playClick();
            Close();
            PlayerControls.Instance.PushPlayer(Vector2.up);
        }*/
    }

    /// <summary>
    /// Open the navigation popup
    /// </summary>
    public void Open() {
        this.gameObject.SetActive(true);
        updateAreaNames();
    }

    /// <summary>
    /// Close the navigation popup
    /// </summary>
    public void Close() {
        GameManager.Instance.OnMap = false;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Select an area to go to
    /// </summary>
    /// <param name="area">Index of the area want to go to</param>
    public void SelectArea(int area) {
        playClick();
        string sceneName = Places[area].GetSceneName();
        GameManager.Instance.PreviousScene = Constants.MAP;
        GameManager.Instance.CurrentScene = sceneName;

        GameManager.Instance.OnMap = false;

        LoadSceneLogic.Instance.LoadScene(sceneName, Vector2.up);
    }

    /// <summary>
    /// Update the buttons with the name of the areas in the navigation popup
    /// </summary>
    private void updateAreaNames() {
        bool setCurrentButton = false;
        for (int i = 0; i < AreaNames.Length; i++) {
            if (i >= Places.Length || !EventManager.Instance.DidEventHappened(Places[i].GetSceneName())) {
                // if there are more buttons than there are traversable scenes, 
                // or if the area hasn't been unlocked
                // hide the rest of the buttons
                AreaNames[i].gameObject.SetActive(false);
                continue;
            }
            SceneName scene = Places[i];
            string area = scene.GetSceneName().Replace('_', ' ');
            AreaNames[i].gameObject.SetActive(true);
            AreaNames[i].text = area;
            if (!setCurrentButton && EventSystem.current != null) {
                EventSystem.current.SetSelectedGameObject(AreaNames[i].gameObject);
                setCurrentButton = true;
            }
        }
    }

    /// <summary>
    /// Play the click sound effect
    /// </summary>
    private void playClick() {
        SoundManager.Instance.PlaySFX(0);
    }
}