using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class for managing the Logic System UI
/// </summary>
public class LogicSystemUI : MonoBehaviour {
    [SerializeField] LogicStep[] logicSteps;
    [SerializeField] string chapterName;
    [SerializeField] SceneName sceneName;
    [SerializeField] SceneName nextScene;
    [SerializeField] int lifeCount = 3;

    [Header("UI for Lives")]
    public Image[] Lives;
    public Sprite LifeIcon;
    public Sprite DamageIcon;
    public EventSystem EventSystem;
    [Header("UI For Dialogue")]
    public Text Name;
    public Text Dialogue;
    public Image Portrait;

    [Header("UI For Clues")]
    public GameObject CluesPopup;
    public Text[] ClueNames;
    public Text Question;
    public Image ClueImage;
    public Text Description;

    [Header("UI For Multiple Choice")]
    public GameObject ChoicePopup;
    public Text[] Choices;

    private List<string> clues;
    private CutsceneSpriteHolder spriteHolder;

    private int currStep = 0;
    private int clueOffset = -1;
    private string currClueName = "";
    private bool showPopup = false;
    private bool wrongAnswer = false;
    private int numWrongGuesses = 0;

    // Start is called before the first frame update
    void Start() {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        clues = GameManager.Instance.GetCurrentClues();

        updateLives();
        updateLogicStepDialogue();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact")) {
            // if press input
            if (!showPopup && !wrongAnswer && currStep < logicSteps.Length - 1) {
                // if can go to next logic step, progress
                currStep++;
                updateLogicStepDialogue();
            } else if (wrongAnswer) {
                showPopup = true;
                wrongAnswer = false;
            } else if (showPopup) {
                // else show the popup after showing current logic step's dialogue
                handlePopup();
            } else if (currStep >= logicSteps.Length - 1) {
                // end of steps
                // TODO: transition to next scene
                Dialogue.text = "End of Logic System";
                //GameManager.Instance.PreviousScene = sceneName.GetSceneName();
                //LoadSceneLogic.Instance.LoadScene(nextScene.GetSceneName());
            }
        }

        handleScroll();
    }

    /// <summary>
    /// Select a clue
    /// </summary>
    /// <param name="clueIndex">Index of the clue</param>
    public void SelectClue(int clueIndex) {
        if (clueIndex < ClueNames.Length) {
            wrongAnswer = false;
            string clueName = ClueNames[clueIndex].text;
            Clue selectedClue = GameManager.Instance.GetClueWithName(chapterName, clueName);
            LogicStep currLogicStep = logicSteps[currStep];
            CluesPopup.SetActive(false);
            showPopup = false;
            if (!currLogicStep.GetClue().IsEqual(selectedClue)) {
                // selected wrong clue
                wrongAnswer = true;
                updateWrongDialogue();
            }
        }
    }

    /// <summary>
    /// Select a choice
    /// </summary>
    /// <param name="choice"> Index of the choice</param>
    public void SelectChoice(int choice) {
        LogicStep currLogicStep = logicSteps[currStep];

        int correctChoice = currLogicStep.GetCorrectChoice();

        ChoicePopup.SetActive(false);
        showPopup = false;
        if (choice != correctChoice) {
            // selected wrong choice
            wrongAnswer = true;
            updateWrongDialogue();
        }
    }

    /// <summary>
    /// Handle scrolling of the popup
    /// </summary>
    private void handleScroll() {
        if (showPopup && Input.GetButtonDown("Vertical") && CluesPopup.activeSelf) {
            // only handle scroll when the clues popup is being shown
            GameObject currButton = EventSystem.current.currentSelectedGameObject;
            string currButtonName = currButton.GetComponent<Text>().text;
            float xInput = Input.GetAxisRaw("Vertical");

            if (currButtonName.Equals(currClueName)) {
                // if curr button name does not match curr clue name, need to scroll
                int prevClueOffset = clueOffset;
                clueOffset += xInput > 0.5f ? -1 : 1;
                clueOffset = Math.Max(0, clueOffset); // cannot go below 0
                clueOffset = Math.Min(clueOffset, clues.Count - ClueNames.Length); // cannot up above number of clues in chapter - 1
                updateCluesPopup();
                int newClueIndx = xInput > 0.5f ? clueOffset : clueOffset + ClueNames.Length - 1;
                currClueName = clues[newClueIndx];
                if (prevClueOffset != clueOffset) updateCluesDescription();
            } else {
                // if prev button isn't current button, update current clue name
                currClueName = currButtonName;
                updateCluesDescription();
            }
        }
    }

    /// <summary>
    /// Update to the next step's dialogue
    /// </summary>
    private void updateLogicStepDialogue() {
        LogicStep currLogicStep = logicSteps[currStep];
        updateDialogue(currLogicStep.GetDialogue());
        // if there is a clue or choice to be made, cannot go to next logic step
        showPopup = currLogicStep.GetClue() != null || currLogicStep.GetCorrectChoice() > -1;
    }

    /// <summary>
    /// Update the dialogue to the wrong answer dialogue
    /// </summary>
    private void updateWrongDialogue() {
        numWrongGuesses++;
        updateLives();
        if (numWrongGuesses >= lifeCount) {
            // gameover
            GameManager.Instance.PreviousScene = sceneName.GetSceneName();
            LoadSceneLogic.Instance.LoadScene(Constants.GAMEOVER);
        }
        LogicStep currLogicStep = logicSteps[currStep];
        updateDialogue(currLogicStep.GetWrongDialogue());
        showPopup = true;
    }

    /// <summary>
    /// Update the dialogue text and protrait
    /// </summary>
    /// <param name="text">Dialogue text</param>
    private void updateDialogue(string text) {
        string[] stepString = Parser.SplitLogicDialogue(text);

        Portrait portrait = Parser.ParsePortrait(stepString[0]); // get the name and sprite name
        string dialogue = stepString[1]; // get dialogue
        Sprite sprite = spriteHolder.GetSprite(portrait.spriteName); // get the sprite

        Name.text = portrait.name;
        Portrait.sprite = sprite;
        Dialogue.text = dialogue;
    }

    /// <summary>
    /// Handle choosing which popup to show and updating the popups with the correct information
    /// </summary>
    private void handlePopup() {
        LogicStep currLogicStep = logicSteps[currStep];

        if (currLogicStep.GetClue() != null) {
            // show clues popup
            clueOffset = 0;
            CluesPopup.SetActive(true);

            string[] stepString = Parser.SplitLogicDialogue(currLogicStep.GetDialogue());
            Question.text = stepString[1];

            updateCluesPopup();
            EventSystem.current.SetSelectedGameObject(ClueNames[0].gameObject);
            currClueName = clues[0];
            updateCluesDescription();
        } else {
            // show choices popup
            updateChoicesPopup();
        }
    }

    /// <summary>
    /// Update the clues popup
    /// </summary>
    private void updateCluesPopup() {
        for (int i = 0; i < ClueNames.Length; i++) {
            ClueNames[i].gameObject.SetActive(true);
            if (i + clueOffset >= clues.Count) {
                // if there are less clues than the amount of clue names
                ClueNames[i].gameObject.SetActive(false);
            } else {
                // update name of the clue
                ClueNames[i].text = clues[i + clueOffset];
            }
        }
    }

    /// <summary>
    /// Update the clue description on Clues popup
    /// </summary>
    private void updateCluesDescription() {
        Clue clue = GameManager.Instance.GetClueWithName(chapterName, currClueName);
        ClueImage.sprite = clue.GetSprite();
        Description.text = clue.GetDescription();
    }

    /// <summary>
    /// Update the choices popup
    /// </summary>
    private void updateChoicesPopup() {
        LogicStep currLogicStep = logicSteps[currStep];

        if (!Dialogue.text.Equals(currLogicStep.GetDialogue())) {
            // if the dialogue isn't the same
            updateLogicStepDialogue();
        }

        string[] choices = currLogicStep.GetChoices();

        ChoicePopup.SetActive(true);
        for (int i = 0; i < Choices.Length; i++) {
            Choices[i].gameObject.SetActive(true);
            if (i >= choices.Length) {
                // if there are less choices than there are buttons for choices
                Choices[i].gameObject.SetActive(false);
            } else {
                Choices[i].text = choices[i];
            }
        }

        EventSystem.current.SetSelectedGameObject(Choices[0].gameObject);
    }

    /// <summary>
    /// Update the number of lives on screen
    /// </summary>
    private void updateLives() {
        int numLives = lifeCount - numWrongGuesses;
        for (int i = 0; i < Lives.Length; i++) {
            Lives[i].gameObject.SetActive(true);
            if (i >= lifeCount) {
                // hide additional lives
                Lives[i].gameObject.SetActive(false);
            } else if (i < numLives) {
                Lives[i].sprite = LifeIcon;
            } else {
                Lives[i].sprite = DamageIcon;
            }
        }
    }
}
