using System.Collections;
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
    public Button[] Choices;

    private List<string> clues;
    private CutsceneSpriteHolder spriteHolder;

    private int currStep = 0;
    private int clueOffset = -1;
    private string currClueName = "";
    private bool showPopup = false;

    // Start is called before the first frame update
    void Start() {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        clues = GameManager.Instance.GetCurrentClues();

        updateDialogue();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact")) {
            // if press input
            if (!showPopup && currStep < logicSteps.Length - 1) {
                // if can go to next logic step, progress
                currStep++;
                updateDialogue();
            } else if (showPopup) {
                // else show the popup after showing current logic step's dialogue
                handlePopup();
            }
        }

        handleScroll();
    }

    public void SelectClue(int clueIndex) {

    }

    public void SelectChoice(int choice) {

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
    /// Update the dialogue and the character portrait being shown on screen
    /// </summary>
    private void updateDialogue() {
        LogicStep currLogicStep = logicSteps[currStep];

        string[] stepString = Parser.SplitLogicDialogue(currLogicStep.GetDialogue());

        Portrait portrait = Parser.ParsePortrait(stepString[0]); // get the name and sprite name
        string dialogue = stepString[1]; // get dialogue
        Sprite sprite = spriteHolder.GetSprite(portrait.spriteName); // get the sprite

        Name.text = portrait.name;
        Portrait.sprite = sprite;
        Dialogue.text = dialogue;

        // if there is a clue or choice to be made, cannot go to next logic step
        showPopup = currLogicStep.GetClue() != null || currLogicStep.GetCorrectChoice() > -1;
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
            if (i + clueOffset >= clues.Count) {
                // if there are less clues than the amount of clue names
                ClueNames[i].gameObject.SetActive(false);
            } else {
                // update name of the clue
                ClueNames[i].text = clues[i + clueOffset];
            }
        }
    }

    private void updateCluesDescription() {
        Clue clue = GameManager.Instance.GetClueWithName(chapterName, currClueName);
        ClueImage.sprite = clue.GetSprite();
        Description.text = clue.GetDescription();
    }

    /// <summary>
    /// Update the choices popup
    /// </summary>
    private void updateChoicesPopup() {

    }
}
