using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles when the player exits an area
/// </summary>
public class AreaExit : MonoBehaviour {
    public SceneName SceneToLoad;

    public string AreaName; // area's name

    public float WaitToLoad = Constants.WAIT_TIME;

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(loadScene());
        }
    }

    /// <summary>
    /// Wait and then load scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadScene() {
        UIFade.Instance.FadeToBlack();
        PlayerControls.Instance.PreviousAreaName = AreaName;
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.CurrentScene = SceneToLoad.GetSceneName();

        if (!SceneToLoad.Equals(Constants.MAP)) {
            // only add scene if not going to map
            EventManager.Instance.AddEvent(SceneToLoad.GetSceneName());
        }
        yield return new WaitForSeconds(WaitToLoad);
        SceneManager.LoadScene(SceneToLoad.GetSceneName());
    }
}
