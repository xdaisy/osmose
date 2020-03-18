using UnityEngine;

/// <summary>
/// Class that handles when the player exits an area
/// </summary>
public class AreaExit : MonoBehaviour {
    public SceneName SceneToLoad;

    public string AreaName; // area's name

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerControls.Instance.PreviousAreaName = AreaName;

            if (!SceneToLoad.Equals(Constants.MAP)) {
                // only add scene if not going to map
                EventManager.Instance.AddEvent(SceneToLoad.GetSceneName());
            }

            LoadSceneLogic.Instance.LoadScene(SceneToLoad.GetSceneName());
        }
    }
}
