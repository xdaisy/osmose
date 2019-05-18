using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBattle : MonoBehaviour {
    public string SceneName;
    public float WaitToLoad = 1f;

    private bool shouldLoadAfterFade;
    private string sceneToLoad;

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // party won so go back to current scene
    public void EndBattle() {
        sceneToLoad = GameManager.Instance.CurrentScene;
        PlayerControls.Instance.PreviousAreaName = SceneName;
        loadScene();
    }

    // party was defeated so go back to town
    public void DefeatedInBattle() {
        sceneToLoad = GameManager.Instance.LastTown;
        GameManager.Instance.CurrentScene = GameManager.Instance.LastTown;
        GameManager.Instance.Party.RecoverParty();
        PlayerControls.Instance.PreviousAreaName = "Defeated";
        PlayerControls.Instance.SetPlayerForward();
        loadScene();
    }

    private void loadScene() {
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        shouldLoadAfterFade = true;
    }
}
