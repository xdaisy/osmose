using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles the Menu
/// </summary>
public class Menu : MonoBehaviour {
    public static Menu Instance;

    [Header("UI")]
    public GameObject[] Menus;
    public GameObject CluesButton;
    public GameObject SaveButton;
    public GameObject[] Party;
    public Text[] PartyNames;
    public Image[] PartyImages;

    [Header("Other Menus")]
    public CluesMenu CluesMenuUI;
    public SaveMenu SaveMenuUI;

    private EventSystem eventSystem;
    private int charPos = -1;
    private int currMenu = 0;

    private const int MAIN_MENU = 0;
    private const int CLUES_MENU = 1;
    private const int SAVE_MENU = 2;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        eventSystem = this.GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(CluesButton);
        updateCharacterOrder();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            // go back
            goBack();
        }
    }

    /// <summary>
    /// Open the menu
    /// </summary>
    public void OpenGameMenu() {
        GameManager.Instance.GameMenuOpen = true;
        currMenu = MAIN_MENU;
        Menus[MAIN_MENU].SetActive(true);
        eventSystem.SetSelectedGameObject(CluesButton);
    }

    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseGameMenu() {
        closeAllMenu();
        eventSystem.SetSelectedGameObject(null);
        currMenu = MAIN_MENU;
        GameManager.Instance.GameMenuOpen = false;
    }

    /// <summary>
    /// Open a menu
    /// </summary>
    /// <param name="index">Position of the menu that is being opened</param>
    public void OpenMenu(int index) {
        closeAllMenu();
        eventSystem.SetSelectedGameObject(null);
        Menus[index].SetActive(true);
        currMenu = index;

        if (index == CLUES_MENU) {
            // opening clues menu
            CluesMenuUI.OpenCluesMenu();
        }

        if (index == SAVE_MENU) {
            SaveMenuUI.OpenSaveMenu();
        }
    }

    /// <summary>
    /// Swap characters around in the party
    /// </summary>
    /// <param name="index">Position of the character being swapped</param>
    public void SwapCharacters(int index) {
        if (charPos < 0) {
            charPos = index;
        } else {
            List<string> party = GameManager.Instance.GetCurrentParty();
            string temp = party[charPos];
            party[charPos] = party[index];
            party[index] = temp;
            GameManager.Instance.ChangeMembers(party);
            charPos = -1;
            updateCharacterOrder();
        }
    }

    /// <summary>
    /// Save the game
    /// </summary>
    /// <param name="file">Index of the file</param>
    public void Save(int file) {
        StartCoroutine(SaveCo(file));
    }

    /// <summary>
    /// Coroutine to save the game
    /// </summary>
    /// <param name="file">Index of the file</param>
    private IEnumerator SaveCo(int file) {
        // save
        SaveMenuUI.PlaySaveAnimation();
        SaveFileManager.Save(file);
        // wait 1 sec
        yield return new WaitForSeconds(1f);
        SaveMenuUI.UpdateSaveMenu();
        SaveMenuUI.StopSaveAnimation();
    }

    /// <summary>
    /// Update the character order
    /// </summary>
    private void updateCharacterOrder() {
        List<string> party = GameManager.Instance.GetCurrentParty();

        for (int i = 0; i < Party.Length; i++) {
            if (i < party.Count) {
                Party[i].gameObject.SetActive(true);
                PartyNames[i].text = party[i];
                PartyImages[i].sprite = GameManager.Instance.GetCharSprite(party[i]);
            } else {
                Party[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Go back to previous area
    /// </summary>
    private void goBack() {
        if (currMenu == CLUES_MENU) {
            if (CluesMenuUI.GoBack()) {
                // if going back from clues menu to main menu
                closeAllMenu();
                currMenu = MAIN_MENU;
                Menus[MAIN_MENU].SetActive(true);
                eventSystem.SetSelectedGameObject(CluesButton);
            }
        }
        if (currMenu == SAVE_MENU) {
            // if going back from save menu to main menu
            closeAllMenu();
            currMenu = MAIN_MENU;
            Menus[MAIN_MENU].SetActive(true);
            eventSystem.SetSelectedGameObject(SaveButton);
        }
    }

    /// <summary>
    /// Set all menus to inactive
    /// </summary>
    private void closeAllMenu() {
        foreach(GameObject menu in Menus) {
            menu.SetActive(false);
        }
    }
}
