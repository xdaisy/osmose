using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles loading after battle
/// </summary>
public class ExitBattle : MonoBehaviour {
    public string SceneName;

    /// <summary>
    /// Go back to previous scene after battle was won
    /// </summary>
    public void EndBattle() {
        string sceneToLoad = GameManager.Instance.PreviousScene;
        GameManager.Instance.PreviousScene = SceneName;
        LoadSceneLogic.Instance.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Go back to town after party was defeated
    /// </summary>
    public void DefeatedInBattle() {
        string sceneToLoad = GameManager.Instance.LastTown;
        GameManager.Instance.CurrentScene = GameManager.Instance.LastTown;
        GameManager.Instance.Party.RecoverParty();
        GameManager.Instance.PreviousScene = "Defeated";
        PlayerControls.Instance.SetPlayerForward();
        LoadSceneLogic.Instance.LoadScene(sceneToLoad);
    }
}
