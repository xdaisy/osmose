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
    private bool seenTutorial;

    [Header("Clues")]
    public List<Clue> allClues;

    private List<string> party;

    private string currentChapter = "ArenPrologue";
    //private List<string> currentClues = new List<string>();
    private List<string> currentClues = new List<string> {
        "Claw marks",
        "Magic Circle",
        "Piece of Fabric",
        "Footprints",
        "Rory's Boots",
        "Tuft of Fur",
        "Testimony from a witness 1",
        "Testimony from witness 2"
    };
    private Dictionary<string, List<string>> past = new Dictionary<string, List<string>> ();
    
    private float playTime = 0f;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
            party = new List<string>();
            party.Add(Constants.AREN);
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (GameMenuOpen || DialogActive || FadingBetweenAreas || InCutscene || OnMap || OnMainMenu) {
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
    /// Get the current chapter
    /// </summary>
    /// <returns>Name of the current chapter</returns>
    public string GetCurrentChapter() {
        return this.currentChapter;
    }

    /// <summary>
    /// Add the clue to the current clues array
    /// </summary>
    /// <param name="clue">Clue that is being added</param>
    public void AddClue(Clue clue) {
        currentClues.Add(clue.GetName());
    }

    /// <summary>
    /// Set the current chapter
    /// </summary>
    /// <param name="chapter">Name of the current chapter</param>
    public void SetCurrentChapter(string chapter) {
        this.currentChapter = chapter;
    }

    /// <summary>
    /// Get the current clue at the index position
    /// </summary>
    /// <param name="index">Index of the clue</param>
    /// <returns>Current clue at index position</returns>
    public Clue GetCurrentClueAt(int index) {
        if (index >= currentClues.Count) return null;
        return CluesManager.Instance.GetClue(currentChapter, currentClues[index]);
    }

    /// <summary>
    /// Get all current clues
    /// </summary>
    /// <returns>List of all current clues</returns>
    public List<string> GetCurrentClues() {
        return new List<string>(currentClues);
    }

    /// <summary>
    /// Set the current clues
    /// </summary>
    /// <param name="clues">List of the current clues</param>
    public void SetCurrentClues(List<string> clues) {
        this.currentClues = new List<string>(clues);
    }

    /// <summary>
    /// Get the past clue at the index position
    /// </summary>
    /// <param name="index">Index of the clue</param>
    /// <returns>Past clue at index position</returns>
    public Clue GetPastClueAt(string chapter, int index) {
        if (!past.ContainsKey(chapter) || index >= past[chapter].Count) return null;
        return CluesManager.Instance.GetClue(chapter, past[chapter][index]);
    }

    /// <summary>
    /// Get all clues from a chapter
    /// </summary>
    /// <param name="chapter">Chapter name</param>
    /// <returns>List of clues from a chapter</returns>
    public List<string> GetChapterClues(string chapter) {
        if (!past.ContainsKey(chapter)) {
            return null;
        }
        return new List<string>(past[chapter]);
    }

    /// <summary>
    /// Set the clues for the chapter
    /// </summary>
    /// <param name="chapter">Name of the chapter</param>
    /// <param name="clues">List of clues for the chapter</param>
    public void SetChapterClues(string chapter, List<string> clues) {
        if (clues != null) {
        past.Add(chapter, new List<string>(clues));
        }
    }

    /// <summary>
    /// Get the clue with the name
    /// </summary>
    /// <param name="chapter">Name of the chapter</param>
    /// <param name="clueName">Name of the clue</param>
    /// <returns>Clue with the name</returns>
    public Clue GetClueWithName(string chapter, string clueName) {
        return CluesManager.Instance.GetClue(chapter, clueName);
    }

    /// <summary>
    /// Get a list of previous Chapters
    /// </summary>
    /// <returns>List of the previous Chapters</returns>
    public List<string> GetPastChapters() {
        return new List<string>(past.Keys);
    }

    /// <summary>
    /// Get whether or not the leader is the name passed in
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>True if the person is the leader, false otherwise</returns>
    public bool IsLeader(string name) {
        return party[0].Equals(name);
    }

    /// <summary>
    /// Get whether or not the player has seen the tutorial
    /// </summary>
    /// <returns></returns>
    public bool DidSeeTutorial() {
        return seenTutorial;
    }

    /// <summary>
    /// The player has seen the tutorial
    /// </summary>
    public void SawTutorial() {
        seenTutorial = true;
    }
}
