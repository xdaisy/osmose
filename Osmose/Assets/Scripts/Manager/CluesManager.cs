using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluesManager : MonoBehaviour {
    public static CluesManager Instance;

    [SerializeField] private bool[] obtainedClues; // List of bool to indicate whether or not the clue was obtained
    [SerializeField] private bool[] updatedClues; // list of bool to indicate if the clue was updated
    [SerializeField] private Clue[] arenPrologueClues; // list of all clues in aren's prologue

    private Dictionary<string, Clue[]> clues;


    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
            clues = new Dictionary<string, Clue[]>();
            clues.Add(Constants.AREN_PROLOGUE, arenPrologueClues);
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Load in save data's obtained clues
    /// </summary>
    /// <param name="saveObtainedClues">Obtained clues array from the save data</param>
    public void LoadObtainedClues(bool[] saveObtainedClues) {
        saveObtainedClues.CopyTo(this.obtainedClues, 0);
    }

    /// <summary>
    /// Get the file's obtained clues array
    /// </summary>
    /// <returns>Array of the obtained clues</returns>
    public bool[] GetObtainedClues() {
        return this.obtainedClues;
    }

    /// <summary>
    /// Load in save data's updated clues
    /// </summary>
    /// <param name="saveUpdatedClues">Updated clues array from the save data</param>
    public void LoadUpdatedClues(bool[] saveUpdatedClues) {
        saveUpdatedClues.CopyTo(this.updatedClues, 0);
    }

    /// <summary>
    /// Get the file's updated clues array
    /// </summary>
    /// <returns>Array of the updated clues</returns>
    public bool[] GetUpdatedClues() {
        return this.updatedClues;
    }

    /// <summary>
    /// Get the clue from the chapter
    /// </summary>
    /// <param name="chapter">Chapter that the clue is in</param>
    /// <param name="clueName">Name of the clue</param>
    /// <returns></returns>
    public Clue GetClue(string chapter, string clueName) {
        if (!clues.ContainsKey(chapter)) {
            return null;
        }
        Clue[] chapterClues = clues[chapter];
        return Array.Find(chapterClues, (clue) => clue.GetName().Equals(clueName));
    }

    /// <summary>
    /// Get whether or not the player has obtained the clue
    /// </summary>
    /// <param name="clueIndex">Index of the clue</param>
    /// <returns>True if the player has obtained the clue, false otherwise</returns>
    public bool DidObtainClue(int clueIndex) {
        if (clueIndex >= obtainedClues.Length) {
            return false;
        }
        return obtainedClues[clueIndex];
    }

    /// <summary>
    /// Set the flag to obtain the clue
    /// </summary>
    /// <param name="clueIndex">Index of the clue in the array</param>
    public void ObtainedClue(int clueIndex) {
        if (clueIndex >= obtainedClues.Length) {
            Debug.LogError("Clue Index (" + clueIndex + ") does not exist");
            return;
        }
        obtainedClues[clueIndex] = true;
    }

    /// <summary>
    /// Get whether or not the clue was updated
    /// </summary>
    /// <param name="clueIndex">Index of the clue</param>
    /// <returns>True if clue was updated, false otherwise</returns>
    public bool DidUpdateClue(int clueIndex) {
        if (clueIndex >= updatedClues.Length) {
            return false;
        }
        return updatedClues[clueIndex];
    }

    /// <summary>
    /// Set the flag that the clue was updated
    /// </summary>
    /// <param name="clueIndex">Index of the clue</param>
    public void UpdateClue(int clueIndex) {
        if (clueIndex >= updatedClues.Length) {
            Debug.LogError("Clue Index (" + clueIndex + ") does not exist");
            return;
        }
        updatedClues[clueIndex] = true;
    }
}
