using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for managing the Logic System UI
/// </summary>
public class LogicSystemUI : MonoBehaviour {
    [SerializeField] LogicStep[] logicSteps;
    [SerializeField] string chapterName;
    [Header("UI For Dialogue")]
    public Text Name;
    public Text Dialogue;
    public Image Portrait;

    [Header("UI For Clues")]
    public GameObject CluesPopup;
    public Text[] ClueNames;
    public Image ClueImage;
    public Text Description;

    [Header("UI For Multiple Choice")]
    public GameObject ChoicePopup;
    public Button[] Choices;

    private List<string> clues;
    private CutsceneSpriteHolder spriteHolder;

    private int currStep = 0;
    private int currClue = -1;

    // Start is called before the first frame update
    void Start() {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        clues = GameManager.Instance.GetChapterClues(chapterName);

        updateDialogue();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && currStep < logicSteps.Length - 1) {
            currStep++;
            updateDialogue();
        }
    }

    public void HandleScroll() {

    }

    public void SelectClue(int clueIndex) {

    }

    public void SelectChoice(int choice) {

    }

    private void updateDialogue() {
        LogicStep currLogicStep = logicSteps[currStep];

        string[] stepString = Parser.SplitLogicDialogue(currLogicStep.GetDialogue());

        Portrait portrait = Parser.ParsePortrait(stepString[0]); // get the name and sprite name
        string dialogue = stepString[1]; // get dialogue
        Sprite sprite = spriteHolder.GetSprite(portrait.spriteName); // get the sprite

        Name.text = portrait.name;
        Portrait.sprite = sprite;
        Dialogue.text = dialogue;
    }

    private void updateCluePopup() {

    }
}
