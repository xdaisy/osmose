using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour {
    public static Dialogue Instance; // dialogue manager instance

    [Header("Dialogue box")]
    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text
    public Text dName; // name of dialogue 

    [Header("Clue image assets")]
    public GameObject BlackBG;
    public Image ClueImage;

    [Header("Confirmation popup")]
    public GameObject ConfirmationPopup;
    public GameObject YesButton;

    private string[] dialogueLines; // lines of dialogue
    private int currentLine; // current line of dialogue

    private bool justStarted; // keep track if the dialogue just got started

    private EventSystem eventSystem;

    private bool needInput;
    private string[] yesDialogue;
    private string[] noDialogue;
    private string[] cannotDialogue;
    private bool canDoPuzzle;
    private string sceneName;
    private bool doChangeScene;

    private int sfx = -1;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update () {
        if (!ConfirmationPopup.activeSelf && dBox.activeSelf && Input.GetButtonUp("Interact")) {
            if (!justStarted) {
                currentLine++;
                if (currentLine >= dialogueLines.Length && doChangeScene) {
                    // change the scene
                    dBox.SetActive(false);
                    BlackBG.SetActive(false);
                    currentLine = 0;
                    GameManager.Instance.DialogActive = false;

                    GameManager.Instance.PreviousScene = GameManager.Instance.CurrentScene;
                    LoadSceneLogic.Instance.LoadScene(sceneName);
                }
                if (currentLine >= dialogueLines.Length && !needInput) {
                    dBox.SetActive(false);
                    BlackBG.SetActive(false);

                    currentLine = 0;

                    playSfx(sfx);
                    sfx = -1;
                    GameManager.Instance.DialogActive = false;
                }
                if (currentLine >= dialogueLines.Length && needInput && !ConfirmationPopup.activeSelf) {
                    // if needs to show popup
                    ConfirmationPopup.SetActive(true);
                    eventSystem = EventSystem.current;
                    eventSystem.SetSelectedGameObject(null);
                    eventSystem.SetSelectedGameObject(YesButton);
                    return;
                }
                showText();
            } else {
                justStarted = false;
            }
        }
	}

    /// <summary>
    /// Start the dialogue
    /// </summary>
    /// <param name="lines">Lines of dialogue</param>
    /// <param name="triggered">Flag if the dialogue was triggered because of a cutscene</param>
    /// <param name="sfx">Sfx played after the dialouge</param>
    public void ShowDialogue(string[] lines, bool triggered, int sfx = -1) {
        dBox.SetActive(true);
        dialogueLines = lines;
        currentLine = 0;
        needInput = false;
        showText();
        GameManager.Instance.DialogActive = true;
        // if triggered bc cutscene, want to set to false
        // if triggered by interacting, want to set to true
        justStarted = !triggered;
        this.sfx = sfx;
    }

    /// <summary>
    /// Show the clue image on canvas
    /// </summary>
    /// <param name="clueImage">Image of the clue</param>
    public void ShowClueImage(Sprite clueImage) {
        BlackBG.SetActive(true);
        ClueImage.sprite = clueImage;
    }

    /// <summary>
    /// Return whether or not the dialogue is showing
    /// </summary>
    /// <returns>True if the dialogue is show, false otherwise</returns>
    public bool GetDialogueActive() {
        return dBox.activeSelf;
    }

    /// <summary>
    /// Show the dialogue for going to the logic puzzle
    /// </summary>
    /// <param name="iniial">Dialogue for when first talking</param>
    /// <param name="yes">Dialogue for choosing yes</param>
    /// <param name="no">Dialogue for choosing no</param>
    /// <param name="cannot">Dialogue for when player cannot go do the logic puzzle</param>
    /// <param name="canDoPuzzle">True if player can go do logic puzzle, false otherwise</param>
    /// <param name="sceneName">Name of the scene to go to for the logic puzzle</param>
    public void ActivatePuzzleDialogue(string[] iniial, string[] yes, string[] no, string[] cannot, bool canDoPuzzle, string sceneName) {
        this.yesDialogue = yes;
        this.noDialogue = no;
        this.cannotDialogue = cannot;
        this.canDoPuzzle = canDoPuzzle;
        this.sceneName = sceneName;
        needInput = true;
        doChangeScene = false;

        dBox.SetActive(true);
        dialogueLines = iniial;
        currentLine = 0;
        showText();
        GameManager.Instance.DialogActive = true;
        justStarted = true;
    }

    /// <summary>
    /// Function for click on the confirmation popup buttons
    /// </summary>
    /// <param name="yes"></param>
    public void ClickConfirmButtons(bool yes) {
        if (yes) {
            dialogueLines = canDoPuzzle ? this.yesDialogue : this.cannotDialogue;
            currentLine = 0;
            showText();
            needInput = false;
            doChangeScene = canDoPuzzle;
        } else {
            // click no
            dialogueLines = this.noDialogue;
            currentLine = 0;
            showText();
            needInput = false;
        }
        justStarted = true;
        ConfirmationPopup.SetActive(false);
    }

    /// <summary>
    /// Show the next line of text
    /// </summary>
    private void showText() {
        string line = dialogueLines[currentLine];
        if (line.Contains("-")) {
            // if someone is talking, will have <name>-<text>
            string[] lineSplit = line.Split('-');
            dName.text = lineSplit[0];
            line = lineSplit[1];
        } else {
            // else, no one is talking. have text showing
            dName.text = "";
        }
        dText.text = line;
    }

    private void playSfx(int sfx) {
        SoundManager.Instance.PlaySFX(sfx);
    }
}
