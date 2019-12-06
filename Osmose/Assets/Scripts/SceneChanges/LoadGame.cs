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
    private bool onContinueScreen;

    void Start() {
        GameManager.Instance.OnMainMenu = true;
    }

    // Update is called once per frame
    void Update () {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                GameManager.Instance.OnMainMenu = false;
                if (isContinue) {
                    SaveFileManager.Load(fileToLoad);
                } else {
                    SceneManager.LoadScene(loadArea);
                }
            }
        }
        if (Input.GetButtonDown("Cancel") && onContinueScreen) {
            // if exiting continue screen
            ContinueScreen.SetActive(false);
            onContinueScreen = false;
            MainGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(ContinueButton.gameObject);
        }
    }

    /// <summary>
    /// Start a new game
    /// </summary>
    public void StartNewGame() {
        shouldLoadAfterFade = true;
        isContinue = false;
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        GameManager.Instance.CurrentScene = loadArea;
    }

    /// <summary>
    /// Open continue menu to choose a file to continue
    /// </summary>
    public void ContinueGame() {
        MainGroup.interactable = false;
        ContinueScreen.SetActive(true);
        onContinueScreen = true;
        SaveFileManager.LoadSaves();
        SaveMenuUI.OpenSaveMenu();
    }

    /// <summary>
    /// Continue the file
    /// </summary>
    /// <param name="file">Index of the file want to continue</param>
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
