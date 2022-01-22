using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour {
    public SceneName[] Places;
    public GameObject UI;
    public Text[] AreaNames;

    // Update is called once per frame
    void Update() {
        if (gameObject.activeSelf && Input.GetButtonDown("Cancel")) {
            playClick();
            Close();
            PlayerControls.Instance.PushPlayer(Vector2.up);
        }
    }

    public void Open() {
        this.gameObject.SetActive(true);
        updateArenNames();
    }

    public void Close() {
        GameManager.Instance.OnMap = false;
        this.gameObject.SetActive(false);
    }

    public void SelectArea(int area) {
        string sceneName = Places[area].GetSceneName();
        GameManager.Instance.PreviousScene = GameManager.Instance.CurrentScene;
        GameManager.Instance.CurrentScene = sceneName;

        PlayerControls.Instance.SetLastMove(Vector2.up);
        GameManager.Instance.OnMap = false;

        LoadSceneLogic.Instance.LoadScene(sceneName);
    }

    private void updateArenNames() {
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
            if (!setCurrentButton) {
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