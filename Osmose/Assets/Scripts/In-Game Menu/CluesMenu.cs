using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles UI behavior of the Clues Menu
/// </summary>
public class CluesMenu : MonoBehaviour {
    [Header("Canvas Groups")]
    public CanvasGroup ListGroup;

    [Header("Header UI")]
    public Dropdown CluesDropDown;

    [Header("Name List")]
    public Text[] Names;

    [Header("Description")]
    public Text ClueName;
    public Text ClueDescription;
    public Image ClueImage;

    private EventSystem eventSystem;
    private string chapter = "";
    private int clueOffset = 0;
    private bool lookCurrentClues = true;
    private Clue currClue = null;

    private Button currButton = null;

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;

        List<string> prevChapters = GameManager.Instance.GetPastChapters();
        CluesDropDown.AddOptions(prevChapters);
    }

    // Update is called once per frame
    void Update() {
        if (ListGroup.interactable && Input.GetButtonDown("Vertical")) {
            // scrolling through clues
            scrollThroughCluesList();
        }
    }

    /// <summary>
    /// Open the clues menu
    /// </summary>
    public void OpenCluesMenu() {
        if (eventSystem == null) {
            eventSystem = EventSystem.current;
        }
        CluesDropDown.interactable = true;
        ListGroup.interactable = true;
        eventSystem.SetSelectedGameObject(CluesDropDown.gameObject);
        clueOffset = 0;
        lookCurrentClues = true;
        currButton = Names[0].GetComponent<Button>();
        currClue = GameManager.Instance.GetCurrentClueAt(clueOffset);
        updateNamesList();
        updateDescription();
    }

    /// <summary>
    /// Go back to Header group if on list, otherwise, set up closing this menu
    /// </summary>
    /// <returns>True if the menu is closed, false otherwise</returns>
    public bool GoBack() {
        // go back to main menu
        ListGroup.interactable = false;
        CluesDropDown.interactable = false;
        eventSystem.SetSelectedGameObject(null);
        return true;
    }

    /// <summary>
    /// Select the chapter of clues player wants to see
    /// </summary>
    /// <param name="index"></param>
    public void ChangeChapterDropdown() {
        int val = CluesDropDown.value;
        string currChapter = "";
        lookCurrentClues = true;
        if (val > 0) {
            // is past chapter
            currChapter = CluesDropDown.options[val].text;
            lookCurrentClues = false;
        }
        chapter = currChapter;
        clueOffset = 0;
        currClue = lookCurrentClues ? GameManager.Instance.GetCurrentClueAt(clueOffset) : GameManager.Instance.GetPastClueAt(chapter, clueOffset);
        currButton = Names[clueOffset].GetComponent<Button>();
        eventSystem.SetSelectedGameObject(Names[clueOffset].gameObject);
        updateNamesList();
        updateDescription();
    }

    /// <summary>
    /// Update the clue names list
    /// </summary>
    private void updateNamesList () {
        for (int i = 0; i < Names.Length; i++) {
            Clue currentClue = null;
            if (lookCurrentClues) {
                // looking at current clues
                currentClue = GameManager.Instance.GetCurrentClueAt(i + clueOffset);
            } else {
                // looking at past clues
                currentClue = GameManager.Instance.GetPastClueAt(chapter, i + clueOffset);
            }
            if (currentClue == null) {
                // no clue at index
                Names[i].gameObject.SetActive(false);
                continue;
            }

            Names[i].gameObject.SetActive(true);
            Names[i].text = currentClue.GetName();
        }
    }
    
    /// <summary>
    /// Update the description of the current clue
    /// </summary>
    private void updateDescription() {
        if (currClue != null) {
            ClueImage.gameObject.SetActive(true);
            ClueName.text = currClue.GetName();
            ClueDescription.text = currClue.GetDescription();
            ClueImage.sprite = currClue.GetSprite();
        } else {
            ClueName.text = "";
            ClueDescription.text = "";
            ClueImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set current clue and update Clues List and Description if needed
    /// </summary>
    private void scrollThroughCluesList() {
        float input = Input.GetAxisRaw("Vertical");
        Button currentButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();
        if (currentButton != null) {
            if (currentButton != currButton) {
                // set what is the current clue
                currClue = GameManager.Instance.GetClueWithName(currentButton.GetComponent<Text>().text);
                currButton = currentButton;
                updateDescription();
            } else {
                // hit end of list (either top or bottom)
                if (input < -0.5f) {
                    // going down
                    Clue c = lookCurrentClues ? GameManager.Instance.GetCurrentClueAt(clueOffset + Names.Length) : GameManager.Instance.GetPastClueAt(chapter, clueOffset + Names.Length);
                    if (c != null) {
                        clueOffset++;
                        currClue = c;
                        updateNamesList();
                        updateDescription();
                    }
                }
                if (input > 0.5f && clueOffset > 0) {
                    // going up
                    clueOffset--;
                    updateNamesList();
                    updateDescription();
                    currClue = GameManager.Instance.GetClueWithName(currButton.GetComponent<Text>().text);
                }
            }
        }
    }
}
