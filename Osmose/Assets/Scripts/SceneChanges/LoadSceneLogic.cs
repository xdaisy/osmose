using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneLogic : MonoBehaviour {
    public static LoadSceneLogic Instance;

    // Use this for initialization
    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    public void LoadScene(string sceneName) {
        StartCoroutine(loadSceneCo(sceneName));
    }

    /// <summary>
    /// Wait and then load scene
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <returns></returns>
    private IEnumerator loadSceneCo(string sceneName) {
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.CurrentScene = sceneName;
        yield return new WaitForSeconds(Constants.WAIT_TIME);
        SceneManager.LoadScene(sceneName);
    }
}
