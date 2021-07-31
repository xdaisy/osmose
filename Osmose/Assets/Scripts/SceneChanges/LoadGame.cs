using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadGame : MonoBehaviour {
    [Header("General")]
    public CanvasGroup MainGroup;
    public SceneName loadArea;
    public GameObject NewGameButton;
    public Button ContinueButton;

    [Header("Continue UI")]
    public GameObject ContinueScreen;
    public SaveMenu SaveMenuUI;
    
    private bool isContinue;
    private int fileToLoad = -1;
    private bool onContinueScreen;

    // fields for playing sound effects
    private GameObject prevButton;

    void Start() {
        GameManager.Instance.OnMainMenu = true;
        prevButton = NewGameButton;
        EventSystem.current.SetSelectedGameObject(NewGameButton);

        // load the save data
        SaveFileManager.LoadSaves();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Vertical")) {
            GameObject currButton = EventSystem.current.currentSelectedGameObject;

            if (currButton != prevButton) {
                playClick();

                prevButton = currButton;
            }
        }
        if (Input.GetButtonDown("Cancel") && onContinueScreen) {
            // if exiting continue screen
            playClick();
            ContinueScreen.SetActive(false);
            onContinueScreen = false;
            MainGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(ContinueButton.gameObject);
            prevButton = ContinueButton.gameObject;
        }
    }

    /// <summary>
    /// Start a new game
    /// </summary>
    public void StartNewGame() {
        playClick();
        isContinue = false;
        GameManager.Instance.OnMainMenu = false;

        LoadSceneLogic.Instance.LoadScene(loadArea.GetSceneName());
    }

    /// <summary>
    /// Open continue menu to choose a file to continue
    /// </summary>
    public void ContinueGame() {
        playClick();
        MainGroup.interactable = false;
        ContinueScreen.SetActive(true);
        onContinueScreen = true;
        SaveMenuUI.OpenSaveMenu();
        prevButton = EventSystem.current.currentSelectedGameObject;
    }

    /// <summary>
    /// Continue the file
    /// </summary>
    /// <param name="file">Index of the file want to continue</param>
    public void ContinueFile(int file) {
        if (SaveFileManager.SaveExists(file)) {
            // can continue if save file exists
            playClick();
            isContinue = true;
            fileToLoad = file;
            GameManager.Instance.OnMainMenu = false;
            SaveFileManager.Load(fileToLoad);
        } else {
            playNotAllowed();
        }
    }

    /// <summary>
    /// Play the click sound effect
    /// </summary>
    private void playClick() {
        SoundManager.Instance.PlaySFX(0);
    }

    /// <summary>
    /// Play the not allowed sound effect
    /// </summary>
    private void playNotAllowed() {
        SoundManager.Instance.PlaySFX(0);
    }
}
