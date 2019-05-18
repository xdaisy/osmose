using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadGame : MonoBehaviour {
    [Header("General")]
    public CanvasGroup MainGroup;
    public string loadArea;
    public float WaitToLoad = 1f;
    public Button ContinueButton;

    [Header("Continue UI")]
    public GameObject ContinueScreen;
    public SaveMenu SaveMenuUI;

    private bool shouldLoadAfterFade;
    private bool isContinue;
    private int fileToLoad = -1;	
	// Update is called once per frame
	void Update () {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                if (isContinue) {
                    SaveFileManager.Load(fileToLoad);
                } else {
                    SceneManager.LoadScene(loadArea);
                }
            }
        }
        if (Input.GetButtonDown("Cancel") && !MainGroup.interactable) {
            // if exiting continue screen
            ContinueScreen.SetActive(false);
            MainGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(ContinueButton.gameObject);
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

    // go to choose while file to continue
    public void ContinueGame() {
        MainGroup.interactable = false;
        ContinueScreen.SetActive(true);
        SaveMenuUI.OpenSaveMenu();
    }

    public void ContinueFile(int file) {
        if (SaveFileManager.SaveExists(file)) {
            // can continue if save file exists
            shouldLoadAfterFade = true;
            isContinue = true;
            fileToLoad = file;
            UIFade.Instance.FadeToBlack();
            GameManager.Instance.FadingBetweenAreas = true;
        }
    }
}
