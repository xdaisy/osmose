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

    /*[Header("Header UI")]
    public Dropdown CluesDropDown;
    public Button DropDownButton;
    public Text SelectedChapterName;
    public GameObject DropDownMenu;
    public Text[] ChapterNames;
    public GameObject UpArrow;
    public GameObject DownArrow;*/

    [Header("Name List")]
    public Text[] Names;
    public GameObject NamesUpArrow;
    public GameObject NamesDownArrow;

    [Header("Description")]
    public Text ClueName;
    public Text ClueDescription;
    public Image ClueImage;

    private EventSystem eventSystem;

    /*private List<string> prevChapters;
    private string chapter = "";
    private int chapterOffset = 0;*/

    private int clueOffset = 0;
    private bool lookCurrentClues = true;
    private Clue currClue = null;

    private Button currButton = null;

    private const string CURRENT_CHAPTER = "------";

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;

        lookCurrentClues = true;
        //prevChapters = GameManager.Instance.GetPastChapters();
        //prevChapters.Insert(0, "------"); // add current chapter
    }

    // Update is called once per frame
    void Update() {
        if (ListGroup.interactable && Input.GetButtonDown("Vertical")) {
            // scrolling through clues
            scrollThroughCluesList();
        }
        /*if (DropDownMenu.activeSelf && Input.GetButtonDown("Vertical")) {
            // scrolling through chapters
            scrollThroughChapterList();
        }*/
    }

    /// <summary>
    /// Open the clues menu
    /// </summary>
    public void OpenCluesMenu() {
        if (eventSystem == null) {
            eventSystem = EventSystem.current;
        }
        ListGroup.interactable = true;
        //DropDownButton.interactable = true;
        clueOffset = 0;
        lookCurrentClues = true;
        currButton = Names[0].GetComponent<Button>();
        currClue = GameManager.Instance.GetCurrentClueAt(clueOffset);
        //prevChapters = GameManager.Instance.GetPastChapters();
        //chapterOffset = 0;
        eventSystem.SetSelectedGameObject(currButton.gameObject);
        updateNamesList();
        updateDescription();
    }

    /// <summary>
    /// Go back to Header group if on list, otherwise, set up closing this menu
    /// </summary>
    /// <returns>True if the menu is closed, false otherwise</returns>
    public bool GoBack() {
        playClick();
        /*if (DropDownMenu.activeSelf) {
            // if drop down menu is active
            DropDownButton.interactable = true;
            DropDownMenu.SetActive(false);
            ListGroup.interactable = true;
            clueOffset = 0;
            chapterOffset = 0;
            currButton = DropDownButton;
            eventSystem.SetSelectedGameObject(DropDownButton.gameObject);
            return false;
        }
        if (ListGroup.interactable) {
            // if scrolling through clues list
            ListGroup.interactable = false;
            DropDownButton.interactable = true;
            currButton = DropDownButton;
            eventSystem.SetSelectedGameObject(DropDownButton.gameObject);
            return false;
        }*/
        // go back to main menu
        ListGroup.interactable = false;
        eventSystem.SetSelectedGameObject(null);
        return true;
    }

    /// <summary>
    /// Open the Chapter drop down menu
    /// </summary>
    public void OpenChapterDropdown() {
        playClick();
        ListGroup.interactable = false;
        /*DropDownMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(ChapterNames[0].gameObject);
        DropDownButton.interactable = false;
        currButton = ChapterNames[0].GetComponent<Button>();*/
        updateChapterNames();
    }

    /// <summary>
    /// Select the chapter of clues player wants to see
    /// </summary>
    /// <param name="index">Index of the button</param>
    public void ChangeChapterDropdown(int index) {
        /*playClick();
        string currChapter = ChapterNames[index].text;
        lookCurrentClues = false;
        if (currChapter.IndexOf(CURRENT_CHAPTER) > -1) {
            // is past chapter
            lookCurrentClues = true;
        }
        //DropDownMenu.SetActive(false);
        ListGroup.interactable = true;
        //chapter = currChapter;
        clueOffset = 0;
        chapterOffset = 0;
        currClue = lookCurrentClues ? GameManager.Instance.GetCurrentClueAt(clueOffset) : GameManager.Instance.GetPastClueAt(chapter, clueOffset);
        currButton = Names[0].GetComponent<Button>();
        eventSystem.SetSelectedGameObject(Names[0].gameObject);
        updateNamesList();
        updateDescription();*/
    }

    /// <summary>
    /// Update the clue names list
    /// </summary>
    private void updateNamesList () {
        // up arrow
        if (clueOffset > 0) {
            NamesUpArrow.SetActive(true);
        }
        else {
            NamesUpArrow.SetActive(false);
        }

        // down arrow
        if (clueOffset + Names.Length < GameManager.Instance.GetNumCurrentClues()) {
            NamesDownArrow.SetActive(true);
        } else {
            NamesDownArrow.SetActive(false);
        }
        for (int i = 0; i < Names.Length; i++) {
            Clue currentClue = null;
            if (lookCurrentClues) {
                // looking at current clues
                currentClue = GameManager.Instance.GetCurrentClueAt(i + clueOffset);
            } else {
                // looking at past clues
                //currentClue = GameManager.Instance.GetPastClueAt(chapter, i + clueOffset);
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
        if (currentButton != null /*&& currentButton != DropDownButton*/) {
            if (currentButton != currButton) {
                // set what is the current clue
                playClick();
                string chapterOfClue = GameManager.Instance.GetCurrentChapter();
                /*string chapterOfClue = chapter.Equals(CURRENT_CHAPTER) ? GameManager.Instance.GetCurrentChapter() : chapter;*/
                currClue = GameManager.Instance.GetClueWithName(chapterOfClue, currentButton.GetComponent<Text>().text);
                currButton = currentButton;
                updateDescription();
            } else {
                // hit end of list (either top or bottom)
                if (input < -0.5f) {
                    // going down
                    Clue c = GameManager.Instance.GetCurrentClueAt(clueOffset + Names.Length);
                    /*Clue c = lookCurrentClues ? GameManager.Instance.GetCurrentClueAt(clueOffset + Names.Length) : GameManager.Instance.GetPastClueAt(chapter, clueOffset + Names.Length);*/
                    if (c != null) {
                        playClick();
                        clueOffset++;
                        currClue = c;
                        updateNamesList();
                        updateDescription();
                    }
                }
                if (input > 0.5f && clueOffset > 0) {
                    // going up
                    playClick();
                    clueOffset--;
                    updateNamesList();
                    updateDescription();
                    string chapterOfClue = GameManager.Instance.GetCurrentChapter();
                    /*string chapterOfClue = chapter.Equals(CURRENT_CHAPTER) ? GameManager.Instance.GetCurrentChapter() : chapter;*/
                    currClue = GameManager.Instance.GetClueWithName(chapterOfClue, currButton.GetComponent<Text>().text);
                }
            }
        }
    }

    /// <summary>
    /// Update the chapter names
    /// </summary>
    private void updateChapterNames() {
        // update the chapter names
        /*for (int i = 0; i < ChapterNames.Length; i++) {
            if (i + chapterOffset < prevChapters.Count) {
                // is has chapter for index
                ChapterNames[i].gameObject.SetActive(true);
                ChapterNames[i].text = prevChapters[i + chapterOffset];
            } else {
                // no chapter for index
                ChapterNames[i].gameObject.SetActive(false);
            }
        }

        // show up arrow if there are chapters above, do not show otherwise
        if (chapterOffset > 0) {
            UpArrow.SetActive(true);
        } else {
            UpArrow.SetActive(false);
        }

        // show up arrow if there are chapters below, do not show otherwise
        if (chapterOffset  + ChapterNames.Length < prevChapters.Count) {
            DownArrow.SetActive(true);
        } else {
            DownArrow.SetActive(false);
        }*/
    }

    /// <summary>
    /// Set current clue and update Clues List and Description if needed
    /// </summary>
    private void scrollThroughChapterList() {
        /*float input = Input.GetAxisRaw("Vertical");
        Button currentButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();
        if (currentButton != null) {
            if (currentButton != currButton) {
                // set current button
                playClick();
                currButton = currentButton;
            } else {
                // hit end of list (either top or bottom)
                if (input < -0.5f && chapterOffset + ChapterNames.Length < prevChapters.Count) {
                    // going down
                    playClick();
                    chapterOffset++;
                    updateChapterNames();
                }
                if (input > 0.5f && chapterOffset > 0) {
                    // going up
                    playClick();
                    chapterOffset--;
                    updateChapterNames();
                }
            }
        }*/
    }

    /// <summary>
    /// Play the click sound effect
    /// </summary>
    private void playClick() {
        SoundManager.Instance.PlaySFX(0);
    }
}
