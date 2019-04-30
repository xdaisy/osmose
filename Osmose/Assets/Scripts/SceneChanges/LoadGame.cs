using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadGame : MonoBehaviour {

    public string loadArea;

    public float WaitToLoad = 1f;

    private bool shouldLoadAfterFade;
    private bool isContinue;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                if (isContinue) {
                    SaveFileManager.Load();
                } else {
                    SceneManager.LoadScene(loadArea);
                }
            }
        }
    }

    // start a new game
    public void StartNewGame() {
        shouldLoadAfterFade = true;
        isContinue = false;
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.CurrentScene = loadArea;
    }

    // continue a gameplay
    public void ContinueGame() {
        if (File.Exists("Assets/Resources/save.txt")) {
            // can continue if save file exists
            shouldLoadAfterFade = true;
            isContinue = true;
            UIFade.Instance.FadeToBlack();
            GameManager.Instance.FadingBetweenAreas = true;
        }
    }
}
