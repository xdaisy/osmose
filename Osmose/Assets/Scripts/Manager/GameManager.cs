using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A manager that handles game related data
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [Header("Party")]
    public Sprite ArenSprite;
    public Sprite ReySprite;
    public Sprite NaoiseSprite;

    [Header("Game Status")]
    public bool GameMenuOpen;
    public bool DialogActive;
    public bool FadingBetweenAreas;
    public bool InCutscene;
    public bool OnMap;
    public bool OnMainMenu;
    public string CurrentScene;
    public string PreviousScene;

    private List<string> party;

    private List<string> currentClues = new List<string> {
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
    };
    private List<string> pastClues = new List<string> {
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
        "Test Clue 2",
        "Test Clue",
    };
    private Dictionary<string, List<string>> past = new Dictionary<string, List<string>> ();

    public List<Clue> allClues;

    private float magicMeter = 1f;
    private float playTime = 0f;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
            party = new List<string>();
            party.Add(Constants.AREN);
            party.Add(Constants.REY);
            party.Add(Constants.NAOISE);
            past.Add("Test_Chapter", pastClues);
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (GameMenuOpen || DialogActive || FadingBetweenAreas /*|| InBattle*/ || InCutscene || OnMap || OnMainMenu) {
            PlayerControls.Instance.SetCanMove(false);
        } else {
            PlayerControls.Instance.SetCanMove(true);
        }
    }

    /// <summary>
    /// Get flag on if can open menu
    /// </summary>
    /// <returns>True if can open menu, false otherwise</returns>
    public bool CanOpenMenu() {
        return !DialogActive && !FadingBetweenAreas && !InCutscene && !OnMap && !OnMainMenu;
    }

    /// <summary>
    /// Get flag on if can show dialogue
    /// </summary>
    /// <returns>True if can show dialogue, false otherwise</returns>
    public bool CanStartDialogue() {
        return !FadingBetweenAreas && !InCutscene && !GameMenuOpen && !OnMap && !OnMainMenu;
    }

    /// <summary>
    /// Get the current party
    /// </summary>
    /// <returns>List of the current party members</returns>
    public List<string> GetCurrentParty() {
        return new List<string>(party);
    }

    public void ChangeMembers(List<string> party) {
        this.party = party;
    }

    /// <summary>
    /// Get the character's sprite
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Sprite of the character, null if the character is not one of the three main characters</returns>
    public Sprite GetCharSprite(string name) {
        if (name == Constants.AREN) {
            return this.ArenSprite;
        } else if (name == Constants.REY) {
            return this.ReySprite;
        } else if (name == Constants.NAOISE) {
            return this.NaoiseSprite;
        }
        return null;
    }

    /// <summary>
    /// Get the magic meter
    /// </summary>
    /// <returns>Magic meter</returns>
    public float GetMagicMeter() {
        return magicMeter;
    }

    /// <summary>
    /// Set the magic meter
    /// </summary>
    /// <param name="magic">Current amount of the magic meter</param>
    public void SetMagicMeter(float magic) {
        magicMeter = magic;
        // cannot be under 0f
        magicMeter = Mathf.Max(magicMeter, 0f);
        // cannot be over 1f
        magicMeter = Mathf.Min(magicMeter, 1f);
    }

    /// <summary>
    /// Get the total play time
    /// </summary>
    /// <returns>The total play time</returns>
    public float GetPlayTime() {
        return playTime;
    }

    /// <summary>
    /// Set the play time when loading save
    /// </summary>
    /// <param name="time">Play time of the save file</param>
    public void SetPlayTIme(float time) {
        this.playTime = time;
    }

    /// <summary>
    /// Get the current clue at the index position
    /// </summary>
    /// <param name="index">Index of the clue</param>
    /// <returns>Current clue at index position</returns>
    public Clue GetCurrentClueAt(int index) {
        if (index >= currentClues.Count) {
            return null;
        }
        return findClue(currentClues[index]);
    }

    /// <summary>
    /// Get the past clue at the index position
    /// </summary>
    /// <param name="index">Index of the clue</param>
    /// <returns>Past clue at index position</returns>
    public Clue GetPastClueAt(string chapter, int index) {
        if (!past.ContainsKey(chapter) ||  index >= past[chapter].Count) {
            return null;
        }
        return findClue(past[chapter][index]);
    }

    /// <summary>
    /// Get the clue with the name
    /// </summary>
    /// <param name="clueName">Name of the clue</param>
    /// <returns>Clue with the name</returns>
    public Clue GetClueWithName(string clueName) {
        return findClue(clueName);
    }

    /// <summary>
    /// Get a list of previous Chapters
    /// </summary>
    /// <returns>List of the previous Chapters</returns>
    public List<string> GetPastChapters() {
        return new List<string>(past.Keys);
    }

    /// <summary>
    /// Find the Clue object with the clue name
    /// </summary>
    /// <param name="clueName">Name of the clue</param>
    /// <returns>Clue object with the clue name</returns>
    private Clue findClue(string clueName) {
        return allClues.Find((clue) => clue.GetName().Equals(clueName));
    }
}
