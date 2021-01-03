using System.Collections;
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

    private int currClue = -1;

    // Start is called before the first frame update
    void Start() {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        clues = GameManager.Instance.GetChapterClues(chapterName);
    }

    // Update is called once per frame
    void Update() {
    }

    public void HandleScroll() {

    }

    public void SelectClue(int clueIndex) {

    }

    public void SelectChoice(int choice) {

    }

    private void updateDialogue() {

    }

    private void updateCluePopup() {

    }
}
