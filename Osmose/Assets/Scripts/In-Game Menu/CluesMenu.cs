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
    public CanvasGroup HeaderGroup;
    public CanvasGroup ListGroup;

    [Header("Header UI")]
    public Button CluesButton;
    public Button PastCluesButton;

    [Header("Name List")]
    public Text[] Names;

    [Header("Description")]
    public Text ClueName;
    public Text ClueDescription;
    public Image ClueImage;

    private EventSystem eventSystem;
    private int clueOffset = 0;
    private bool lookCurrentClues = true;
    private Clue currClue = null;

    private Button currButton = null;

    // Start is called before the first frame update
    void Start() {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update() {
        if (HeaderGroup.interactable && Input.GetButtonDown("Horizontal")) {
            // go about selecting which type of items
            Button currentButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();
            if (currentButton != currButton) {
                // different button
                lookCurrentClues = currentButton == CluesButton;
                currClue = lookCurrentClues ? GameManager.Instance.getCurrentClueAt(0) : GameManager.Instance.getPastClueAt(0);
                updateNamesList();
                updateDescription();
                currButton = currentButton;
            }
        }
    }

    /// <summary>
    /// Open the clues menu
    /// </summary>
    public void OpenCluesMenu() {
        if (eventSystem == null) {
            eventSystem = EventSystem.current;
        }
        HeaderGroup.interactable = true;
        eventSystem.SetSelectedGameObject(CluesButton.gameObject);
        clueOffset = 0;
        lookCurrentClues = true;
        currButton = CluesButton;
        currClue = GameManager.Instance.getCurrentClueAt(clueOffset);
        updateNamesList();
        updateDescription();
    }

    /// <summary>
    /// Go back to Header group if on list, otherwise, set up closing this menu
    /// </summary>
    /// <returns>True if the menu is closed, false otherwise</returns>
    public bool GoBack() {
        if (ListGroup.interactable) {
            // go back to selecting current vs past clues
            ListGroup.interactable = false;
            HeaderGroup.interactable = true;
            if (lookCurrentClues) {
                eventSystem.SetSelectedGameObject(CluesButton.gameObject);
            } else {
                eventSystem.SetSelectedGameObject(PastCluesButton.gameObject);
            }
            return false;
        }
        // go back to main menu
        ListGroup.interactable = false;
        HeaderGroup.interactable = false;
        eventSystem.SetSelectedGameObject(null);
        return true;
    }

    /// <summary>
    /// Go to looking at clue list
    /// </summary>
    /// <param name="lookCurrentClues">True if looking at current clues, false if looking at past clues</param>
    public void ClickHeaderOption(bool lookCurrentClues) {
        this.lookCurrentClues = lookCurrentClues;
        HeaderGroup.interactable = false;
        ListGroup.interactable = true;
        currClue = lookCurrentClues ? GameManager.Instance.getCurrentClueAt(0) : GameManager.Instance.getPastClueAt(0);
        eventSystem.SetSelectedGameObject(Names[0].gameObject);
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
                currentClue = GameManager.Instance.getCurrentClueAt(i + clueOffset);
            } else {
                // looking at past clues
                currentClue = GameManager.Instance.getPastClueAt(i + clueOffset);
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
}
