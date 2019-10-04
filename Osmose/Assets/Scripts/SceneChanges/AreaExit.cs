using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles when the player exits an area
/// </summary>
public class AreaExit : MonoBehaviour {

    public string SceneToLoad;

    public string AreaName; // area's name

    public float WaitToLoad = Constants.WAIT_TIME; 

    private bool shouldLoadAfterFade;

    // Update is called once per frame
    void Update() {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(SceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            shouldLoadAfterFade = true;
            UIFade.Instance.FadeToBlack();
            PlayerControls.Instance.PreviousAreaName = AreaName;
            GameManager.Instance.FadingBetweenAreas = true;
            GameManager.Instance.CurrentScene = SceneToLoad;

            if (!SceneToLoad.Equals(Constants.MAP)) {
                // only add scene if not going to map
                EventManager.Instance.AddEvent(SceneToLoad);
            }
        }
    }
}
