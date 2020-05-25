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

    public GameObject[] Menus;
    public GameObject CluesButton;
    public GameObject[] Party;
    public Text[] PartyNames;
    public Image[] PartyImages;

    private EventSystem eventSystem;
    private int charPos = -1;

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
    }

    /// <summary>
    /// Open the menu
    /// </summary>
    public void OpenGameMenu() {
        GameManager.Instance.GameMenuOpen = true;
        Menus[0].SetActive(true);
        eventSystem.SetSelectedGameObject(CluesButton);
    }

    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseGameMenu() {
        closeAllMenu();
        eventSystem.SetSelectedGameObject(null);
        GameManager.Instance.GameMenuOpen = false;
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
    /// Set all menus to inactive
    /// </summary>
    private void closeAllMenu() {
        foreach(GameObject menu in Menus) {
            menu.SetActive(false);
        }
    }
}
