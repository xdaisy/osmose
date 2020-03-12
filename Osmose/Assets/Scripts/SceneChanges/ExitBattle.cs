using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles loading after battle
/// </summary>
public class ExitBattle : MonoBehaviour {
    public string SceneName;
    public float WaitToLoad = 1f;

    private bool shouldLoadAfterFade;
    private string sceneToLoad;

    /// <summary>
    /// Go back to previous scene after battle was won
    /// </summary>
    public void EndBattle() {
        sceneToLoad = GameManager.Instance.CurrentScene;
        PlayerControls.Instance.PreviousAreaName = SceneName;
        StartCoroutine(loadScene());
    }

    /// <summary>
    /// Go back to town after party was defeated
    /// </summary>
    public void DefeatedInBattle() {
        sceneToLoad = GameManager.Instance.LastTown;
        GameManager.Instance.CurrentScene = GameManager.Instance.LastTown;
        GameManager.Instance.Party.RecoverParty();
        PlayerControls.Instance.PreviousAreaName = "Defeated";
        PlayerControls.Instance.SetPlayerForward();
        StartCoroutine(loadScene());
    }

    /// <summary>
    /// Wait and then load scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadScene() {
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        shouldLoadAfterFade = true;
        yield return new WaitForSeconds(WaitToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
